using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

/// <summary>
/// Class type which stems from gun and establishes single shot weapons
/// </summary>
public class SingleShotGun : Gun
{
    #region Vars
    [SerializeField] Camera cam;
    private AudioSource soundOutput;
    private PhotonView weaponPV;
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
    /// Method starts the reloading of the gun when called
    /// </summary>
    public override void RefreshItem()
    {
        //Play sound
        weaponPV.RPC("PlaySound", RpcTarget.All, 1, weaponPV.ViewID);

        //Start reload timer
        ((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
        reloadTimerCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// Method references the base use method from item class and calls the shoot method
    /// </summary>
    public override void Use()
    {
        //Ammo available and not reloading
        if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0) && (!soundOutput.isPlaying))
        {
            //Fire weapon
            RecoilShoot();
            ((GunInfo)itemInfo).currentAmmo--;

            //Create sound effect on gun
            weaponPV.RPC("PlaySound", RpcTarget.All, 0, weaponPV.ViewID);
        }
        //ammo unavailable
        else if ((((GunInfo)itemInfo).currentAmmo <= 0) && (!soundOutput.isPlaying))
        {
            //dryfire
            weaponPV.RPC("PlaySound", RpcTarget.All, 2, weaponPV.ViewID);
        }
    }

    #region Shoot Methods
    /// <summary>
    /// Old method creates a raycast from the center of the user's screen to hit target
    /// </summary>
    private void Shoot()
    {
        //Initial raycast setup
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        //detect if the ray hit an object
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //check to see if we are playing TDM
            if ((GameSettings.GameMode == GameMode.TDM) && (hit.collider.gameObject.GetComponent<PlayerControllerModelled>()))
            {
                //Check if the shooter and the shootee are on different teams
                if (hit.collider.gameObject.GetComponent<PlayerControllerModelled>().blueTeam != GameSettings.IsBlueTeam)
                {
                    //check if hit object is damagable and apply damage
                    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                }
            }
            else
            {
                //check if hit object is damagable and apply damage
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            }
        }
    }

    /// <summary>
    /// Method creates a raycast from center of screen within an accuracy cone
    /// </summary>
    void RecoilShoot()
    {
        // Fire once for each shotPerRound value
        for (int i = 0; i < ((GunInfo)itemInfo).shotsPerRound; i++)
        {
            // Calculate accuracy for this shot
            float accuracyVary = (100 - ((GunInfo)itemInfo).currentAccuracy) / 1000;
            Vector3 direction = cam.transform.forward;
            direction.x += Random.Range(-accuracyVary, accuracyVary);
            direction.y += Random.Range(-accuracyVary, accuracyVary);
            direction.z += Random.Range(-accuracyVary, accuracyVary);

            //Decrease the accuracy accordingly
            ((GunInfo)itemInfo).currentAccuracy -= ((GunInfo)itemInfo).accuracyDropPerShot;
            if (((GunInfo)itemInfo).currentAccuracy <= 0.0f)
                ((GunInfo)itemInfo).currentAccuracy = 0.0f;

            // The ray that will be used for this shot
            Ray ray = new Ray(cam.transform.position, direction);

            //detect if the ray hit an object
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //check to see if we are playing TDM
                if ((GameSettings.GameMode == GameMode.TDM) && (hit.collider.gameObject.GetComponent<PlayerControllerModelled>()))
                {
                    //Check if the shooter and the shootee are on different teams
                    if (hit.collider.gameObject.GetComponent<PlayerControllerModelled>().blueTeam != GameSettings.IsBlueTeam)
                    {
                        //check if hit object is damagable and apply damage
                        hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                    }
                }
                else
                {
                    //check if hit object is damagable and apply damage
                    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                }
            }

            /*
            // Muzzle flash effects
            if (makeMuzzleEffects)
            {
                GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
                if (muzfx != null)
                    Instantiate(muzfx, muzzleEffectsPosition.position, muzzleEffectsPosition.rotation);
            }
            */
        }
    }
    #endregion

    /// <summary>
    /// RPC Method to play sound on weapon
    /// </summary>
    /// <param name="soundID">Array Index of desired sound effect</param>
    /// <param name="weaponPVID">PhotonView ID of the weapon</param>
    [PunRPC]
    public void PlaySound(int soundID, int weaponPVID)
    {
        //Exit method if the passed ID doesn't match this weapon
        if (weaponPVID != weaponPV.ViewID)
            return;

        //Play Sound
        soundOutput.clip = soundEffect[soundID];
        soundOutput.Play();
    }
}