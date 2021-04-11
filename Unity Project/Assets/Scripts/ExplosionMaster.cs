/// <summary>
/// Explosion.cs
/// Author: MutantGopher
/// Attach this script to your explosion prefabs.  It handles damage for
/// nearby healths, force for nearby rigidbodies, and camera shaking FX.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionMaster : MonoBehaviour
{
	public float explosionForce = 5.0f;			// The force with which nearby objects will be blasted outwards
	public float explosionRadius = 10.0f;		// The radius of the explosion
	public bool causeDamage = true;				// Whether or not the explosion should apply damage to nearby GameObjects with the Heatlh component
	public float damage = 75;				// The multiplier by which the ammount of damage to be applied is determined

	IEnumerator Start()
	{
		// Wait one frame so that explosion force will be applied to debris which
		// might not yet be instantiated
		yield return null;

		// An array of nearby colliders
		Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

		// Apply damage to any nearby GameObjects with the Health component
		if (causeDamage)
		{
			foreach (Collider col in cols)
			{
				float damageAmount = damage * (1 / Vector3.Distance(transform.position, col.transform.position));

//				col..gameObject.SendMessageUpwards("ChangeHealth", -damageAmount, SendMessageOptions.DontRequireReceiver);
				col.GetComponent<Collider>().gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);
			}
		}

		// A list to hold the nearby rigidbodies
		List<Rigidbody> rigidbodies = new List<Rigidbody>();

		foreach (Collider col in cols)
		{
			// Get a list of the nearby rigidbodies
			if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
			{
				rigidbodies.Add(col.attachedRigidbody);
			}
		}

		foreach (Rigidbody rb in rigidbodies)
		{
			rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
		}
	}
}
