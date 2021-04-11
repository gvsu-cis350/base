using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RocketLauncher : Gun
{
    //unity reference var
    [SerializeField] Camera cam;
	[SerializeField] AudioClip fireSound;
	[SerializeField] GameObject projectile;
	[SerializeField] Transform projectileSpawnSpot;

	private Coroutine reloadTimerCoroutine;
	public int maxAmmo = 2;
	private int currentAmmo;
	public int reloadTime = 5;
	private int currentTimer;


    private void Awake()
    {
		currentAmmo = maxAmmo;
    }
    /// <summary>
    /// Method references the base use method from item class and calls the shoot method
    /// </summary>
    public override void Use()
    {
		if (currentAmmo > 0)
        {
			Shoot();
			currentAmmo--;
		}
		else if(currentAmmo <= 0)
        {
			reloadTimerCoroutine = StartCoroutine(Timer());
        }
    }

    /// <summary>
    /// Method creates a raycast from the center of the user's screen to hit target
    /// </summary>
    void Shoot()
	{
			// Instantiate the projectile
			if (projectile != null)
			{
				GameObject proj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Rocket"), projectileSpawnSpot.position, projectileSpawnSpot.rotation) as GameObject;
				proj.GetComponent<Projectile>().setPV(boot.bootObject.localPV.ViewID);
			}
			else
			{
				Debug.Log("Projectile to be instantiated is null.  Make sure to set the Projectile field in the inspector.");
			}
/*
		// Recoil
		if (recoil)
			Recoil();
*/
		// Play the gunshot sound
		GetComponent<AudioSource>().PlayOneShot(fireSound);
	}

	private IEnumerator Timer()
	{
		yield return new WaitForSeconds(1f);

		reloadTime -= 1;

		if (reloadTime <= 0)
		{
			reloadTimerCoroutine = null;
			currentAmmo = maxAmmo;
		}
		else
		{
			reloadTimerCoroutine = StartCoroutine(Timer());
		}
	}
}
