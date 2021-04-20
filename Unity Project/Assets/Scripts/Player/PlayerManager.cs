using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using ExitGames.Client.Photon;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//red = home
//blue = away

/// <summary>
/// Class for managing players throughout their time in a game, instance of playerManagers are only destroyed upon leaving a room
/// </summary>
public class PlayerManager : MonoBehaviourPunCallbacks, IOnEventCallback, IInRoomCallbacks
{
    #region Vars
    #region Menus
    [SerializeField] MenuManager GameMenus;
    [SerializeField] Menu Respawn, Pause;
    private bool pauseMenu = false;
    #endregion

    #region Photon
    public PhotonView PV;
    GameObject controller;
    PlayerStats localPlayerStats;
    public List<PlayerStats> playerStats = new List<PlayerStats>();
    public int myIndex;
    Hashtable customRoomProperties;
    #endregion

    #region States
    bool activeController = false;
    public GameState state = GameState.Waiting;
    public bool perpetual;
    private bool playerAdded;
    #endregion

    private int currentMatchTime;
    private Coroutine matchTimerCoroutine;
    private int matchLength = 600000;
    private int scoreCheck = 0;

    private List<string> weaponNamesMaster = new List<string>(5){ "MA37 Rifle", "M6G Pistol", "SRS99-AM Sniper", "M41 Launcher", "M45 Shotgun" };
    private List<string> weaponNames = new List<string>();
    private List<int> weaponIndex = new List<int>();
    [HideInInspector] public int primaryWeaponPM;
    [HideInInspector] public int secondaryWeaponPM;
    private bool allWeapons;
    private int redScoreCount = 0;
    private int blueScoreCount = 0;

    private List<GameObject> vehicles = new List<GameObject>();

    [SerializeField] GameObject[] items;
    #region UI
    //Text label variables
    [SerializeField] TMP_Text kills, deaths, map, gameType, timer, blueScoreText, redScoreText, endKills, endDeaths, endPlayer, endBlueScore, endRedScore, endTeam;
    public TMP_Text ammoCounter;

    //Leaderboard
    [SerializeField] Transform leaderBoard, endGame;
    [SerializeField] GameObject statsCard, endPlayerCard, endTeamCard;

    //HUD
    public GameObject HUD, primary, secondary, depletedShields;
    public Slider shields, blueScoreSlider, redScoreSlider;
    [SerializeField] TMP_Dropdown primaryDropdown, secondaryDropdown;
    #endregion
    #endregion

