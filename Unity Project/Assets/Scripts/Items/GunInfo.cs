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
    public float damage;                                // How much damage the gun does
    public int maxAmmo;                                 // Maximum ammo after reload    
    [HideInInspector] public int currentAmmo;           // The current ammo in the gun
    public int maxReloadTime;                           // How long the reload is for this gun
    [HideInInspector] public int reloadTime;            // The current reload time for a gun item

    // Accuracy
    public float accuracy = 80.0f;                      // How accurate this weapon is on a scale of 0 to 100
    [HideInInspector] public float currentAccuracy;     // Holds the current accuracy.  
    public float accuracyDropPerShot = 1.0f;            // How much the accuracy will decrease on each shot
    public float accuracyRecoverRate = 0.1f;            // How quickly the accuracy recovers after each shot (value between 0 and 1)

    [Tooltip("Leave empty if semi-auto/not applicable")]
    public float roundsPerMinute;                       // How many rounds a fully automatic weapon can fire per minute
    public int shotsPerRound;                           // How many bullets are fired per round in a multiShotGun
}
