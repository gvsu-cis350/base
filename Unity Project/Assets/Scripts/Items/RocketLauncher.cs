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

	private GameObject temp;
	/*
		private Coroutine reloadTimerCoroutine;
		public int maxAmmo = 2;
		private int currentAmmo;
		public int reloadTime = 5;
		private int currentTimer;
	*/

	public override void RefreshItem()
	{
		Debug.Log("Reloading");
		((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
		this.reloadTimerCoroutine = StartCoroutine(Timer());
	}

	public override int returnInfo()
	{
		return ((GunInfo)itemInfo).currentAmmo;
	}

	/// <summary>
	/// Method references the base use method from item class and calls the shoot method
	/// </summary>
	public override void Use()
    {
		if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0))
        {
			Shoot();
			((GunInfo)itemInfo).currentAmmo--;
			temp = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Sounds", soundEffect.name), weaponLeftGrip.position, weaponLeftGrip.rotation, 0, new object[] { boot.bootObject.localPV.ViewID });
			temp.transform.SetParent(itemGameObject.transform);
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
//			controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerModelled"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
				GameObject proj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Rocket"), projectileSpawnSpot.position, projectileSpawnSpot.rotation, 0, new object[] { boot.bootObject.localPV.ViewID });
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
	}
}
