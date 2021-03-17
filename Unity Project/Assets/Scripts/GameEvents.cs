using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    public event Action onSettingsUpdate;
    public event Action onNewSettings;

    public void onSettingsUpdateEvent()
    {
        if (onSettingsUpdate != null)
        {
            onSettingsUpdate();
        }
    }

    public void onNewSettingsEvent()
    {
        if(onNewSettings != null)
        {
            onNewSettings();
        }
    }
}
