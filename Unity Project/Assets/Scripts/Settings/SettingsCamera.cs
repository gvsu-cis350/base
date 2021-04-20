using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCamera : MonoBehaviour
{
    /// <summary>
    /// Method subscribes to the new settings event and load the initial camera FOV settings
    /// </summary>
    void Start()
    {
        GameEvents.current.onNewSettings += updateFOV;
        GetComponent<Camera>().fieldOfView = (float)boot.bootObject.currentSettings.fov;
    }

    /// <summary>
    /// Method gets called whenever the settings update to update the FOV of the camera 
    /// </summary>
    void updateFOV()
    {
        GetComponent<Camera>().fieldOfView = (float) boot.bootObject.currentSettings.fov;
    }

    /// <summary>
    /// Method removes this object from the gameEvents list if it gets destroyed
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.current.onNewSettings -= updateFOV;
    }
}
