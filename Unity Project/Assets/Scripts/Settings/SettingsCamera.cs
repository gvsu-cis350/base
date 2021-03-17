using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCamera : MonoBehaviour
{
    void Start()
    {
        GameEvents.current.onNewSettings += updateFOV;
        this.gameObject.GetComponent<Camera>().fieldOfView = (float)boot.bootObject.currentSettings.fov;
    }

    void updateFOV()
    {
        this.gameObject.GetComponent<Camera>().fieldOfView = (float) boot.bootObject.currentSettings.fov;
    }
}
