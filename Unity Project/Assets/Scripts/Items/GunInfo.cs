using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add class type for guns in unity
[CreateAssetMenu(menuName = "FPS/New Gun")]

/// <summary>
/// Public class extends the abstract of iteminfo and stores information for guns
/// </summary>
public class GunInfo : ItemInfo
{
    public float damage;
    public int maxAmmo;
    public int currentAmmo;
    public int maxReloadTime;
    public int reloadTime;
}
