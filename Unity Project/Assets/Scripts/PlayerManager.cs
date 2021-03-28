using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


/// <summary>
/// Class for managing players throughout their time in a game, instance of playerManagers are only destroyed upon leaving a room
/// </summary>
public class PlayerManager : MonoBehaviour
{
    //reference vars
    PhotonView PV;
    GameObject controller;
    
    //unity reference vars
    [SerializeField] MenuManager GameMenus;
    [SerializeField] Menu Respawn, Pause;

    //menu activation bools
    public bool pauseState = false;
    bool activeController = false;

    /// <summary>
    /// Method called when this class is referenced, assigns the Photon View to PM's PV reference var
    /// </summary>
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
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
            if (!PhotonNetwork.IsMasterClient)
            {
    //            controller.gameObject.GetComponent<MeshRenderer>().material = Host;
            }
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
}
