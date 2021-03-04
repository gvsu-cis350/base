using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract class so player controller can call this use method for all items
public abstract class Item : MonoBehaviour
{
    //assign a name and object to all items
    public ItemInfo itemInfo;
    public GameObject itemGameObject;

    //method call use for all items that inherit this class
    public abstract void Use();
}
