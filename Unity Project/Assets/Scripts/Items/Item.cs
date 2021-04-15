using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class so player controller can call this use method for all items
/// </summary>
public abstract class Item : MonoBehaviour
{
    #region Variables
    //assign a name and object to all items, also assign a position for both of the hands
    public ItemInfo itemInfo;
    public GameObject itemGameObject;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    #endregion

    #region Abstracts
    //Abstract classes which all items need to extend
    public abstract void Use();
    public abstract void RefreshItem();
    public abstract void ResetItem(int level);
    public abstract Hashtable returnInfo();
    #endregion
}
