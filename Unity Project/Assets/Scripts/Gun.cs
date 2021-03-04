using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class which extends the item class
public abstract class Gun : Item
{
    //Reference to the item class use method
    public abstract override void Use();
}
