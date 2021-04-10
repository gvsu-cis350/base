using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAudioSFX : MonoBehaviour
{
    /// <summary>
    /// Method subscribes to settings update event and loads the initial Maseter volume
    /// </summary>
    void Start()
    {
        GameEvents.current.onSettingsUpdate += updateVolume;
        this.GetComponent<AudioSource>().volume = (float)(boot.bootObject.currentSettings.sfxVolume) / 100f;
    }

    /// <summary>
    /// Method loads volume setting when the new settings event occurs
    /// </summary>
    private void updateVolume()
    {
        this.GetComponent<AudioSource>().volume = (float)(boot.bootObject.currentSettings.sfxVolume) / 100f;
    }
}
