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


/// <summary>
/// Class for managing players throughout their time in a game, instance of playerManagers are only destroyed upon leaving a room
/// </summary>
public class PlayerManager : MonoBehaviour, IOnEventCallback
{
    //reference vars
    public PhotonView PV;
    GameObject controller;

    //unity reference vars
    [SerializeField] MenuManager GameMenus;
    [SerializeField] Menu Respawn, Pause;

    //menu activation bools
    public bool pauseState = false;
    bool activeController = false;

    PlayerStats localPlayerStats;

    public List<PlayerStats> playerStats = new List<PlayerStats>();
    public int myIndex;

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
        }
    }

    /// <summary>
    /// Update method runs with framerate and calls the toggle pause method
    /// </summary>
    private void Update()
    {
        togglePause();
    }

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

    /// <summary>
    /// Method opens or closes the pause menu when escape key is pressed
    /// </summary>
    private void togglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PV.IsMine)
            {
                if (pauseState)
                {
                    //logic statment to evaluate if the respawn menu needs to be opened when the pause menu is closed
                    if (activeController)
                    {
                        closeMM(Pause);
                    }
                    else
                    {
                        pauseState = false;
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
                pauseState = false;
                GameMenus.OpenMenu(Respawn);
            }
        }
    }

    /// <summary>
    /// Private method which closes the passed menu, locks the cursor to the screen, makes the cursor and menu background invisable, and sets pause bool to false
    /// </summary>
    /// <param name="menuName"></param>
    private void closeMM(Menu menuName)
    {
        pauseState = false;
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
        pauseState = true;
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

    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeStat
    }

    public void OnEvent(EventData photonEvent)
    {
//        Debug.LogError("We are receiving events");
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

        UpdatePlayers_S(playerStats);    //(int)state, playerStats);
    }

    public void UpdatePlayers_S(List<PlayerStats> info)    //int state, List<PlayerStats> info)
    {
        object[] package = new object[info.Count + 1];

        //package[0] = state;
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
 //       state = (GameState)data[0];

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

        //StateCheck();
    }

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

//                if (i == myind) RefreshMyStats();
//                if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);

                break;
            }
        }

//        ScoreCheck();
    }
}
