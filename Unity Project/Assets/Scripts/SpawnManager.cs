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

    //Arrays for all the different spawnpoint lists
    Spawnpoint[] spawnpoints;
    List<Spawnpoint> bluepoints = new List<Spawnpoint>();
    List<Spawnpoint> redpoints = new List<Spawnpoint>();
    VehicleSpawnpoint[] vehiclePoints;

    private bool teamSpecificSpawns = false;

    public bool hasVehicles = false;

    /// <summary>
    /// When the class is referenced, set a global reference and compile an array of spawnpoints
    /// </summary>
    private void Awake()
    {
        Instance = this;

        //Get player spawns and sort red and blue spawns out
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
        foreach(Spawnpoint s in spawnpoints)
        {
            if (s.isBlueTeam)
            {
                bluepoints.Add(s);
            }
            else
            {
                redpoints.Add(s);
            }
        }

        //Collect vehicle spawns and register if we have vehicles
        vehiclePoints = GetComponentsInChildren<VehicleSpawnpoint>();

        if(vehiclePoints.Length >= 1)
        {
            hasVehicles = true;
        }
    }

    #region Spawnpoint Getters
    /// <summary>
    /// Method which randomly returns one of the spawnpoints so that a user can spawn
    /// </summary>
    /// <returns></returns>
    public Transform GetSpawnpoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }

    /// <summary>
    /// Method which randomly returns one of the blue spawnpoints so that a user can spawn
    /// </summary>
    /// <returns></returns>
    public Transform GetBlueSpawnpoint()
    {
        if(teamSpecificSpawns)
            return bluepoints[Random.Range(0, bluepoints.Count)].transform;
        else
            return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }

    /// <summary>
    /// Method which randomly returns one of the red spawnpoints so that a user can spawn
    /// </summary>
    /// <returns></returns>
    public Transform GetRedSpawnpoint()
    {
        if (teamSpecificSpawns)
            return redpoints[Random.Range(0, redpoints.Count)].transform;
        else
            return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }
    #endregion

    public VehicleSpawnpoint[] GetVehiclePoint()
    {
        return vehiclePoints;
    }
}
