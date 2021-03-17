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
    [SerializeField] Slider volumeSlider, fovSlider;
    [SerializeField] TMPro.TMP_Dropdown resolutionSelection;
    [SerializeField] Toggle fullScreen;
    [SerializeField] TMP_Text volumeText, fovText;
    [SerializeField] TMP_InputField nickName;

    public Resolution[] resolutions;
    Dictionary<int, Resolution> resolutionDict = new Dictionary<int, Resolution>();
    private List<string> WidthByHeight = new List<string>();
    int curRes;

    public void Awake()
    {
        resolutions = Screen.resolutions;
        int highestRefresh = 0;
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate > highestRefresh)
                highestRefresh = r.refreshRate;
        }
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate == highestRefresh)
            {
                WidthByHeight.Add(r.width + "x" + r.height);
                resolutionDict.Add(resolutionDict.Count, r);
            }
        }
        loadSettings();
    }

    public void loadSettings()
    {
        PlayerInfo loadData = DataSaver.loadData<PlayerInfo>("config");

        volumeText.text = "Volume: " + loadData.masterVolume;
        volumeSlider.value = loadData.masterVolume;
        fovText.text = "FOV: " + loadData.fov;
        fovSlider.value = loadData.fov;
        resolutionSelection.ClearOptions();
        resolutionSelection.AddOptions(WidthByHeight);
        foreach(var item in resolutionDict)
        {
            if ((item.Value.width == loadData.resolutionWidth) && (item.Value.height == loadData.resolutionHeight))
            {
                curRes = item.Key;
            }
        }
        resolutionSelection.value = curRes;
        fullScreen.SetIsOnWithoutNotify(loadData.fullscreen);
        nickName.text = loadData.nickname;
    }

    public void applySettings()
    {
        Screen.SetResolution(resolutionDict[resolutionSelection.value].width, resolutionDict[resolutionSelection.value].height, fullScreen.isOn);
        GameEvents.current.onNewSettingsEvent();
    }

    public void saveSettings()
    {
        PlayerInfo newSave = new PlayerInfo();
        newSave.masterVolume = (int)volumeSlider.value;
        newSave.fov = (int)fovSlider.value;
        newSave.savedResolution = resolutionDict[resolutionSelection.value];
        newSave.resolutionHeight = resolutionDict[resolutionSelection.value].height;
        newSave.resolutionWidth = resolutionDict[resolutionSelection.value].width;
        newSave.fullscreen = fullScreen.isOn;
        newSave.nickname = nickName.text;
        DataSaver.saveData(newSave, "config");
    }

    public void updateText()
    {
        volumeText.text = "Volume: " + volumeSlider.value;
        fovText.text = "FOV: " + fovSlider.value;
    }
}
