using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //
    // IMPORTANT TODO: FIX BUG WHERE YOU CAN SWITCH FROM SPAWN MENU TO PAUSE MENU BUT NOT BACK
    //

    PhotonView PV;
    
    GameObject controller;

//    private static PlayerManager Instance;

    [SerializeField] MenuManager GameMenus;
    [SerializeField] GameObject GameMenu;
    [SerializeField] Menu Respawn, Pause;

    //    [SerializeField] MenuManager GameMenu;
    //    [SerializeField] Menu Respawn;

    public bool pauseState = false;

    private void Awake()
    {
//        Instance = this;
//        Instantiate(GameMenus, Instance);
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            //<GameMenu>().gameObject);
        }
        else
        {
            GameMenus.GetComponent<Image>().enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameMenus.OpenMenu(Respawn);
            //   GameMenu.OpenMenu(Respawn);
            //        if (PV.IsMine)
            //       {
            //            CreateController();
            //        }
        }
    }

    private void Update()
    {
        togglePause();
    }

    public void CreateNewController()
    {
        if (PV.IsMine)
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
            GameMenus.CloseMenu(Respawn);
            Cursor.lockState = CursorLockMode.Locked;
            GameMenus.GetComponent<Image>().enabled = false;
        }
    }

    public void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });

        //old non-spawnpoint reliant spawn code
        //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        GameMenus.OpenMenu(Respawn);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.GetComponent<Image>().enabled = true;
        //        CreateController();
    }

    private void togglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PV.IsMine)
            {
                if (pauseState)
                {
                    pauseState = false;
                    GameMenus.CloseMenu(Pause);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameMenus.GetComponent<Image>().enabled = false;
                }
                else
                {
                    pauseState = true;
                    GameMenus.OpenMenu(Pause);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    GameMenus.GetComponent<Image>().enabled = true;
                }

            }
        }
    }

    public void exitPause()
    {
        if (PV.IsMine)
        {
            pauseState = false;
            GameMenus.CloseMenu(Pause);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameMenus.GetComponent<Image>().enabled = false;
        }
    }

    private void closeMM(string menuName)
    {
        pauseState = false;
        GameMenus.CloseMenu(Pause);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
