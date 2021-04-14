/// <summary>
/// TimedObjectDestroyer.cs
/// Author: MutantGopher
/// This script destroys a GameObject after the number of seconds specified in
/// the lifeTime variable.  Useful for things like explosions and rockets.
/// 
/// Extra Pun additions made by Connor Boerma
/// </summary>

using UnityEngine;
using System.Collections;
using Photon.Pun;


public class ObjectDestroyer : MonoBehaviour
{
	public bool signal = false;
	public float lifeTime = 10.0f;

	// Use this for initialization
	void Start()
	{
        if (!signal)
        {
			Destroy(gameObject, lifeTime);
		}
	}

    [PunRPC]
    public void RPC_DestroyObject()
    {
        Destroy(gameObject);
    }
}
