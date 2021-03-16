using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class boot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        newload();
        PlayerInfo load = DataSaver.loadData<PlayerInfo>("config");
        Screen.SetResolution(load.resolutionWidth, load.resolutionHeight, load.fullscreen);
        SceneManager.LoadScene(1);
    }

    private void newload()
    {
        string tempPath = Path.Combine(Path.Combine(Application.persistentDataPath, "data"), "config.txt");

        //Create config file if a config file does not exist
        if (!File.Exists(tempPath))
        {
            Resolution[] resolutions = Screen.resolutions;
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            PlayerInfo newLoad = new PlayerInfo();
            newLoad.masterVolume = 70;
            newLoad.fov = 80;
            newLoad.savedResolution = resolutions[0];
            newLoad.resolutionWidth = resolutions[0].width;
            newLoad.resolutionHeight = resolutions[0].height;
            newLoad.fullscreen = false;
            newLoad.nickname = "Player " + UnityEngine.Random.Range(0, 1000).ToString("0000");
            DataSaver.saveData(newLoad, "config");
        }
    }
}
