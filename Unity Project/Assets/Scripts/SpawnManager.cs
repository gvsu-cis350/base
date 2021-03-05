using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which manages overall spawnpoints in a scene
/// </summary>
public class SpawnManager : MonoBehaviour
{
    //global refernce var
    public static SpawnManager Instance;

    //array to combine the total number of spawnpoints in a scene
    Spawnpoint[] spawnpoints;

    /// <summary>
    /// When the class is referenced, set a global reference and compile an array of spawnpoints
    /// </summary>
    private void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    /// <summary>
    /// Method which randomly returns one of the spawnpoints so that a user can spawn
    /// </summary>
    /// <returns></returns>
    public Transform GetSpawnpoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }
}
