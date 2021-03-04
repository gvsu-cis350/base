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

    //roomlist interactions through unity
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListButtonPrefab;

    //playerlist interactions through unity
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;

    //serialization of the starting game button
    [SerializeField] GameObject startGameButton;

    //method call when the launcher is first accessed
    private void Awake()
    {
        //set a global instance of launcher to this object
        Instance = this;
    }

    //method that triggers once on the creation of this class
    void Start()
    {
        //throw a console message
        Debug.Log("Connecting to Master");
        //connect to a pre-deterimined photon master server
        PhotonNetwork.ConnectUsingSettings();
    }

    //method that triggers when user connects to photon master server
    public override void OnConnectedToMaster()
    {
        //throw a console message
        Debug.Log("Connected to Master");
        //join the lobby of the server that the user just connected to
        PhotonNetwork.JoinLobby();
        //automatically sync the scene to the Photon server's scene
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //method that triggers after user connects to lobby
    public override void OnJoinedLobby()
    {
        //open the main menu through the menuManager
        MenuManager.Instance.OpenMenu("MainMenu");
        //throw a console message
        Debug.Log("Joined Lobby");
        //set Nickname to an automatically generated name
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }
    
    //method that when called creates a new room within the photon lobby
    public void CreateRoom()
    {
        //looks if room name field is empty
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            //do nothing if it is
            return;
        }
        //create a new room named with the text in the textfield
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        //use the menumanager to open the connecting menu
        MenuManager.Instance.OpenMenu("Connecting");
    }

    //method which automatically calls when user joins a new room
    public override void OnJoinedRoom()
    {
        //Open automatically to the waiting menu
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
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    //method switches host client if old host disconnects
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    //method to throw an error menu if a room create fails
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Error: Failed to Join Room: " + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    //method allows user to leave a room and reconnects them to lobby and main menu
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Connecting");
    }

    //method which joins a room based off of information pulled from parameter
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Connecting");
    }

    //method to return to main menu on leaving a room    
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("MainMenu");
    }

    //Method that calls whenever a room is updated to update local runs of users
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

    //Method to update the player list whenever someone enters a room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    //Method which starts the game for everyone
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
