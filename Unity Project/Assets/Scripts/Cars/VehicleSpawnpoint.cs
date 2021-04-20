using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawnpoint : MonoBehaviour
{
    //unity refrence var
    [SerializeField] GameObject graphics;

    /// <summary>
    /// Method which makes spawnpoints invisible when they are referenced/created
    /// </summary>
    private void Awake()
    {
        graphics.SetActive(false);
    }
}
