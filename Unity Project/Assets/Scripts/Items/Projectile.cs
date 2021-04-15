/// <summary>
/// Projectile.cs
/// Author: MutantGopher
/// Attach this script to your projectile prefabs.  This includes rockets, missiles,
/// mortars, grenade launchers, and a number of other weapons.  This script handles
/// features like seeking missiles and the instantiation of explosions on impact.
/// 
/// Edited by Connor Boerma
/// </summary>

using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    #region Vars
    public float speed = 10.0f;											// The speed at which this projectile will move
	public float initialForce = 1000.0f;								// The force to be applied to the projectile initially
	public float lifetime = 30.0f;                                      // The maximum time (in seconds) before the projectile is destroyed
	private float lifeTimer = 0.0f;                                     // The timer to keep track of how long this projectile has been in existence

	[SerializeField] GameObject explosion, explosionMaster;				
	private PhotonView PV;
    #endregion

    void Start()
	{
		// Add the initial force to rigidbodyand get the Photon View
		GetComponent<Rigidbody>().AddRelativeForce(0, 0, initialForce);
		PV = GetComponent<PhotonView>();
	}

	void Update()
	{
		// Update the timer
		lifeTimer += Time.deltaTime;

		// Destroy the projectile if the time is up
		if (lifeTimer >= lifetime)
		{
			Explode(transform.position);
		}

		// Make the projectile move
		//if (initialForce == 0)		// Only if initial force is not being used to propel this projectile
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	void OnCollisionEnter(Collision col)
	{
		// If the projectile collides with something, call the Hit() function
		Hit(col);
	}

	void Hit(Collision col)
	{
		// Make the projectile explode
		Explode(col.GetContact(0).point);
	}

	void Explode(Vector3 position)
	{
		// Instantiate the explosion
		if (explosion != null)
		{
			//Create master expolosion on the shooters local machine so that kills can be tracked
            if (PV.IsMine)
            {
				Instantiate(explosionMaster, position, Quaternion.identity);
			}
            else
            {
				Instantiate(explosion, position, Quaternion.identity);
			}
		}

		// Destroy this projectile
		Destroy(gameObject);
	}
}

