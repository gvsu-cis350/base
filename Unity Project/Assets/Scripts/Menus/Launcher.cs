using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//class to hand the launch of the game and the main menu screen
public class Launcher : MonoBehaviourPunCallbacks
{
    #region Vars
    //static laucncher instance so the launcher can be called throughout the system
    public static Launcher Instance;

    #region Room Vars
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    #endregion

    #region RoomList Vars
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListButtonPrefab;
    #endregion

    #region PlayerList Vars
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    #endregion

    #region Match Settings Vars
    [SerializeField] GameObject startGameButton, roomSettingsButton;
    [SerializeField] TMP_Dropdown mapSelectionDropdown, gameTypeDropdown, matchLengthDropdown, scoreCheckDropdown;
    [SerializeField] Toggle[] weapons;
    [SerializeField] Toggle allWeapons;

    Hashtable roomSettings = new Hashtable();
    #endregion
    #endregion

    /// <summary>
    /// Method call when the launcher is first accessed to set a global instacne of launcher
    /// </summary>
    private void Awake()
    {
 //       PlayerInfo playerInfo = DataSaver.loadData<PlayerInfo>("")
        Instance = this;
    }

    #region Connection Pipeline
    //Methods Below establish a pipeline to see user properly connect to the server
    /// <summary>
    /// Method that triggers once on the creation and connects user to pre-determined photon master server.
    /// </summary>
    void Start()
    {
   //     Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Method that triggers when user connects to photon master server, and then joins the lobby of the server and syncs scenes with the server
    /// </summary>
    public override void OnConnectedToMaster()
    {
       // Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// Method that triggers after user connects to lobby to open the main menu and assign a username based on the player config file
    /// </summary>
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("MainMenu");
       // Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = boot.bootObject.currentSettings.nickname;
    }
    #endregion

    #region Room Creation Methods
    /// <summary>
    /// Method that creates a new room within the photon lobby pased off of a textfield input
    /// </summary>
    public void CreateRoom()
    {
        //exit method if the textfield is empty
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 16;
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
        MenuManager.Instance.OpenMenu("Connecting");
    }

    /// <summary>
    /// Method switches host client if old host disconnects
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Error: Failed to Join Room: " + message;
        MenuManager.Instance.OpenMenu("Error");
    }
    #endregion

    #region Leaving and Joining Rooms
    /// <summary>
    /// Method allows user to leave a room and reconnects them to lobby and main menu
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Connecting");
    }

    /// <summary>
    /// Method which joins a room based off of information passed through the parameter
    /// </summary>
    /// <param name="info"></param>
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Connecting");
    }

    /// <summary>
    /// Method to return to main menu when leaving a room    
    /// </summary>
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("MainMenu");
    }

    /// <summary>
    /// Method called when user joins a room. Method changes menus and complies a new playerlist when possible.
    /// </summary>
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Waiting");

        //set the room name text field to the photon network's room name
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //create an array of players from Photon network
        Player[] players = PhotonNetwork.PlayerList;

        //clear out all previously stored players
        foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }

        //add all players to a playerlist pulled from the Photon Network's list
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        //set the start button to be active for host client
        roomSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    #endregion

    #region Photon Room Update Methods
    /// <summary>
    /// Method that calls whenever a room is updated to update local runs of users
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //destroy all stored rooms on update
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        //fill in room list from passed list
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            //Create buttons from a prefabs and based the photon room list
            Instantiate(roomListButtonPrefab, roomListContent).GetComponent<RoomListButton>().SetUp(roomList[i]);
        }
    }

    /// <summary>
    /// Method to update the player list whenever someone enters a room
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    /// <summary>
    /// Method switches host client if old host disconnects
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        roomSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    #endregion

    /// <summary>
    /// Method which starts the game for everyone in room when called
    /// </summary>
    public void StartGame()
    {
        //Get the current settings
        roomSettings = PhotonNetwork.CurrentRoom.CustomProperties;

        //Game Type remove-record
        if (roomSettings.ContainsKey("GameType"))
            roomSettings.Remove("GameType");
        roomSettings.Add("GameType", gameTypeDropdown.value);

        //Game Length remove-record
        if (roomSettings.ContainsKey("MatchLength"))
            roomSettings.Remove("MatchLength");
        roomSettings.Add("MatchLength", matchLengthDropdown.value);

        //Game Length remove-record
        if (roomSettings.ContainsKey("ScoreCheck"))
            roomSettings.Remove("ScoreCheck");
        roomSettings.Add("ScoreCheck", scoreCheckDropdown.value);

        //Allowed weapons remove-record
        int i = 0;
        foreach(Toggle t in weapons)
        {
            if (roomSettings.ContainsKey(t.name))
                roomSettings.Remove(t.name);

            if (t.isOn)
            {
                roomSettings.Add(t.name, true);
            }
            i++;
        }

        //All Weapons Equipable remove-record
        if (roomSettings.ContainsKey("AllWeapons"))
            roomSettings.Remove("AllWeapons");
        roomSettings.Add("AllWeapons", !allWeapons.isOn);

        //Set properties and load scene
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomSettings);
        PhotonNetwork.LoadLevel(mapSelectionDropdown.value + 2);
    }
}
