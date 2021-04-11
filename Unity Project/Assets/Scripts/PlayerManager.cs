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
public class PlayerManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Vars
    #region Menus
    [SerializeField] MenuManager GameMenus;
    [SerializeField] Menu Respawn, Pause;
    #endregion

    #region Photon
    public PhotonView PV;
    GameObject controller;
    PlayerStats localPlayerStats;
    public List<PlayerStats> playerStats = new List<PlayerStats>();
    public int myIndex;
    Hashtable customProperties;
    public bool blueTeam;
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
    private int arenaKills = 20;
    private int tdmKills = 100;

    #region UI
    [SerializeField] TMP_Text kills, deaths, map, gameType, timer, blueScore, redScore;
    [SerializeField] TMP_Text endKills, endDeaths, endPlayer, endBlueScore, endRedScore, endTeam;
    [SerializeField] Transform leaderBoard, endGame;
    [SerializeField] GameObject statsCard, endPlayerCard, endTeamCard;
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
        customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        blueTeam = CalculateTeam();
    }

    /// <summary>
    /// Method called upon a PlayerManager creations which destroys the ingame menus of other users from the local user's run of the game if PV doesn't match and opens the respawn menu if PV does match
    /// </summary>
    void Start()
    {
        GameSettings.GameMode = (GameMode)(int)customProperties["GameType"];
        GameSettings.IsBlueTeam = blueTeam;
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Canvas>().gameObject);
        }
        else
        {
            localPlayerStats = new PlayerStats(boot.bootObject.currentSettings.nickname, 0, 0, 0, false);
            NewPlayer_S(localPlayerStats);
            refreshStats();
            InitializeTimer();

            if (PhotonNetwork.IsMasterClient)
            {
                playerAdded = true;
                openMM(Respawn);
            }
        }
    }

    /// <summary>
    /// Update method runs with framerate and calls the toggle pause method
    /// </summary>
    private void Update()
    {
        if (state == GameState.Ending)
            return;

        togglePause();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (leaderBoard.gameObject.activeSelf) leaderBoard.gameObject.SetActive(false);
            else Leaderboard(leaderBoard);
        }
    }
    #endregion

    #region Enable/Disable
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion

    #region Enums
    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeStat,
        NewMatch,
        RefreshTimer
    }

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
        if (PV.IsMine)
        {
            //get a random spawnpoint
            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();

            //create a new controller at the spawnpoint prefab loaction
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerModelled"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
            //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "test"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
            //old non-spawnpoint reliant spawn code kept here as a backup
            //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });

            //record an active playerController and close the respawn menu
            activeController = true;
            closeMM(Respawn);
        }
    }

    /// <summary>
    /// Method destroys the player controller, opens respawn menu, and sets active controller to false
    /// </summary>
    public void Die()
    {
        activeController = false;
        PhotonNetwork.Destroy(controller);
        openMM(Respawn);
        ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
    }
    #endregion

    #region Pause
    /// <summary>
    /// Method opens or closes the pause menu when escape key is pressed
    /// </summary>
    private void togglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PV.IsMine)
            {
                if (state != GameState.Playing)
                {
                    //logic statment to evaluate if the respawn menu needs to be opened when the pause menu is closed
                    if (activeController)
                    {
                        closeMM(Pause);
                    }
                    else
                    {
                        state = GameState.Playing;
                        GameMenus.OpenMenu(Respawn);
                    }
                }
                else
                {
                    openMM(Pause);
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
                state = GameState.Playing;
                GameMenus.OpenMenu(Respawn);
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
            blueScore.gameObject.SetActive(false);
            redScore.gameObject.SetActive(false);
        }
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            gameType.text = "Team Deathmatch";

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
            blueScore.text = blueKills.ToString();
            redScore.text = redKills.ToString();
        }

        map.text = SceneManager.GetActiveScene().name;

        // cache prefab
        GameObject playercard = p_lb.GetChild(1).gameObject;
        playercard.SetActive(false);

        // sort
        List<PlayerStats> sorted = SortPlayers(playerStats);

        // display
        bool t_alternateColors = false;
        foreach (PlayerStats a in sorted)
        {
            GameObject newcard = Instantiate(playercard, p_lb) as GameObject;

            /*
            if ((int)customProperties["GameType"] == 1)
            {
                newcard.transform.Find("red").gameObject.SetActive(!a.blueTeam);
                newcard.transform.Find("blue").gameObject.SetActive(a.blueTeam);
            }
            */

            if (t_alternateColors) newcard.GetComponent<Image>().color = new Color32(0, 0, 0, 180);
            t_alternateColors = !t_alternateColors;

            newcard.transform.Find("Username").GetComponent<TMP_Text>().text = a.username;
            newcard.transform.Find("Kills Counter").GetComponent<TMP_Text>().text = a.kills.ToString();
            newcard.transform.Find("Deaths Counter").GetComponent<TMP_Text>().text = a.deaths.ToString();

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

    private void ScoreCheck()
    {
        // define temporary variables
        bool detectwin = false;
        if(GameSettings.GameMode == GameMode.FFA)
        {
            // check to see if any player has met the win conditions
            foreach (PlayerStats a in playerStats)
            {
                // free for all
                if (a.kills >= arenaKills)
                {
                    detectwin = true;
                    break;
                }
            }
        }
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            int blueKills = 0;
            int redKills = 0;

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
            if ((blueKills >= tdmKills) || (redKills >= tdmKills))
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
            //PhotonNetwork.DestroyAll();
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
        GameMenus.GetComponent<Image>().enabled = true;

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
        else if (GameSettings.GameMode == GameMode.TDM)
        {
            endTeamCard.SetActive(true);
            int blueKills = 0;
            int redKills = 0;

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
            endBlueScore.text = blueKills.ToString();
            endRedScore.text = redKills.ToString();

            if (blueKills > redKills)
            {
                endTeam.text = "Blue Team Wins";
                endTeam.color = new Color(0, 0, 255);
            }
            else if (blueKills < redKills)
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

    private void InitializeTimer()
    {
        currentMatchTime = matchLength;
        RefreshTimerUI();

        if (PhotonNetwork.IsMasterClient)
        {
            matchTimerCoroutine = StartCoroutine(MatchTimer());
        }
    }

    private Boolean CalculateTeam()
    {
        return PhotonNetwork.LocalPlayer.ActorNumber % 2 == 0;
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
    public void NewPlayer_S(PlayerStats p)
    {
        object[] package = new object[5];

        package[0] = p.username;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = (short)0;
        package[3] = (short)0;
        package[4] = CalculateTeam();

        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, new SendOptions { Reliability = true });
    }

    public void NewPlayer_R(object[] data)
    {
        PlayerStats p = new PlayerStats((string)data[0], (int)data[1], (short)data[2], (short)data[3], (bool)data[4]);

        playerStats.Add(p);

        /*
        //resync our local player information with the new player
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            gameObject.GetComponent<Player>().TrySync();
        }
        */

        UpdatePlayers_S((int)state, playerStats);
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
                if (leaderBoard.gameObject.activeSelf) Leaderboard(leaderBoard);

                break;
            }
        }

        ScoreCheck();
    }
    #endregion

    #region NewMatch
    public void NewMatch_S()
    {
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

        // reset ui
        refreshStats();

        // reinitialize time
        InitializeTimer();

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
