using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    //Variables for the rider to hold
    public Transform playerPostion, exitPosition;
    public bool occupied = false;
    public bool driver = false;
    public Car parentCar;
    private BoxCollider col;

    /// <summary>
    /// Method to get the BoxCollider
    /// </summary>
    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Method turns off the box collider of rider, so that they can be hit and take damage.
    /// </summary>
    private void Update()
    {
        if (occupied)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
}
