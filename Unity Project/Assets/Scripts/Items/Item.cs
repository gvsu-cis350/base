using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class so player controller can call this use method for all items
/// </summary>
public abstract class Item : MonoBehaviour
{
    //assign a name and object to all items
    public ItemInfo itemInfo;
    public GameObject itemGameObject;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    /// <summary>
    /// Method call use for all items that inherit this class
    /// </summary>
    public abstract void Use();
}
