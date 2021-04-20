using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using Photon.Pun;

/// <summary>
/// Boot object class stores universal references and variables that will be need in the furture and also sets up the inital settings
/// </summary>
public class boot : MonoBehaviour
{
    #region Vars
    public PlayerInfo currentSettings;
    public PhotonView localPV;
    public static boot bootObject;
    #endregion

    /// <summary>
    /// Method triggers on the instantiation of the boot object
    /// </summary>
    private void Awake()
    {
        //Set this object to the global static reference, establish that this object will not be deleted, and check for player config file
        bootObject = this;
        DontDestroyOnLoad(transform.gameObject);
        newload();
        currentSettings = DataSaver.loadData<PlayerInfo>("config");
        
        //Set resolution and leave scene
        Screen.SetResolution(currentSettings.resolutionWidth, currentSettings.resolutionHeight, currentSettings.fullscreen);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Start method triggers on the first frame and subscribes to the new settings event.
    /// </summary>
    private void Start()
    {
        GameEvents.current.onNewSettings += SettingsUpdated;
    }

    /// <summary>
    /// Method reloads the saved settings vars when there are new settings and then throws an event so that all affected objects update their values to the new values stored here
    /// </summary>
    private void SettingsUpdated()
    {
        //Delete current settings and load the new ones
        currentSettings = null;
        currentSettings = DataSaver.loadData<PlayerInfo>("config");
        
        //Establish new nickname and throw the settings updated event
        PhotonNetwork.NickName = currentSettings.nickname;
        GameEvents.current.onSettingsUpdateEvent();
    }

    /// <summary>
    /// Method is called to determine if there is a current player config file and create one with generic values if there isn't one
    /// </summary>
    private void newload()
    {
        //Create a string which stores the file directory path where the player config file is stored
        string tempPath = Path.Combine(Path.Combine(Application.persistentDataPath, "data"), "config.txt");

        //Create config file if a config file does not exist
        if (!File.Exists(tempPath))
        {
            //Generic settings for the game which each config file starts with
            Resolution[] resolutions = Screen.resolutions;
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            currentSettings.masterVolume = 70;
            currentSettings.musicVolume = 70;
            currentSettings.sfxVolume = 70;
            currentSettings.fov = 80;
            currentSettings.savedResolution = resolutions[0];
            currentSettings.resolutionWidth = resolutions[0].width;
            currentSettings.resolutionHeight = resolutions[0].height;
            currentSettings.fullscreen = false;
            currentSettings.nickname = "Player " + UnityEngine.Random.Range(0, 1000).ToString("0000");
            currentSettings.mouseSensitvity = 10;
            DataSaver.saveData(currentSettings, "config");
        }
    }
}
