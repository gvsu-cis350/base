using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RocketLauncher : Gun
{
    //unity reference var
    [SerializeField] Camera cam;
	[SerializeField] GameObject projectile;
	[SerializeField] Transform projectileSpawnSpot;

	//Sound effect var holder
	private GameObject temp;

	/// <summary>
	/// Method starts the reloading process when called
	/// </summary>
	public override void RefreshItem()
	{
		((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
		this.reloadTimerCoroutine = StartCoroutine(Timer());
	}

	/// <summary>
	/// Method references the base use method from item class and calls the shoot method
	/// </summary>
	public override void Use()
    {
		//If we have ammo and aren't reloading
		if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0))
        {
			Shoot();
			((GunInfo)itemInfo).currentAmmo--;

			//Code to make and attach a sound effect object
			temp = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Sounds", soundEffect.name), weaponLeftGrip.position, weaponLeftGrip.rotation, 0, new object[] { boot.bootObject.localPV.ViewID });
			temp.transform.SetParent(itemGameObject.transform);
		}
    }

    /// <summary>
    /// Method creates a rocket and sends it on its way
    /// </summary>
    private void Shoot()
	{
        // Instantiate the projectile
        if (projectile != null)
        {
            GameObject proj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Rocket"), projectileSpawnSpot.position, projectileSpawnSpot.rotation, 0, new object[] { boot.bootObject.localPV.ViewID });
        }
        else
        {
            Debug.Log("Projectile to be instantiated is null.  Make sure to set the Projectile field in the inspector.");
        }
    }
}
