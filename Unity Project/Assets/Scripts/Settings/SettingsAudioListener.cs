using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAudioListener : MonoBehaviour
{
    /// <summary>
    /// Method subscribes to settings update event and loads the initial Master volume
    /// </summary>
    void Start()
    {
        GameEvents.current.onSettingsUpdate += updateVolume;
        AudioListener.volume = (float)(boot.bootObject.currentSettings.masterVolume) / 100f;
    }

    /// <summary>
    /// Method loads volume setting when the new settings event occurs
    /// </summary>
    private void updateVolume()
    {
        AudioListener.volume = (float)(boot.bootObject.currentSettings.masterVolume) / 100f;
    }

    /// <summary>
    /// Method removes this object from the gameEvents list if it gets destroyed
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.current.onSettingsUpdate -= updateVolume;
    }
}
