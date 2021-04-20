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

	/// <summary>
	/// Method start projectile with proper values
	/// </summary>
    void Start()
	{
		// Add the initial force to rigidbodyand get the Photon View
		GetComponent<Rigidbody>().AddRelativeForce(0, 0, initialForce);
		PV = GetComponent<PhotonView>();
	}

	/// <summary>
	/// Method to maintain updates of projectile
	/// </summary>
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
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	/// <summary>
	/// Method to make projectile explode when it hits another object
	/// </summary>
	/// <param name="col">Collision that this projectile entered into</param>
	void OnCollisionEnter(Collision col)
	{
		// If the projectile collides with something, call the Explode() function
		Explode(col.GetContact(0).point);
	}

	/// <summary>
	/// Method causes the projectile to explode based on its postion
	/// </summary>
	/// <param name="position"></param>
	void Explode(Vector3 position)
	{
		// Instantiate the explosion
		if (explosion != null)
		{
			//Create master expolosion on the shooter's local machine so that kills can be tracked
            if (PV.IsMine)
            {
				Instantiate(explosionMaster, position, Quaternion.identity);
			}
			//Create force expolosion on everyone else's machine
            else
            {
				Instantiate(explosion, position, Quaternion.identity);
			}
		}

		// Destroy this projectile
		Destroy(gameObject);
	}
}

