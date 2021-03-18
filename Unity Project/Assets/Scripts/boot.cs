using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using Photon.Pun;

public class boot : MonoBehaviour
{
    public PlayerInfo currentSettings;

    public static boot bootObject;

    private void Awake()
    {
        bootObject = this;
        DontDestroyOnLoad(transform.gameObject);
        newload();

        currentSettings = DataSaver.loadData<PlayerInfo>("config");
        Screen.SetResolution(currentSettings.resolutionWidth, currentSettings.resolutionHeight, currentSettings.fullscreen);
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        GameEvents.current.onNewSettings += SettingsUpdated;
    }

    private void SettingsUpdated()
    {
        currentSettings = null;
        currentSettings = DataSaver.loadData<PlayerInfo>("config");
        PhotonNetwork.NickName = currentSettings.nickname;
        GameEvents.current.onSettingsUpdateEvent();
    }

    private void newload()
    {
        string tempPath = Path.Combine(Path.Combine(Application.persistentDataPath, "data"), "config.txt");

        //Create config file if a config file does not exist
        if (!File.Exists(tempPath))
        {
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
