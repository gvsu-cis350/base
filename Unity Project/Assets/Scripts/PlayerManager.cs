using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    
    GameObject controller;

//    private static PlayerManager Instance;

    [SerializeField] MenuManager GameMenus;
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.OpenMenu(Respawn);
        //   GameMenu.OpenMenu(Respawn);
        //        if (PV.IsMine)
        //       {
        //            CreateController();
        //        }
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
//        CreateController();
    }

    private void togglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseState)
            {
                pauseState = false;
                GameMenus.CloseMenu(Pause);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                pauseState = true;
                GameMenus.OpenMenu(Pause);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void exitPause()
    {
        pauseState = false;
        GameMenus.CloseMenu(Pause);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
