/// <summary>
/// Projectile.cs
/// Author: MutantGopher
/// Attach this script to your projectile prefabs.  This includes rockets, missiles,
/// mortars, grenade launchers, and a number of other weapons.  This script handles
/// features like seeking missiles and the instantiation of explosions on impact.
/// </summary>

using UnityEngine;
using System.Collections;
using Photon.Pun;

/*
public enum ProjectileType
{
	Standard,
	Seeker,
	ClusterBomb
}
public enum DamageType
{
	Direct,
	Explosion
}
*/

public class Projectile : MonoBehaviour
{
	public float damage = 100.0f;										// The amount of damage to be applied (only for Direct damage type)
	public float speed = 10.0f;											// The speed at which this projectile will move
	public float initialForce = 1000.0f;								// The force to be applied to the projectile initially
	public float lifetime = 30.0f;										// The maximum time (in seconds) before the projectile is destroyed
	
	[SerializeField] GameObject explosion, explosionMaster;				

	private float lifeTimer = 0.0f;                                     // The timer to keep track of how long this projectile has been in existence

	private PhotonView PV;
	void Start()
	{
		// Add the initial force to rigidbody
		GetComponent<Rigidbody>().AddRelativeForce(0, 0, initialForce);
		PV = GetComponent<PhotonView>();
	}

	// Update is called once per frame
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
//		Explode(col.contacts[0].point);

		/*
		// Apply damage to the hit object if damageType is set to Direct
		if (damageType == DamageType.Direct)
		{
			col.collider.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);

			//call the ApplyDamage() function on the enenmy CharacterSetup script
			if (col.collider.gameObject.layer == LayerMask.NameToLayer("Limb"))
			{
				Vector3 directionShot = col.collider.transform.position - transform.position;

				// Un-comment the following section for Bloody Mess support
				/*
				if (col.collider.gameObject.GetComponent<Limb>())
				{
					GameObject parent = col.collider.gameObject.GetComponent<Limb>().parent;
					CharacterSetup character = parent.GetComponent<CharacterSetup>();
					character.ApplyDamage(damage, col.collider.gameObject, weaponType, directionShot, Camera.main.transform.position);
				}
				
			}
		}
		*/
	}

	void Explode(Vector3 position)
	{
		// Instantiate the explosion
		if (explosion != null)
		{
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

	// Modify the damage that this projectile can cause
	public void MultiplyDamage(float amount)
	{
		damage *= amount;
	}

	// Modify the inital force
	public void MultiplyInitialForce(float amount)
	{
		initialForce *= amount;
	}
}

