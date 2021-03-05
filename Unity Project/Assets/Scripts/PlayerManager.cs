using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    PhotonView PV;
    
    GameObject controller;

    [SerializeField] MenuManager GameMenus;
    [SerializeField] Menu Respawn, Pause;

    public bool pauseState = false;
    bool activeController = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
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

    private void Update()
    {
        togglePause();
    }

    public void CreateNewController()
    {
        if (PV.IsMine)
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            activeController = true;
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
            closeMM(Respawn);
        }
    }

    public void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });

        //old non-spawnpoint reliant spawn code kept here as a backup
        //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        activeController = false;
        PhotonNetwork.Destroy(controller);
        openMM(Respawn);
    }

    private void togglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PV.IsMine)
            {
                if (pauseState)
                {
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

    public void exitPause()
    {
        if (PV.IsMine)
        {
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

    private void closeMM(Menu menuName)
    {
        pauseState = false;
        GameMenus.CloseMenu(menuName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameMenus.GetComponent<Image>().enabled = false;
    }

    private void openMM(Menu menuName)
    {
        pauseState = true;
        GameMenus.OpenMenu(menuName);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMenus.GetComponent<Image>().enabled = true;
    }
}