    #region Awake, Start, Update
    /// <summary>
    /// Method called when this class is referenced, assigns the Photon View to PM's PV reference var
    /// </summary>
    private void Awake()
    {
        perpetual = true;
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            boot.bootObject.localPV = PV;
        }
        customRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
    }

    /// <summary>
    /// Method Triggerd on 1st frame to setup PlayerManager baselines
    /// </summary>
    private void Start()
    {
        //Set the local gameMode 
        GameSettings.GameMode = (GameMode)(int)customRoomProperties["GameType"];
        
        //Remove other player's charater's instances of menus
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            Destroy(HUD);
        }
        else
        {
            //Setup weapon selection
            //record if we have access to all of the weapons in the game
            if (customRoomProperties.ContainsKey("AllWeapons"))
                allWeapons = (bool)customRoomProperties["AllWeapons"];
            else
                allWeapons = true;

            if (allWeapons)
            {
                //Make sure that the 2 weapon settings are inactive
                primary.SetActive(false);
                secondary.SetActive(false);
            }
            else
            {
                //Make sure that the 2 weapon settings are active
                primary.SetActive(true);
                secondary.SetActive(true);

                //Compare weapons in the hashtable to a master list that each player manager holds
                int i = 0;
                foreach(string s in weaponNamesMaster)
                {
                    if (customRoomProperties.ContainsKey(s))
                    {
                        weaponNames.Add(s);
                        weaponIndex.Add(i);
                    }
                    i++;
                }

                //Clear dropdowns
                primaryDropdown.ClearOptions();
                secondaryDropdown.ClearOptions();

                //Set dropdowns to available weapons
                primaryDropdown.AddOptions(weaponNames);
                secondaryDropdown.AddOptions(weaponNames);
            }

            //Method is nested here to create vehicles once when the master client playerManager starts
            if (PhotonNetwork.IsMasterClient)
            {
                CreateVehicles();
            }
        }

        //Refresh UI and Start Match
        refreshStats();
        InitializeMatch();

        //Check to see if the master client is running on this script
        if (PhotonNetwork.IsMasterClient)
        {
            //If it is, send the master client's player manager the new player's information
            NewPlayer_S();
            playerAdded = true;
            openMM(Respawn);
        }
    }

    /// <summary>
    /// Update method runs with framerate and calls the toggle pause method
    /// </summary>
    private void Update()
    {
        //Exit the method if this isn't my playerManager
        if (!PV.IsMine)
            return;

        //Exit the method if the game is ending
        if (state == GameState.Ending)
            return;

        Debug.Log(state);
        //Check if the player is paused/pausing
        togglePause();

        //Display the Leaderboard if the tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (leaderBoard.gameObject.activeSelf) leaderBoard.gameObject.SetActive(false);
            else Leaderboard(leaderBoard);
        }

        //If there is an acitve controller
        if(controller != null)
        {
            //Set all of the weapon overlays off
            for(int i = 0; i < items.Length; i++)
            {
                if(i != controller.GetComponent<PlayerControllerModelled>().itemIndex)
                {
                    items[i].SetActive(false);
                }
            }

            //Activate the weapon overlay for the current weapon
            items[controller.GetComponent<PlayerControllerModelled>().itemIndex].SetActive(true);
        }
    }
    #endregion

    #region Enable/Disable
    /// <summary>
    /// Method subscribes this object to the Photon Network when enabled
    /// </summary>
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    /// <summary>
    /// Method unsubscribes this object to the Photon Network when disabled
    /// </summary>
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion

    #region Enums
    /// <summary>
    /// Enums for the Photon Event system of players
    /// </summary>
    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeStat,
        NewMatch,
        RefreshTimer
    }

    /// <summary>
    /// Enums for the State of the Match
    /// </summary>
    public enum GameState
    {
        Waiting = 0,
        Starting = 1,
        Playing = 2,
        Ending = 3
    }
    #endregion

    #region Creation and Death
    /// <summary>
    /// Method creates a new player controller when called
    /// </summary>
    public void CreateNewController()
    {
        //Verify this is my playerManager
        if (PV.IsMine)
        {
            //get a random spawnpoint
            Transform spawnpoint = this.transform;

            //Based on FFA rules
            if (GameSettings.GameMode == GameMode.FFA)
            {
                spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            }
            //Based on TDM rules
            else if (GameSettings.GameMode == GameMode.TDM)
            {
                if (GameSettings.IsBlueTeam)
                {
                    spawnpoint = SpawnManager.Instance.GetBlueSpawnpoint();
                }
                else
                {
                    spawnpoint = SpawnManager.Instance.GetRedSpawnpoint();
                }
            }

            //Get the weapons if applicable
            if (!allWeapons)
            {
                primaryWeaponPM = weaponIndex[primaryDropdown.value];
                secondaryWeaponPM = weaponIndex[secondaryDropdown.value];
            }

            //create a new controller at the spawnpoint prefab loaction
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerModelled"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });

            //old non-spawnpoint reliant spawn code kept here as a backup
            //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerModelled"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });

            //record an active playerController and close the respawn menu
            activeController = true;
            HUD.SetActive(true);
            closeMM(Respawn);
        }
    }

    /// <summary>
    /// Method destroys the player controller, opens respawn menu, and sets active controller to false
    /// </summary>
    public void Die()
    {
        //Remove controller
        activeController = false;
        PhotonNetwork.Destroy(controller);

        //Set spawn menu to active
        HUD.SetActive(false);
        primaryDropdown.value = primaryWeaponPM;
        secondaryDropdown.value = secondaryWeaponPM;
        openMM(Respawn);
        pauseMenu = false;

        //Tell master client we died
        ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
    }
    #endregion

    #region Pause
    /// <summary>
    /// Method opens or closes the pause menu when escape key is pressed
    /// </summary>
    private void togglePause()
    {
        //Escape was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PlayerManager is mine
            if (PV.IsMine)
            {
                //We aren't playing
                if (state != GameState.Playing)
                {
                    //We are currently in the game
                    if (activeController)
                    {
                        //Close pause
                        closeMM(Pause);
                    }
                    else
                    {
                        //If we are paused
                        if (pauseMenu)
                        {
                            //Open respawn
                            openMM(Respawn);
                            pauseMenu = false;
                        }
                        else
                        {
                            //Open Pause
                            openMM(Pause);
                            pauseMenu = true;
                        }
                    }
                }
                //Open Pause Menu
                else
                {
                    openMM(Pause);
                    pauseMenu = true;
                }

            }
        }
    }

    /// <summary>
    /// Method which exits pause when called. Made specifically for the resume button in the pause menu
    /// </summary>
    public void exitPause()
    {
        if (PV.IsMine)
        {
            //logic statment to evaluate if the respawn menu needs to be opened when the pause menu is closed
            if (activeController)
            {
                closeMM(Pause);
            }
            else
            {
                openMM(Respawn);
            }
        }
    }
    #endregion

    #region Menus
    /// <summary>
    /// Private method which closes the passed menu, locks the cursor to the screen, makes the cursor and menu background invisable, and sets pause bool to false
    /// </summary>
    /// <param name="menuName"></param>
    private void closeMM(Menu menuName)
    {
        if (!PV.IsMine)
            return;
        state = GameState.Playing;
        GameMenus.CloseMenu(menuName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameMenus.GetComponent<Image>().enabled = false;
    }

    /// <summary>
    /// Private method which opens the passed menu, unlocks the cursor, makes the cursor and menu background visable, and sets pause bool to true
    /// </summary>
    /// <param name="menuName"></param>
    private void openMM(Menu menuName)
    {
        if (!PV.IsMine)
            return;
        leaderBoard.gameObject.SetActive(false);
        state = GameState.Waiting;
        GameMenus.OpenMenu(menuName);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.GetComponent<Image>().enabled = true;
    }

    /// <summary>
    /// Public method to force a player to return to the lobby
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Kill Methods
    public void killedPlayer(int shooter)
    {
        PhotonView.Find(shooter).RPC("RPC_KilledPlayer", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_KilledPlayer()
    {
        if (!PV.IsMine)
            return;
        ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 0, 1);
    }
    #endregion

    #region Refresh Methods
    /// <summary>
    /// Method updates leaderboard with my current stats
    /// </summary>
    private void refreshStats()
    {
        if (playerStats.Count > myIndex)
        {
            kills.text = $"{playerStats[myIndex].kills} kills";
            deaths.text = $"{playerStats[myIndex].deaths} deaths";
        }
        else
        {
            kills.text = "0 kills";
            deaths.text = "0 deaths";
        }
    }

    private void RefreshTimerUI()
    {
        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");
        timer.text = $"{minutes}:{seconds}";
    }

    /// <summary>
    /// Method updates the blueScoreCount and RedScoreCount Variables
    /// </summary>
    private void UpdateTeamScores()
    {
        int blueKills = 0;
        int redKills = 0;

        // set scores
        foreach (PlayerStats p in playerStats)
        {
            if (p.blueTeam)
            {
                blueKills += p.kills;
            }
            else
            {
                redKills += p.kills;
            }
        }

        redScoreCount = redKills;
        blueScoreCount = blueKills;

        if (GameSettings.GameMode == GameMode.TDM)
        {
            blueScoreSlider.value = blueScoreCount;
            redScoreSlider.value = redScoreCount;
        }
    }
    #endregion

    #region Leaderboard
    private void Leaderboard(Transform p_lb)
    { 
        // clean up
        for (int i = 2; i < p_lb.childCount; i++)
        {
            Destroy(p_lb.GetChild(i).gameObject);
        }

        // set details
        if(GameSettings.GameMode == GameMode.FFA)
        {
            gameType.text = "Free For All";
            blueScoreText.gameObject.SetActive(false);
            redScoreText.gameObject.SetActive(false);
        }
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            gameType.text = "Team Deathmatch";

            //Set Scores
            blueScoreText.text = blueScoreCount.ToString();
            redScoreText.text = redScoreCount.ToString();
        }

        map.text = SceneManager.GetActiveScene().name;

        // cache prefab
        GameObject playercard = p_lb.GetChild(1).gameObject;
        playercard.SetActive(false);

        // sort
        List<PlayerStats> sorted = SortPlayers(playerStats);

        // display
        foreach (PlayerStats a in sorted)
        {
            GameObject newcard = Instantiate(playercard, p_lb) as GameObject;

            newcard.transform.Find("Username").GetComponent<TMP_Text>().text = a.username;
            newcard.transform.Find("Kills Counter").GetComponent<TMP_Text>().text = a.kills.ToString();
            newcard.transform.Find("Deaths Counter").GetComponent<TMP_Text>().text = a.deaths.ToString();

            if(GameSettings.GameMode == GameMode.TDM)
            {
                if (a.blueTeam)
                {
                    newcard.transform.Find("Base").GetComponent<Image>().color = new Color32(27, 27, 133, 255);
                }
                else
                {
                    newcard.transform.Find("Base").GetComponent<Image>().color = new Color32(133, 27, 27, 255);
                }
            }

            newcard.SetActive(true);
        }

        // activate
        p_lb.gameObject.SetActive(true);
        p_lb.parent.gameObject.SetActive(true);
    }

    private List<PlayerStats> SortPlayers(List<PlayerStats> p_info)
    {
        List<PlayerStats> sorted = new List<PlayerStats>();

        if (GameSettings.GameMode == GameMode.FFA)
        {
            while (sorted.Count < p_info.Count)
            {
                // set defaults
                short highest = -1;
                PlayerStats selection = p_info[0];

                // grab next highest player
                foreach (PlayerStats a in p_info)
                {
                    if (sorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                sorted.Add(selection);
            }
        } 
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            List<PlayerStats> redSorted = new List<PlayerStats>();
            List<PlayerStats> blueSorted = new List<PlayerStats>();

            int blueSize = 0;
            int redSize = 0;

            foreach (PlayerStats p in p_info)
            {
                if (p.blueTeam) blueSize++;
                else redSize++;
            }

            while (redSorted.Count < redSize)
            {
                // set defaults
                short highest = -1;
                PlayerStats selection = p_info[0];

                // grab next highest player
                foreach (PlayerStats a in p_info)
                {
                    if (a.blueTeam) continue;
                    if (redSorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                redSorted.Add(selection);
            }

            while (blueSorted.Count < blueSize)
            {
                // set defaults
                short highest = -1;
                PlayerStats selection = p_info[0];

                // grab next highest player
                foreach (PlayerStats a in p_info)
                {
                    if (!a.blueTeam) continue;
                    if (blueSorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                blueSorted.Add(selection);
            }

            sorted.AddRange(redSorted);
            sorted.AddRange(blueSorted);
        }
        return sorted;
    }
    #endregion

    #region Checks
    private void StateCheck()
    {
        if (state == GameState.Ending)
        {
            EndGame();
        }
    }

    /// <summary>
    /// Method to Check if a player or team has reached the score total
    /// </summary>
    private void ScoreCheck()
    {
        //Exit method if the match isn't dependant on a certain kill amount
        if (scoreCheck == 0)
            return;

        // define temporary variables
        bool detectwin = false;

        //FFA
        if (GameSettings.GameMode == GameMode.FFA)
        {
            // check to see if any player has met the win conditions
            foreach (PlayerStats a in playerStats)
            {
                if (a.kills >= scoreCheck)
                {
                    detectwin = true;
                    break;
                }
            }
        }
        //TDM
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            //Call method to calculate team scores
            UpdateTeamScores();

            //Check if team scores meet threshold
            if ((blueScoreCount >= scoreCheck) || (redScoreCount >= scoreCheck))
            {
                detectwin = true;
            }
        }

        // did we find a winner?
        if (detectwin)
        {
            // are we the master client? is the game still going?
            if (PhotonNetwork.IsMasterClient && state != GameState.Ending)
            {
                // if so, tell the other players that a winner has been detected
                UpdatePlayers_S((int)GameState.Ending, playerStats);
            }
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient && state != GameState.Ending)
        {
            List<PlayerStats> newPlayerStats = new List<PlayerStats>();
            foreach (PlayerStats s in playerStats)
            {
                /*
                Debug.Log(PhotonView.Find(s.viewID).IsOwnerActive);
                if (!PhotonView.Find(s.viewID).IsOwnerActive)
                {
                    playerStats.Remove(s);
                }
                */
                if(otherPlayer.ActorNumber != s.actor)
                {
                    newPlayerStats.Add(s);
                }
            }

            playerStats = newPlayerStats;
            UpdatePlayers_S((int)state, playerStats);
        }
    }
    #endregion

    #region Random
    private void EndGame()
    {
        // set game state to ending
        state = GameState.Ending;

        leaderBoard.gameObject.SetActive(false);

        // set timer to 0
        if (matchTimerCoroutine != null) StopCoroutine(matchTimerCoroutine);
        currentMatchTime = 0;
        RefreshTimerUI();


        // disable room
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(controller);
            if (!perpetual)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        //Run commands to turn off all menus
        GameMenus.CloseAllMenus();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.GetComponent<Image>().enabled = false;

        //Display FFA Winner
        if (GameSettings.GameMode == GameMode.FFA)
        {
            endPlayerCard.SetActive(true);
            int highest = -1;
            PlayerStats selection = playerStats[0];
            foreach (PlayerStats a in playerStats)
            {
                if (a.kills > highest)
                {
                    selection = a;
                    highest = a.kills;
                }
            }

            endDeaths.text = selection.deaths.ToString();
            endKills.text = selection.kills.ToString();
            endPlayer.text = selection.username;
        }
        //Display TDM Winner
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            endTeamCard.SetActive(true);
            UpdateTeamScores();

            endBlueScore.text = blueScoreCount.ToString();
            endRedScore.text = redScoreCount.ToString();

            if (blueScoreCount > redScoreCount)
            {
                endTeam.text = "Blue Team Wins";
                endTeam.color = new Color(0, 0, 255);
            }
            else if (blueScoreCount < redScoreCount)
            {
                endTeam.text = "Red Team Wins";
                endTeam.color = new Color(255, 0, 0);
            }
            else
            {
                endTeam.text = "Draw";
                endTeam.color = new Color(255, 255, 255);
            }
        }

        // show end game ui
        endGame.gameObject.SetActive(true);
        Leaderboard(leaderBoard);

        // wait X seconds and then return to main menu
        StartCoroutine(End(6f));
    }

    private void InitializeMatch()
    {
        //Code sets and or disables match timer depending on room parameters
        if (customRoomProperties.ContainsKey("MatchLength"))
        {
            if((int)customRoomProperties["MatchLength"] == 0)
            {
                timer.gameObject.SetActive(false);
            }
            else
            {
                matchLength = (int)customRoomProperties["MatchLength"] * 5 * 60;
                currentMatchTime = matchLength;
                RefreshTimerUI();

                if (PhotonNetwork.IsMasterClient)
                {
                    matchTimerCoroutine = StartCoroutine(MatchTimer());
                }
            }
        } 
        else
        {
            timer.text = "ERROR";
        }

        //Game sets and or disables score checking based on room parameters
        if (GameSettings.GameMode != GameMode.TDM)
        {
            blueScoreSlider.gameObject.SetActive(false);
            redScoreSlider.gameObject.SetActive(false);
        }

        if (customRoomProperties.ContainsKey("ScoreCheck"))
        {
            scoreCheck = (int)customRoomProperties["ScoreCheck"] * 10;

            if (GameSettings.GameMode == GameMode.TDM)
            {
                blueScoreSlider.maxValue = scoreCheck;
                redScoreSlider.maxValue = scoreCheck;

                if(scoreCheck == 0)
                {
                    blueScoreSlider.gameObject.SetActive(false);
                    redScoreSlider.gameObject.SetActive(false);
                }
            }
        }
    }
    
    /// <summary>
    /// Method calulates which team a player should be on via their actornumber
    /// </summary>
    /// <param name="ActorNumber"></param>
    /// <returns></returns>
    private bool CalculateTeam(int ActorNumber)
    {
        //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        return ActorNumber % 2 == 0;
    }

    /// <summary>
    /// Method can be called to spawn vehicles in and update the master list of vehicles to newly created vehicles
    /// </summary>
    private void CreateVehicles()
    {
        //Check if the spawn manager allows for vehicles to be spawned
        if (SpawnManager.Instance.hasVehicles)
        {
            //Get spawnpoints and add a new vehicle at the spawn point
            VehicleSpawnpoint[] points = SpawnManager.Instance.GetVehiclePoint();

            foreach(VehicleSpawnpoint p in points)
            {
                GameObject temp = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Warhog"), p.transform.position, p.transform.rotation, 0, null);
                vehicles.Add(temp);
            }
        }
    }
    #endregion

    #region Photon Events

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code >= 200) return;

        EventCodes e = (EventCodes)photonEvent.Code;
        object[] o = (object[])photonEvent.CustomData;

        switch (e)
        {
            case EventCodes.NewPlayer:
                NewPlayer_R(o);
                break;

            case EventCodes.UpdatePlayers:
                UpdatePlayers_R(o);
                break;

            case EventCodes.ChangeStat:
                ChangeStat_R(o);
                break;

            case EventCodes.NewMatch:
                NewMatch_R();
                break;

            case EventCodes.RefreshTimer:
                RefreshTimer_R(o);
                break;
        }
    }

    #region NewPlayer
    public void NewPlayer_S()
    {
        object[] package = new object[5];

        package[0] = PV.Owner.NickName;
        package[1] = PV.Owner.ActorNumber;// PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = (short)0;
        package[3] = (short)0;
        package[4] = CalculateTeam(PV.Owner.ActorNumber);

        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, new SendOptions { Reliability = true });
    }

    public void NewPlayer_R(object[] data)
    {
        PlayerStats p = new PlayerStats((string)data[0], (int)data[1], (short)data[2], (short)data[3], (bool)data[4]);

        playerStats.Add(p);
        
        //resync our local player information with the new player
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            gameObject.GetComponent<PlayerControllerModelled>().TrySync();
        }
        
        if(PhotonNetwork.IsMasterClient && PV.IsMine)
        {
            UpdatePlayers_S((int)state, playerStats);
        }
    }
    #endregion

    #region UpdatePlayers
    public void UpdatePlayers_S(int state, List<PlayerStats> info)
    {
        object[] package = new object[info.Count + 1];

        package[0] = state;
        for (int i = 0; i < info.Count; i++)
        {
            object[] piece = new object[5];

            piece[0] = info[i].username;
            piece[1] = info[i].actor;
            piece[2] = info[i].kills;
            piece[3] = info[i].deaths;
            piece[4] = info[i].blueTeam;

            package[i + 1] = piece;
        }

        PhotonNetwork.RaiseEvent((byte)EventCodes.UpdatePlayers, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    public void UpdatePlayers_R(object[] data)
    {
        state = (GameState)data[0];

        playerStats = new List<PlayerStats>();

        for (int i = 1; i < data.Length; i++)
        {
            object[] extract = (object[])data[i];

            PlayerStats p = new PlayerStats((string)extract[0], (int)extract[1], (short)extract[2], (short)extract[3], (bool)extract[4]);

            playerStats.Add(p);

            if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor)
            {
                myIndex = i - 1;

                //if we have been waiting to be added to the game then spawn us in
                if (!playerAdded)
                {
                    playerAdded = true;
                    GameSettings.IsBlueTeam = p.blueTeam;
                    openMM(Respawn);
                }
            }
        }

        StateCheck();
    }
    #endregion

    #region ChangeStat
    public void ChangeStat_S(int actor, byte stat, byte amt)
    {
        object[] package = new object[] { actor, stat, amt };

        PhotonNetwork.RaiseEvent((byte)EventCodes.ChangeStat, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }
    public void ChangeStat_R(object[] data)
    {
        int actor = (int)data[0];
        byte stat = (byte)data[1];
        byte amt = (byte)data[2];

        for (int i = 0; i < playerStats.Count; i++)
        {
            if (playerStats[i].actor == actor)
            {
                switch (stat)
                {
                    case 0: //kills
                        playerStats[i].kills += amt;
//                        Debug.Log($"Player {playerStats[i].username} : kills = {playerStats[i].kills}");
                        break;

                    case 1: //deaths
                        playerStats[i].deaths += amt;
 //                       Debug.Log($"Player {playerStats[i].username} : deaths = {playerStats[i].deaths}");
                        break;
                }

                if (i == myIndex) refreshStats();
                if (!PV.IsMine) break;
                if (leaderBoard.gameObject.activeSelf) Leaderboard(leaderBoard);

                break;
            }
        }

        if (GameSettings.GameMode == GameMode.TDM)
        {
            UpdateTeamScores();
        }

        ScoreCheck();
    }
    #endregion

    #region NewMatch
    public void NewMatch_S()
    {
        foreach(GameObject v in vehicles)
        {
            PhotonNetwork.Destroy(v);
        }

        CreateVehicles();
        PhotonNetwork.RaiseEvent((byte)EventCodes.NewMatch, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    public void NewMatch_R()
    {
        // set game state to waiting
        state = GameState.Waiting;

        // deactivate map camera
        //            mapcam.SetActive(false);

        // hide end game ui
        endGame.gameObject.SetActive(false);

        // reset scores
        foreach (PlayerStats p in playerStats)
        {
            p.kills = 0;
            p.deaths = 0;
        }
        redScoreCount = 0;
        blueScoreCount = 0;

        // reset ui
        refreshStats();

        // reinitialize match
        InitializeMatch();

        // spawn
        openMM(Respawn);
    }
    #endregion

    #region Timer
    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime };

        PhotonNetwork.RaiseEvent((byte)EventCodes.RefreshTimer, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }
    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0];
        RefreshTimerUI();
    }
    #endregion
    #endregion

    #region Coroutines
    private IEnumerator MatchTimer()
    {
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1;

        if (currentMatchTime <= 0)
        {
            matchTimerCoroutine = null;
            UpdatePlayers_S((int)GameState.Ending, playerStats);
        }
        else
        {
            RefreshTimer_S();
            matchTimerCoroutine = StartCoroutine(MatchTimer());
        }
    }

    private IEnumerator End(float p_wait)
    {
        yield return new WaitForSeconds(p_wait);

        if (perpetual)
        {
            // new match
            if (PhotonNetwork.IsMasterClient)
            {
                NewMatch_S();
            }
        }
        else
        {
            // disconnect
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
        }
    }
    #endregion
}
