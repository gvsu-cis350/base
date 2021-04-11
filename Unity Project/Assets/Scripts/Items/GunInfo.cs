using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add class type for guns in unity
[CreateAssetMenu(menuName = "FPS/New Gun")]

/// <summary>
/// Public class extends the abstract of iteminfo
/// </summary>
public class GunInfo : ItemInfo
{
    /// <summary>
    /// Allows unity to assign damage values to gun objects
    /// </summary>
    
    public float damage;
    public int maxAmmo;
    public int currentAmmo;
    public int maxReloadTime;
    public int reloadTime;
    private int currentTimer;
}
