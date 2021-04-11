using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerSettings : MonoBehaviour
{
    #region Vars
    #region Inspector Reference Vars
    [SerializeField] Slider volumeSlider, fovSlider, musicVolumeSlider, sfxVolumeSlider, mouseSensitivitySlider;
    [SerializeField] TMP_Dropdown resolutionSelection;
    [SerializeField] Toggle fullScreen;
    [SerializeField] TMP_Text volumeText, fovText, musicVolumeText, sfxVolumeText, mouseSensitivityText;
    [SerializeField] TMP_InputField nickName;
    #endregion

    #region Resolution Vars
    public Resolution[] resolutions;
    Dictionary<int, Resolution> resolutionDict = new Dictionary<int, Resolution>();
    private List<string> WidthByHeight = new List<string>();
    int curRes;
    #endregion
    #endregion

    /// <summary>
    /// Method triggers on instantiation and records the resolutions available at the highest referesh rate and also loads the player settings.
    /// </summary>
    public void Awake()
    {
        //Collect the available resolutions and create a int to store the highest refresh rate
        resolutions = Screen.resolutions;
        int highestRefresh = 0;

        //Find the highest refresh rate
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate > highestRefresh)
                highestRefresh = r.refreshRate;
        }

        //Collect the resolutions with the highest refresh rate into a new dictionary
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate == highestRefresh)
            {
                WidthByHeight.Add(r.width + "x" + r.height);
                resolutionDict.Add(resolutionDict.Count, r);
            }
        }

        //Call the load settings menu to initalize the settings menu
        loadSettings();
    }

    /// <summary>
    /// Method loads the player config file and displays the stored settings onto the menu
    /// </summary>
    public void loadSettings()
    {
        //Read and set the values for the:
        //Master Volume
        volumeText.text = "Volume: " + boot.bootObject.currentSettings.masterVolume;
        volumeSlider.value = boot.bootObject.currentSettings.masterVolume;

        //Music Volume
        musicVolumeText.text = "Music Volume: " + boot.bootObject.currentSettings.musicVolume;
        musicVolumeSlider.value = boot.bootObject.currentSettings.musicVolume;

        //SFX Volume
        sfxVolumeText.text = "SFX Volume: " + boot.bootObject.currentSettings.sfxVolume;
        sfxVolumeSlider.value = boot.bootObject.currentSettings.sfxVolume;

        //FOV
        fovText.text = "FOV: " + boot.bootObject.currentSettings.fov;
        fovSlider.value = boot.bootObject.currentSettings.fov;

        //Mouse Sensitivity
        mouseSensitivityText.text = "Mouse Sensitivity: " + boot.bootObject.currentSettings.mouseSensitvity;
        mouseSensitivitySlider.value = boot.bootObject.currentSettings.mouseSensitvity;

        //Resolutions
        resolutionSelection.ClearOptions();
        resolutionSelection.AddOptions(WidthByHeight);
        foreach(var item in resolutionDict)
        {
            if ((item.Value.width == boot.bootObject.currentSettings.resolutionWidth) && (item.Value.height == boot.bootObject.currentSettings.resolutionHeight))
            {
                curRes = item.Key;
            }
        }
        resolutionSelection.value = curRes;

        //Fullscreen
        fullScreen.SetIsOnWithoutNotify(boot.bootObject.currentSettings.fullscreen);

        //Nickname
        nickName.text = boot.bootObject.currentSettings.nickname;
    }

    /// <summary>
    /// Method Applies the resolution settings and triggers the settings update event
    /// </summary>
    public void applySettings()
    {
        Screen.SetResolution(resolutionDict[resolutionSelection.value].width, resolutionDict[resolutionSelection.value].height, fullScreen.isOn);
        GameEvents.current.onNewSettingsEvent();
    }

    /// <summary>
    /// Method reads the currently set values and saves them to the player config file
    /// </summary>
    public void saveSettings()
    {
        PlayerInfo newSave = new PlayerInfo();
        newSave.masterVolume = (int)volumeSlider.value;
        newSave.musicVolume = (int)musicVolumeSlider.value;
        newSave.sfxVolume = (int)sfxVolumeSlider.value;
        newSave.fov = (int)fovSlider.value;
        newSave.savedResolution = resolutionDict[resolutionSelection.value];
        newSave.resolutionHeight = resolutionDict[resolutionSelection.value].height;
        newSave.resolutionWidth = resolutionDict[resolutionSelection.value].width;
        newSave.mouseSensitvity = (int)mouseSensitivitySlider.value;
        newSave.fullscreen = fullScreen.isOn;
        newSave.nickname = nickName.text;
        DataSaver.saveData(newSave, "config");
    }

    /// <summary>
    /// Method updates the text associated with certain values when those values are updated in the settings menu
    /// </summary>
    public void updateText()
    {
        volumeText.text = "Master Volume: " + volumeSlider.value;
        fovText.text = "FOV: " + fovSlider.value;
        musicVolumeText.text = "Music Volume: " + musicVolumeSlider.value;
        sfxVolumeText.text = "SFX Volume: " + sfxVolumeSlider.value;
        mouseSensitivityText.text = "Mouse Sensitivity: " + mouseSensitivitySlider.value;
    }
}
