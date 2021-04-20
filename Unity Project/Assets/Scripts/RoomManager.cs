using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Realtime;

/// <summary>
/// Class to manage room creations and create playermanagers
/// </summary>
public class RoomManager : MonoBehaviourPunCallbacks
{
    //var for global roomManager reference
    public static RoomManager Instance;

    /// <summary>
    /// Method that triggers when RoomManager is referenced, and updates the global instance to the most current reference
    /// </summary>
    private void Awake()
    {
        //checks if another RoomManager exists and destroys it if there is one
        if (Instance)// != null)
        {
            Destroy(gameObject);
            return;
        }
        //If this is the only RM, then the code makes itself the instance and manager for the game
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    /// <summary>
    /// Method is setup such that if a scene is ever enabled this method will trigger an OnSceneLoaded event
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Method is setup such that if a scene is disabled this method will trigger disable an OnSceneLoaded event
    /// </summary>
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    /// <summary>
    /// This method triggers with an OnSceneLoaded and creates a playermanager for the user if they are on a map scene
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //If user is between map scene range
        if ((scene.buildIndex > 1) && (scene.buildIndex < SceneManager.sceneCountInBuildSettings))
        {
            //Create a playermanager
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }

    /// <summary>
    /// Method which cases a local player to force a player onto the menu scene
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }
}
