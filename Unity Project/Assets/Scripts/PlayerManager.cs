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


/// <summary>
/// Class for managing players throughout their time in a game, instance of playerManagers are only destroyed upon leaving a room
/// </summary>
public class PlayerManager : MonoBehaviour, IOnEventCallback
{
    #region Vars
    #region SerializeFields
    [SerializeField] MenuManager GameMenus;
    [SerializeField] Menu Respawn, Pause;
    #endregion

    #region Photon
    public PhotonView PV;
    GameObject controller;
    PlayerStats localPlayerStats;
    public List<PlayerStats> playerStats = new List<PlayerStats>();
    public int myIndex;
    #endregion

    #region Menu
    bool activeController = false;
    #endregion

    [SerializeField] TMP_Text kills, deaths, arenaMap, tdmMap;
    [SerializeField] Transform leaderBoard, endGame;
    [SerializeField] GameObject statsCard;

    public GameState state = GameState.Waiting;
    public bool perpetual = false;
    #endregion

    /// <summary>
    /// Method called when this class is referenced, assigns the Photon View to PM's PV reference var
    /// </summary>
    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            boot.bootObject.localPV = PV;
        }

        PhotonNetwork.AddCallbackTarget(this);
    }

    /// <summary>
    /// Method called upon a PlayerManager creations which destroys the ingame menus of other users from the local user's run of the game if PV doesn't match and opens the respawn menu if PV does match
    /// </summary>
    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Canvas>().gameObject);
        }
        else
        {
            //PhotonNetwork.OnEventCall += this.OnEvent;
            localPlayerStats = new PlayerStats(boot.bootObject.currentSettings.nickname, 0, 0, 0);
            NewPlayer_S(localPlayerStats);
            openMM(Respawn);
            refreshStats();
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

    #region Enums
    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeStat
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
        state = GameState.Waiting;
        GameMenus.OpenMenu(menuName);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.GetComponent<Image>().enabled = true;
    }
    #endregion

    /// <summary>
    /// Public method to force a player to return to the lobby
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

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

    private void Leaderboard(Transform p_lb)
    {
        /*
        // specify leaderboard
        if (GameSettings.GameMode == GameMode.FFA) p_lb = p_lb.Find("FFA");
        if (GameSettings.GameMode == GameMode.TDM) p_lb = p_lb.Find("TDM");
        */

        arenaMap.text = SceneManager.GetActiveScene().name;
        // clean up
        for (int i = 2; i < p_lb.childCount; i++)
        {
            Destroy(p_lb.GetChild(i).gameObject);
        }

        /*
        // set details
        p_lb.Find("Header/Mode").GetComponent<Text>().text = System.Enum.GetName(typeof(GameMode), GameSettings.GameMode);
        p_lb.Find("Header/Map").GetComponent<Text>().text = SceneManager.GetActiveScene().name;
        */

        /*
        // set scores
        if (GameSettings.GameMode == GameMode.TDM)
        {
            p_lb.Find("Header/Score/Home").GetComponent<Text>().text = "0";
            p_lb.Find("Header/Score/Away").GetComponent<Text>().text = "0";
        }
        */

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
            if (GameSettings.GameMode == GameMode.TDM)
            {
                newcard.transform.Find("Home").gameObject.SetActive(!a.awayTeam);
                newcard.transform.Find("Away").gameObject.SetActive(a.awayTeam);
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

        //if (GameSettings.GameMode == GameMode.FFA)
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
        /*
        if (GameSettings.GameMode == GameMode.TDM)
        {
            List<PlayerInfo> homeSorted = new List<PlayerInfo>();
            List<PlayerInfo> awaySorted = new List<PlayerInfo>();

            int homeSize = 0;
            int awaySize = 0;

            foreach (PlayerInfo p in p_info)
            {
                if (p.awayTeam) awaySize++;
                else homeSize++;
            }

            while (homeSorted.Count < homeSize)
            {
                // set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                // grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (a.awayTeam) continue;
                    if (homeSorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                homeSorted.Add(selection);
            }

            while (awaySorted.Count < awaySize)
            {
                // set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                // grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (!a.awayTeam) continue;
                    if (awaySorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                awaySorted.Add(selection);
            }

            sorted.AddRange(homeSorted);
            sorted.AddRange(awaySorted);
        }
        */
        return sorted;
    }

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

        // check to see if any player has met the win conditions
        foreach (PlayerStats a in playerStats)
        {
            // free for all
            //         if (a.kills >= killcount)
            if (a.deaths >= 2)
            {
                detectwin = true;
                break;
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
    private void EndGame()
    {
        // set game state to ending
        state = GameState.Ending;

        leaderBoard.gameObject.SetActive(false);

        // set timer to 0
  //      if (timerCoroutine != null) StopCoroutine(timerCoroutine);
//        currentMatchTime = 0;
 //       RefreshTimerUI();

        // disable room
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.DestroyAll();

            if (!perpetual)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        // activate map camera
        //        mapcam.SetActive(true);

        //Run commands to turn off all menus
        GameMenus.CloseAllMenus();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.GetComponent<Image>().enabled = true;

        // show end game ui
        endGame.gameObject.SetActive(true);
        Leaderboard(endGame);

        // wait X seconds and then return to main menu
        StartCoroutine(End(6f));
    }

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
        }
    }

    #region NewPlayer
    public void NewPlayer_S(PlayerStats p)
    {
        object[] package = new object[4];

        package[0] = p.username;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = (short)0;
        package[3] = (short)0;
        //       package[5] = CalculateTeam();

        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, new SendOptions { Reliability = true });
    }

    public void NewPlayer_R(object[] data)
    {
        PlayerStats p = new PlayerStats((string)data[0], (int)data[1], (short)data[2], (short)data[3]);

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
            //          piece[5] = info[i].awayTeam;

            package[i + 1] = piece;
        }

        PhotonNetwork.RaiseEvent((byte)EventCodes.UpdatePlayers, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    public void UpdatePlayers_R(object[] data)
    {
        state = (GameState)data[0];

        /*
        //check if there is a new player
        if (playerStats.Count < data.Length - 1)
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                //if so, resync our local player information
                gameObject.GetComponent<Player>().TrySync();
            }
        }
        */

        playerStats = new List<PlayerStats>();

        for (int i = 1; i < data.Length; i++)
        {
            object[] extract = (object[])data[i];

            PlayerStats p = new PlayerStats((string)extract[0], (int)extract[1], (short)extract[2], (short)extract[3]);

            playerStats.Add(p);

            if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor)
            {
                myIndex = i - 1;
                /*
                //if we have been waiting to be added to the game then spawn us in
                if (!playerAdded)
                {
                    playerAdded = true;
                    GameSettings.IsAwayTeam = p.awayTeam;
                    Spawn();
                }
                */
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
                        Debug.Log($"Player {playerStats[i].username} : kills = {playerStats[i].kills}");
                        break;

                    case 1: //deaths
                        playerStats[i].deaths += amt;
                        Debug.Log($"Player {playerStats[i].username} : deaths = {playerStats[i].deaths}");
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
    #endregion
    #region Coroutines
    /*
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1;

        if (currentMatchTime <= 0)
        {
            timerCoroutine = null;
            UpdatePlayers_S((int)GameState.Ending, playerInfo);
        }
        else
        {
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
        }
    }
    */

    private IEnumerator End(float p_wait)
    {
        yield return new WaitForSeconds(p_wait);

        if (perpetual)
        {
            // new match
            if (PhotonNetwork.IsMasterClient)
            {
 //               NewMatch_S();
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
