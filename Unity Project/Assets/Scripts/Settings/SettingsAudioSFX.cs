﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAudioSFX : MonoBehaviour
{
    /// <summary>
    /// Method subscribes to settings update event and loads the initial Master volume
    /// </summary>
    void Start()
    {
        GameEvents.current.onSettingsUpdate += updateVolume;
        GetComponent<AudioSource>().volume = (float)(boot.bootObject.currentSettings.sfxVolume) / 100f;
    }

    /// <summary>
    /// Method loads volume setting when the new settings event occurs
    /// </summary>
    private void updateVolume()
    {
        GetComponent<AudioSource>().volume = (float)(boot.bootObject.currentSettings.sfxVolume) / 100f;
    }

    /// <summary>
    /// Method removes this object from the gameEvents list if it gets destroyed
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.current.onSettingsUpdate -= updateVolume;
    }
}
