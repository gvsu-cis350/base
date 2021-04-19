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

	//sound effects
	private AudioSource soundOutput;
	private PhotonView weaponPV;

	/// <summary>
	/// Start method to get the PhotonView and Audio Source
	/// </summary>
	private void Start()
	{
		weaponPV = GetComponent<PhotonView>();
		soundOutput = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Method starts the reloading process when called
	/// </summary>
	public override void RefreshItem()
	{
		weaponPV.RPC("PlaySound", RpcTarget.All, 1, weaponPV.ViewID);

		((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
		this.reloadTimerCoroutine = StartCoroutine(Timer());
	}

	/// <summary>
	/// Method references the base use method from item class and calls the shoot method
	/// </summary>
	public override void Use()
    {
		//If we have ammo and aren't reloading
		if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0) && (!soundOutput.isPlaying))
        {
			Shoot();
			((GunInfo)itemInfo).currentAmmo--;

			//Code to make and attach a sound effect object
			weaponPV.RPC("PlaySound", RpcTarget.All, 0, weaponPV.ViewID);
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

	[PunRPC]
	public void PlaySound(int soundID, int weaponPVID)
	{
		if (weaponPVID != weaponPV.ViewID)
			return;

		soundOutput.clip = soundEffect[soundID];
		soundOutput.Play();
	}
}
