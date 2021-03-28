using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    //Static game event reference for the entire game
    public static GameEvents current;

    /// <summary>
    /// Set the current game event instance to the global reference and don't destroy this
    /// </summary>
    void Awake()
    {
        current = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    #region Vars for Settings update
    public event Action onSettingsUpdate;
    public event Action onNewSettings;
    #endregion

    /// <summary>
    /// Method establishes the hub which receives updates to the onSettingsUpdate event and throws the event to listeners
    /// </summary>
    public void onSettingsUpdateEvent()
    {
        if (onSettingsUpdate != null)
        {
            onSettingsUpdate();
        }
    }

    /// <summary>
    /// Method establishes the hub which receives updates to the newSettings event and throws the event to listeners
    /// </summary>
    public void onNewSettingsEvent()
    {
        if(onNewSettings != null)
        {
            onNewSettings();
        }
    }
}
