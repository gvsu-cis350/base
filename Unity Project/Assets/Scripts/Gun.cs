using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which extends the item class
/// </summary>
public abstract class Gun : Item
{
    /// <summary>
    /// Reference to the item class use method
    /// </summary>
    public abstract override void Use();
}
