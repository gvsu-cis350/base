using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class boot : MonoBehaviour
{
    public bool firstConnection = false;

    //public static boot Instance;

    private void Awake()
    {
       // Instance = this;
        DontDestroyOnLoad(transform.gameObject);
        //Debug.Log("Boot Scene Worked");
        //     Resolution[] resolutions = Screen.resolutions;
        //      foreach (Resolution i in resolutions)
        //      {
        //          Debug.Log(i);
        //       }
        newload();
     //   Debug.Log("passed newload");
        PlayerInfo load = DataSaver.loadData<PlayerInfo>("config");
        Screen.SetResolution(load.resolutionWidth, load.resolutionHeight, load.fullscreen);
        SceneManager.LoadScene(1);
    }

    private void newload()
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, "config.txt");

    //    Debug.Log(!Directory.Exists(Path.GetDirectoryName(tempPath)));
    //    Debug.Log(!File.Exists(tempPath));
        //Create Directory if it does not exist
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
            newLoad.fullscreen = true;
            DataSaver.saveData(newLoad, "config");
  //          Debug.Log("Should have worked");
        }
    }
}
