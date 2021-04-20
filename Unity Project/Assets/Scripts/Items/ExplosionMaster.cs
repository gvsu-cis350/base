/// <summary>
/// Explosion.cs
/// Author: MutantGopher
/// Attach this script to your explosion prefabs.  It handles damage for
/// nearby healths, force for nearby rigidbodies, and camera shaking FX.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class which applies explosive force and damage
/// </summary>
public class ExplosionMaster : MonoBehaviour
{
    #region Vars
    public float explosionForce = 5.0f;         // The force with which nearby objects will be blasted outwards
    public float explosionRadius = 10.0f;       // The radius of the explosion
    public float damage = 75;               // The multiplier by which the ammount of damage to be applied is determined
    #endregion

    /// <summary>
    /// Method to apply explosion force to nearby objects and damage them as well
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

        //Iterate through all nearby objects
        foreach (Collider col in cols)
        {
            /* Damage Calulation based on base damage and distance from explosion
            //Calculate damage from explosion length
            float damageAmount = damage * (1 / Vector3.Distance(transform.position, col.transform.position));

            //Apply Damage to all IDamageable objects
            col.GetComponent<Collider>().gameObject.GetComponent<IDamageable>()?.TakeDamage(damageAmount);
            */

            //Apply Damage to all IDamageable objects
            col.GetComponent<Collider>().gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);

            //Find rigidbodies in the list of nearby objects
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }

        //Add explosive force to all rigidbodies
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
        }
    }
}
