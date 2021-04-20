using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    #region Vars
    //Static game event reference for the entire game
    public static GameEvents current;

    #region Settings Update Events
    public event Action onSettingsUpdate;
    public event Action onNewSettings;
    #endregion
    #endregion

    /// <summary>
    /// Set the current game event instance to the global reference and don't destroy this
    /// </summary>
    void Awake()
    {
        current = this;
        DontDestroyOnLoad(transform.gameObject);
    }

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
