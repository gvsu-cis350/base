using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RocketLauncher : Gun
{
    #region Vars
    #region Inspector Vars
    [SerializeField] Camera cam;
	[SerializeField] GameObject projectile;
	[SerializeField] Transform projectileSpawnSpot;
    #endregion

    #region Components
    private AudioSource soundOutput;
	private PhotonView weaponPV;
    #endregion
    #endregion
    
	/// <summary>
    /// Start method to get the PhotonView and Audio Source components
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
		//Play sound
		weaponPV.RPC("PlaySound", RpcTarget.All, 1, weaponPV.ViewID);

		//Start coroutine
		((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
		reloadTimerCoroutine = StartCoroutine(Timer());
	}

	/// <summary>
	/// Method references the base use method from item class and calls the shoot method
	/// </summary>
	public override void Use()
    {
		//If we have ammo and aren't reloading
		if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0) && (!soundOutput.isPlaying))
        {
			//Fire
			Shoot();
			((GunInfo)itemInfo).currentAmmo--;

			//Play shoot sound effect
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

	/// <summary>
	/// Method to play sound on this object
	/// </summary>
	/// <param name="soundID">Array Index of desired sound effect</param>
	/// <param name="weaponPVID">PhotonView ID of object that sound will play on</param>
	[PunRPC]
	public void PlaySound(int soundID, int weaponPVID)
	{
		//Exit if the ID of this weapon doesn't match this object's
		if (weaponPVID != weaponPV.ViewID)
			return;

		//Play sound effect
		soundOutput.clip = soundEffect[soundID];
		soundOutput.Play();
	}
}
