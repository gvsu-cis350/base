using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    public Transform playerPostion;
    public bool occupied = false;
    public bool driver = false;
    public Car parentCar;

    private BoxCollider col;
    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

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
