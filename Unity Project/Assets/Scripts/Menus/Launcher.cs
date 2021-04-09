using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

//class to hand the launch of the game and the main menu screen
public class Launcher : MonoBehaviourPunCallbacks
{
    //static laucncher instance so the launcher can be called throughout the system
    public static Launcher Instance;

    //Textfield interactions through unity
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Dropdown mapSelectionDropdown;

    //roomlist interactions through unity
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListButtonPrefab;

    //playerlist interactions through unity
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;

    //serialization of the host buttons
    [SerializeField] GameObject startGameButton, roomSettingsButton;

    /// <summary>
    /// Method call when the launcher is first accessed to set a global instacne of launcher
    /// </summary>
    private void Awake()
    {
 //       PlayerInfo playerInfo = DataSaver.loadData<PlayerInfo>("")
        Instance = this;
    }

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

    /// <summary>
    /// Method that creates a new room within the photon lobby pased off of a textfield input
    /// </summary>
    public void CreateRoom()
    {
        //exit method if the textfield is empty
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return;
        RoomOptions options = new RoomOptions();
        PhotonNetwork.CreateRoom(roomNameInputField.text, options);
        MenuManager.Instance.OpenMenu("Connecting");
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

    /// <summary>
    /// Method switches host client if old host disconnects
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        roomSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
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
    /// Method which starts the game for everyone in room when called
    /// </summary>
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(mapSelectionDropdown.value + 2);
    }
}
