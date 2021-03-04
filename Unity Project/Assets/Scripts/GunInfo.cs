using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add class type for guns in unity
[CreateAssetMenu(menuName = "FPS/New Gun")]

//public class extends the abstract of iteminfo
public class GunInfo : ItemInfo
{
    //Allows unity to assign damage values to gun objects
    public float damage;
}
