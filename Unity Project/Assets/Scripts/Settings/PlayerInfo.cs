using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public int masterVolume;
    public int musicVolume;
    public int sfxVolume;
    public int fov;
    public Resolution savedResolution;
    public int resolutionWidth;
    public int resolutionHeight;
    public bool fullscreen;
    public string nickname;
    public float mouseSensitvity;
}
