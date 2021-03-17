using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAudio : MonoBehaviour
{
    void Start()
    {
        GameEvents.current.onSettingsUpdate += updateCamera;
        AudioListener.volume = (float)(boot.bootObject.currentSettings.masterVolume) / 100f;
    }

    private void updateCamera()
    {
        AudioListener.volume = (float)(boot.bootObject.currentSettings.masterVolume) / 100f;
    }
}
