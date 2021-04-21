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
    private List<GameObject> vehicles = new List<GameObject>();

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

    #region Length and Scores
    private int currentMatchTime;
    private Coroutine matchTimerCoroutine;
    private int matchLength = 600000;
    private int scoreCheck = 0;
    private int redScoreCount = 0;
    private int blueScoreCount = 0;
    #endregion

    #region Weapons
    [SerializeField] GameObject[] items;

    private List<string> weaponNamesMaster = new List<string>(5){ "MA37 Rifle", "M6G Pistol", "SRS99-AM Sniper", "M41 Launcher", "M45 Shotgun" };
    private List<string> weaponNames = new List<string>();
    private List<int> weaponIndex = new List<int>();

    [HideInInspector] public int primaryWeaponPM;
    [HideInInspector] public int secondaryWeaponPM;
    private bool allWeapons;
    #endregion

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
        //Check if this is my PlayerManager
        if (PV.IsMine)
        {
            //If we are alive
            if (activeController)
            {
                //Resume Game
                closeMM(Pause);
            }
            else
            {
                //Go to the respawn menu
                openMM(Respawn);
            }
        }
    }
    #endregion

    #region Menus
    /// <summary>
    /// Private method which closes the passed menu and executes other commands with that
    /// </summary>
    /// <param name="menuName">Menu object that we want to close</param>
    private void closeMM(Menu menuName)
    {
        //If this is my PlayerManager
        if (!PV.IsMine)
            return;

        //Start playing
        state = GameState.Playing;

        //Close this menu and background
        GameMenus.CloseMenu(menuName);
        GameMenus.GetComponent<Image>().enabled = false;

        //Lock the cursor to the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Private method which opens the passed menu and executes other commands with that
    /// </summary>
    /// <param name="menuName">Menu object that we want to open</param>
    private void openMM(Menu menuName)
    {
        //If this is my PlayerManager
        if (!PV.IsMine)
            return;

        //Open menu and ensusre only the menu is open
        leaderBoard.gameObject.SetActive(false);
        GameMenus.OpenMenu(menuName);
        GameMenus.GetComponent<Image>().enabled = true;

        //Enter pause state
        state = GameState.Waiting;
        
        //Unlock the Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
    /// <summary>
    /// Method calls RPC to tell another player that they killed this player
    /// </summary>
    /// <param name="shooter">The PhotonView ID of the player who killed this player</param>
    public void killedPlayer(int shooter)
    {
        //Call this RPC with the Shooter's PhotonView
        PhotonView.Find(shooter).RPC("RPC_KilledPlayer", RpcTarget.All);
    }


    /// <summary>
    /// RPC Method to register if this player killed another player
    /// </summary>
    [PunRPC]
    public void RPC_KilledPlayer()
    {
        //Exit this method if we are not the player who got the kill
        if (!PV.IsMine)
            return;

        //Call even to tell the master Client that we got a kill
        ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 0, 1);
    }
    #endregion

    #region Refresh Methods
    /// <summary>
    /// Method updates leaderboard with my current stats
    /// </summary>
    private void refreshStats()
    {
        //f my index is in the playerStats list
        if (playerStats.Count > myIndex)
        {
            //Display the most up to date kills and deaths
            kills.text = $"{playerStats[myIndex].kills} kills";
            deaths.text = $"{playerStats[myIndex].deaths} deaths";
        }
        else
        {
            //If we are not in the list the reset our counters
            kills.text = "0 kills";
            deaths.text = "0 deaths";
        }
    }

    /// <summary>
    /// Method refreshes the TimerUI
    /// </summary>
    private void RefreshTimerUI()
    {
        //Calculate the minutes
        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");

        //Display the remaining time
        timer.text = $"{minutes}:{seconds}";
    }

    /// <summary>
    /// Method updates the blueScoreCount and RedScoreCount Variables
    /// </summary>
    private void UpdateTeamScores()
    {
        //Disposable variables
        int blueKills = 0;
        int redKills = 0;

        // Iterate through the list and calculate the total kills
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

        //Save the calcualted kill counts
        redScoreCount = redKills;
        blueScoreCount = blueKills;

        //Update the score sliders
        if (GameSettings.GameMode == GameMode.TDM)
        {
            blueScoreSlider.value = blueScoreCount;
            redScoreSlider.value = redScoreCount;
        }
    }
    #endregion

    #region Leaderboard
    /// <summary>
    /// Method updates and creates the leaderboard with current stats when called
    /// </summary>
    /// <param name="p_lb">The transform that the leaderboard is activating</param>
    private void Leaderboard(Transform p_lb)
    { 
        //Clean up the old leaderboard, set to index after 2 as the first 2 children are the header card and template card
        for (int i = 2; i < p_lb.childCount; i++)
        {
            Destroy(p_lb.GetChild(i).gameObject);
        }

        //Change leaderbaord acording to the game mode
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

        //Set the map name
        map.text = SceneManager.GetActiveScene().name;

        //Find the template player card and save it
        GameObject playercard = p_lb.GetChild(1).gameObject;
        playercard.SetActive(false);

        //Sort the players
        List<PlayerStats> sorted = SortPlayers(playerStats);

        //Instantiate a player card for each player
        foreach (PlayerStats a in sorted)
        {
            //Create new card
            GameObject newcard = Instantiate(playercard, p_lb) as GameObject;

            //Set this card's stats
            newcard.transform.Find("Username").GetComponent<TMP_Text>().text = a.username;
            newcard.transform.Find("Kills Counter").GetComponent<TMP_Text>().text = a.kills.ToString();
            newcard.transform.Find("Deaths Counter").GetComponent<TMP_Text>().text = a.deaths.ToString();

            //If we are in TDM mode then change the background color of the card
            if(GameSettings.GameMode == GameMode.TDM)
            {
                //Blue team players get blue
                if (a.blueTeam)
                {
                    newcard.transform.Find("Base").GetComponent<Image>().color = new Color32(27, 27, 133, 255);
                }
                //Red team players get ret
                else
                {
                    newcard.transform.Find("Base").GetComponent<Image>().color = new Color32(133, 27, 27, 255);
                }
            }

            //Activate the new card
            newcard.SetActive(true);
        }

        //Activate the leaderboard
        p_lb.gameObject.SetActive(true);
        p_lb.parent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method to sort players based on team and kills
    /// </summary>
    /// <param name="p_info">The PlayerStats list that needs to be sorted</param>
    /// <returns></returns>
    private List<PlayerStats> SortPlayers(List<PlayerStats> p_info)
    {
        //Create a new temporary list
        List<PlayerStats> sorted = new List<PlayerStats>();

        //Check for FFA mode
        if (GameSettings.GameMode == GameMode.FFA)
        {
            //Keep running until the sorted list is equal in length to the passed playerStats list
            while (sorted.Count < p_info.Count)
            {
                //Set defaults
                short highest = -1;
                PlayerStats selection = p_info[0];

                //Iterate through all players
                foreach (PlayerStats a in p_info)
                {
                    //Exit this run if we already have this player sorted
                    if (sorted.Contains(a)) continue;

                    //Check if this player has a higher kill count
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                //Add player
                sorted.Add(selection);
            }
        } 
        //Check for TDM mode
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            //Create temporary lists for red and blue teams
            List<PlayerStats> redSorted = new List<PlayerStats>();
            List<PlayerStats> blueSorted = new List<PlayerStats>();

            //vars to insure that we get all players
            int blueSize = 0;
            int redSize = 0;

            //Iterate through the passed list and get the total players
            foreach (PlayerStats p in p_info)
            {
                if (p.blueTeam) blueSize++;
                else redSize++;
            }

            //Run while there are still users that haven't been counted for the red team
            while (redSorted.Count < redSize)
            {
                //Set defaults
                short highest = -1;
                PlayerStats selection = p_info[0];

                //Iterate through all players
                foreach (PlayerStats a in p_info)
                {
                    //Exit run if this player is on the blue team
                    if (a.blueTeam) continue;

                    //Exit run if this player is already sorted
                    if (redSorted.Contains(a)) continue;

                    //Check if this player has the highest kill count
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                //Add player
                redSorted.Add(selection);
            }
            //Run while there are still users that haven't been counted for the blue team
            while (blueSorted.Count < blueSize)
            {
                //Set defaults
                short highest = -1;
                PlayerStats selection = p_info[0];

                //Iterate through all playres
                foreach (PlayerStats a in p_info)
                {
                    //Exit run if player is on the red team
                    if (!a.blueTeam) continue;

                    //Exit run if this player has already been sorted
                    if (blueSorted.Contains(a)) continue;

                    //Check if this player has the highest kill count of the remaining players
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                //Add player
                blueSorted.Add(selection);
            }

            //Add blue and red lists together
            sorted.AddRange(redSorted);
            sorted.AddRange(blueSorted);
        }

        //Return the sorted list
        return sorted;
    }
    #endregion

    #region Checks
    /// <summary>
    /// Method to get that the State of the game is ending
    /// </summary>
    private void StateCheck()
    {
        if (state == GameState.Ending)
        {
            //If it is call the end game method
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

        //Define temporary variables
        bool detectwin = false;

        //FFA
        if (GameSettings.GameMode == GameMode.FFA)
        {
            //Check to see if any player has met the win conditions
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

        //Did we find a winner?
        if (detectwin)
        {
            //Are we the master client? is the game still going?
            if (PhotonNetwork.IsMasterClient && state != GameState.Ending)
            {
                //If so, tell the other players that a winner has been detected
                UpdatePlayers_S((int)GameState.Ending, playerStats);
            }
        }
    }

    /// <summary>
    /// Method to detect if a player leaves the match
    /// </summary>
    /// <param name="otherPlayer">The player that left the match</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Check if we are the master client and that the game isn't ending at the moment
        if (PhotonNetwork.IsMasterClient && state != GameState.Ending)
        {
            //Temporary list
            List<PlayerStats> newPlayerStats = new List<PlayerStats>();

            //Iterate through all of the players in playerStats
            foreach (PlayerStats s in playerStats)
            {
                //If this player's actor number doesn't match the actor number of the player who left
                if(otherPlayer.ActorNumber != s.actor)
                {
                    //Add them to the new temporary list
                    newPlayerStats.Add(s);
                }
            }

            //Update the playerStats list and tell other users to as well
            playerStats = newPlayerStats;
            UpdatePlayers_S((int)state, playerStats);
        }
    }
    #endregion

    #region Random
    /// <summary>
    /// Method to end a match
    /// </summary>
    private void EndGame()
    {
        // set game state to ending
        state = GameState.Ending;

        //Disable the leaderboard
        leaderBoard.gameObject.SetActive(false);

        //Set timer to 0 and disable the timer
        if (matchTimerCoroutine != null) StopCoroutine(matchTimerCoroutine);
        currentMatchTime = 0;
        RefreshTimerUI();


        //Disable room is need be and remove our player if they are active
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(controller);
            if (!perpetual)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        //Turn off all menus
        GameMenus.CloseAllMenus();
        GameMenus.GetComponent<Image>().enabled = false;

        //Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        //Display FFA Winner
        if (GameSettings.GameMode == GameMode.FFA)
        {
            //Activaet the FFA end screen
            endPlayerCard.SetActive(true);

            //Find the winner of the match
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

            //Update the text to display the winner
            endDeaths.text = selection.deaths.ToString();
            endKills.text = selection.kills.ToString();
            endPlayer.text = selection.username;
        }
        //Display TDM Winner
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            //Find the winning team
            endTeamCard.SetActive(true);
            UpdateTeamScores();

            //Display the final scores
            endBlueScore.text = blueScoreCount.ToString();
            endRedScore.text = redScoreCount.ToString();

            //Blue team wins
            if (blueScoreCount > redScoreCount)
            {
                endTeam.text = "Blue Team Wins";
                endTeam.color = new Color(0, 0, 255);
            }
            //Red team wins
            else if (blueScoreCount < redScoreCount)
            {
                endTeam.text = "Red Team Wins";
                endTeam.color = new Color(255, 0, 0);
            }
            //Draw
            else
            {
                endTeam.text = "Draw";
                endTeam.color = new Color(255, 255, 255);
            }
        }

        //Show end game ui
        endGame.gameObject.SetActive(true);
        Leaderboard(leaderBoard);

        //Wait X seconds and then return to main menu
        StartCoroutine(End(6f));
    }

    /// <summary>
    /// Method to start a match
    /// </summary>
    private void InitializeMatch()
    {
        //If there is a match timer
        if (customRoomProperties.ContainsKey("MatchLength"))
        {
            //If 0, then disable
            if((int)customRoomProperties["MatchLength"] == 0)
            {
                timer.gameObject.SetActive(false);
            }
            //Start timer if not
            else
            {
                //Reset the timer
                matchLength = (int)customRoomProperties["MatchLength"] * 5 * 60;
                currentMatchTime = matchLength;
                RefreshTimerUI();

                //Master user starts the match timer
                if (PhotonNetwork.IsMasterClient)
                {
                    matchTimerCoroutine = StartCoroutine(MatchTimer());
                }
            }
        } 
        //Error catch
        else
        {
            timer.text = "ERROR";
        }

        //Disble socre trackers if not TDM mode
        if (GameSettings.GameMode != GameMode.TDM)
        {
            blueScoreSlider.gameObject.SetActive(false);
            redScoreSlider.gameObject.SetActive(false);
        }

        //Verify score setting exists
        if (customRoomProperties.ContainsKey("ScoreCheck"))
        {
            //Set score threshold
            scoreCheck = (int)customRoomProperties["ScoreCheck"] * 10;

            //If TDM
            if (GameSettings.GameMode == GameMode.TDM)
            {
                //Set socre slider maximums
                blueScoreSlider.maxValue = scoreCheck;
                redScoreSlider.maxValue = scoreCheck;

                //Disable if score = 0
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
    /// <param name="ActorNumber">Actor number for a player</param>
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
    /// <summary>
    /// Method to handle photon Event codes
    /// </summary>
    /// <param name="photonEvent">Event that was raised</param>
    public void OnEvent(EventData photonEvent)
    {
        //If this is a pre-defined photon event
        if (photonEvent.Code >= 200) return;

        //Find the code for the event, and get the attached data
        EventCodes e = (EventCodes)photonEvent.Code;
        object[] o = (object[])photonEvent.CustomData;

        //Raise the appropriate event
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
    /// <summary>
    /// Send method for new player
    /// </summary>
    public void NewPlayer_S()
    {
        //Create new data package
        object[] package = new object[5];

        //Fill the package out
        package[0] = PV.Owner.NickName;                     //Name of player
        package[1] = PV.Owner.ActorNumber;                  //Player's actor number
        package[2] = (short)0;                              //Kills
        package[3] = (short)0;                              //Deaths
        package[4] = CalculateTeam(PV.Owner.ActorNumber);   //Team based on player's actor number

        //Send the package to the master client
        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, new SendOptions { Reliability = true });
    }

    /// <summary>
    /// Recieve method for new player
    /// </summary>
    /// <param name="data">Data package for the new player</param>
    public void NewPlayer_R(object[] data)
    {
        //Create a new playerStats instance with the given data
        PlayerStats p = new PlayerStats((string)data[0], (int)data[1], (short)data[2], (short)data[3], (bool)data[4]);

        //Add it to the master list
        playerStats.Add(p);
        
        //Resync our local player information with the new player
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            gameObject.GetComponent<PlayerControllerModelled>().TrySync();
        }
        
        //Check so only the master client's player manager sends update event
        if(PhotonNetwork.IsMasterClient && PV.IsMine)
        {
            UpdatePlayers_S((int)state, playerStats);
        }
    }
    #endregion

    #region UpdatePlayers
    /// <summary>
    /// Send method to update players
    /// </summary>
    /// <param name="state">Current state of the game according to the master user</param>
    /// <param name="info">PlayerStats list that players need to get</param>
    public void UpdatePlayers_S(int state, List<PlayerStats> info)
    {
        //Create package for sending
        object[] package = new object[info.Count + 1];

        //Frist record game state
        package[0] = state;

        //Add all player's information to the package
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

        //Raise this event for everyone
        PhotonNetwork.RaiseEvent((byte)EventCodes.UpdatePlayers, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    /// <summary>
    /// Receive mehtod to update players
    /// </summary>
    /// <param name="data">New data package for all players</param>
    public void UpdatePlayers_R(object[] data)
    {
        //Record game state
        state = (GameState)data[0];

        //Clear out current player stats list
        playerStats = new List<PlayerStats>();

        for (int i = 1; i < data.Length; i++)
        {
            //Create object for extraction
            object[] extract = (object[])data[i];

            //Create new playerStats from information
            PlayerStats p = new PlayerStats((string)extract[0], (int)extract[1], (short)extract[2], (short)extract[3], (bool)extract[4]);

            //Add playerStats to the main list
            playerStats.Add(p);

            //Check if we are in the list
            if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor)
            {
                //Record the index of where we are in the list
                myIndex = i - 1;

                //if we have been waiting to be added to the game then open the respawn menu
                if (!playerAdded)
                {
                    playerAdded = true;
                    GameSettings.IsBlueTeam = p.blueTeam;
                    openMM(Respawn);
                }
            }
        }

        //Run a check to see if the game is ending
        StateCheck();
    }
    #endregion

    #region ChangeStat
    /// <summary>
    /// Send method to update kill or death count for this player
    /// </summary>
    /// <param name="actor">Actor number of player</param>
    /// <param name="stat">The stat that needs to be changed</param>
    /// <param name="amt">The amount with which the stat changed</param>
    public void ChangeStat_S(int actor, byte stat, byte amt)
    {
        //Create new data package based on parameters
        object[] package = new object[] { actor, stat, amt };

        //Send it to everyone
        PhotonNetwork.RaiseEvent((byte)EventCodes.ChangeStat, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    /// <summary>
    /// Receive method of changeStat pair
    /// </summary>
    /// <param name="data">New stats of the player</param>
    public void ChangeStat_R(object[] data)
    {
        //Get the information from the data package
        int actor = (int)data[0];
        byte stat = (byte)data[1];
        byte amt = (byte)data[2];

        //Iterate through all players
        for (int i = 0; i < playerStats.Count; i++)
        {
            //Find the player whose stats changed
            if (playerStats[i].actor == actor)
            {
                //Get the stat that is changing
                switch (stat)
                {
                    case 0: //kills
                        playerStats[i].kills += amt;
                        //Debug.Log($"Player {playerStats[i].username} : kills = {playerStats[i].kills}");
                        break;

                    case 1: //deaths
                        playerStats[i].deaths += amt;
                        //Debug.Log($"Player {playerStats[i].username} : deaths = {playerStats[i].deaths}");
                        break;
                }

                //Refresh our stats if we are the player who changged
                if (i == myIndex) refreshStats();
                //Exit if not our PlayerManager. Protection against null pointer as other playerManager's leaderboards are destroyed
                if (!PV.IsMine) break;
                //If the leaderboard is active, then call its activation method to update it
                if (leaderBoard.gameObject.activeSelf) Leaderboard(leaderBoard);

                //Stop iterating through players
                break;
            }
        }

        //If we are in TDM mode
        if (GameSettings.GameMode == GameMode.TDM)
        {
            //make sure that team scores are up to date
            UpdateTeamScores();
        }

        //Check if someone won the game
        ScoreCheck();
    }
    #endregion

    #region NewMatch
    /// <summary>
    /// Send method for new matches
    /// </summary>
    public void NewMatch_S()
    {
        //Before new match, destory all vehicles
        foreach(GameObject v in vehicles)
        {
            PhotonNetwork.Destroy(v);
        }
        
        //Run a create vehicles command
        CreateVehicles();

        //Raise new match event
        PhotonNetwork.RaiseEvent((byte)EventCodes.NewMatch, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    /// <summary>
    /// Receive method for new matches
    /// </summary>
    public void NewMatch_R()
    {
        //Set game state to waiting
        state = GameState.Waiting;

        //Hide end game ui
        endGame.gameObject.SetActive(false);

        //Reset scores
        foreach (PlayerStats p in playerStats)
        {
            p.kills = 0;
            p.deaths = 0;
        }
        redScoreCount = 0;
        blueScoreCount = 0;

        //Reset ui
        refreshStats();

        //Reinitialize match
        InitializeMatch();

        //Open respawn menu
        openMM(Respawn);
    }
    #endregion

    #region Timer
    /// <summary>
    /// Send method for the timer updates
    /// </summary>
    public void RefreshTimer_S()
    {
        //Package the current match time
        object[] package = new object[] { currentMatchTime };

        //Raise event with all players in room
        PhotonNetwork.RaiseEvent((byte)EventCodes.RefreshTimer, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    /// <summary>
    /// Receive method for timer updates
    /// </summary>
    /// <param name="data">Data on the current timer</param>
    public void RefreshTimer_R(object[] data)
    {
        //Set the current timer to the passed data
        currentMatchTime = (int)data[0];

        //Update the timer UI
        RefreshTimerUI();
    }
    #endregion
    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine for match timer waits one second before running
    /// </summary>
    /// <returns></returns>
    private IEnumerator MatchTimer()
    {
        //Wait one second
        yield return new WaitForSeconds(1f);

        //Decrease match time
        currentMatchTime -= 1;

        //Check if the match is over
        if (currentMatchTime <= 0)
        {
            //Stop the coroutine and update everyone to end the match
            matchTimerCoroutine = null;
            UpdatePlayers_S((int)GameState.Ending, playerStats);
        }
        //Continue with the timer
        else
        {
            RefreshTimer_S();
            matchTimerCoroutine = StartCoroutine(MatchTimer());
        }
    }

    /// <summary>
    /// Coroutiune for the end of the match
    /// </summary>
    /// <param name="p_wait">Time in seconds of how long to wait on the win screen</param>
    /// <returns></returns>
    private IEnumerator End(float p_wait)
    {
        //Wait for the passed time on the end screen
        yield return new WaitForSeconds(p_wait);

        //If this match should restart
        if (perpetual)
        {
            //Start a new match
            if (PhotonNetwork.IsMasterClient)
            {
                NewMatch_S();
            }
        }
        //Kick players from room
        else
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
        }
    }
    #endregion
}
