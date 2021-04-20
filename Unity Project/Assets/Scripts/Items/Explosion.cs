/// <summary>
/// Explosion.cs
/// Author: MutantGopher
/// Attach this script to your explosion prefabs.  It handles damage for
/// nearby healths, force for nearby rigidbodies, and camera shaking FX.
/// 
/// Edited by Connor Boerma
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
	public float explosionForce = 5.0f;			// The force with which nearby objects will be blasted outwards
	public float explosionRadius = 10.0f;		// The radius of the explosion

	/// <summary>
	/// Method to apply explosive force to objects near explosion
	/// </summary>
	/// <returns></returns>
	IEnumerator Start()
	{
		// Wait one frame so that explosion force will be applied to objects which might not yet be instantiated
		yield return null;

		// An array of nearby colliders
		Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

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

		//Add explosive force for each rigid body
		foreach (Rigidbody rb in rigidbodies)
		{
			rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
		}
	}
}
