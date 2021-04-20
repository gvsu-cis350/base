using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

/// <summary>
/// Class type which stems from gun and establishes single shot weapons
/// </summary>
public class AutoGun : Gun
{
    #region Vars
    private bool isFiring = false;

    #region Components
    [SerializeField] Camera cam;
    private AudioSource soundOutput;
    private PhotonView weaponPV;
    #endregion
    #endregion

    /// <summary>
    /// Start method to get the PhotonView and Audio Source
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
        //Play reload sound
        weaponPV.RPC("PlaySoundAuto", RpcTarget.All, 1, weaponPV.ViewID, false);

        //Start reload timer
        ((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
        this.reloadTimerCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// Method references the base use method from item class and calls the shoot method
    /// </summary>
    public override void Use()
    {
        StartCoroutine(nextShoot());
    }

    /// <summary>
    /// Coroutine to fire the gun at a specific FireRate
    /// </summary>
    /// <returns></returns>
    private IEnumerator nextShoot()
    {
        //Wait for the preset firerate time, or set to the minimum threshold for coroutine
        yield return new WaitForSeconds(((60f / ((GunInfo)itemInfo).roundsPerMinute) >= 0.1f)? (60f / ((GunInfo)itemInfo).roundsPerMinute) : 0.1f);

        //Ammo available and not reloading
        if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0))
        {
            //Fire the gun
            RecoilShoot();
            ((GunInfo)itemInfo).currentAmmo--;

            //Create sound effect on gun
            if (!isFiring)
            {
                isFiring = true;
                weaponPV.RPC("PlaySoundAuto", RpcTarget.All, 0, weaponPV.ViewID, isFiring);
            }
        }

        //If we are still firing and have ammo
        if (Input.GetMouseButton(0) && (((GunInfo)itemInfo).currentAmmo > 0))
        {
            //Fire again
            StartCoroutine(nextShoot());
        }
        //If we are not firing and have ammo
        else if (!Input.GetMouseButton(0) && (((GunInfo)itemInfo).currentAmmo > 0))
        {
            //Stop firing
            isFiring = false;
            weaponPV.RPC("PlaySoundAuto", RpcTarget.All, 0, weaponPV.ViewID, isFiring);
        }
        //If we are trying to fire and don't have ammo
        else if (Input.GetMouseButton(0) && (((GunInfo)itemInfo).currentAmmo <= 0))
        {
            //Dryfire
            weaponPV.RPC("PlaySoundAuto", RpcTarget.All, 2, weaponPV.ViewID, isFiring);
        }
    }

    #region Shoot Methods
    /// <summary>
    /// Method creates a raycast from the center of the user's screen to hit target
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

            //Decrease the shot in accordance to accuracy levels
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
    /// RPC Method to play sound on everyone's local copy of the player
    /// </summary>
    /// <param name="soundID">Array Index corresponding to desired sound effect</param>
    /// <param name="weaponPVID">PhotonView ID for the weapon that is firing/needs to play sound</param>
    /// <param name="firing">Boolean to turn on and off the looping </param>
    [PunRPC]
    public void PlaySoundAuto(int soundID, int weaponPVID, bool firing)
    {
        //Exit method if the weapon ids don't match
        if (weaponPVID != weaponPV.ViewID)
            return;

        //Check if we are given the shot sound effect
        if(soundID == 0)
        {
            //Check if we are firing
            if (firing)
            {
                //Play sounds
                soundOutput.loop = true;
                soundOutput.clip = soundEffect[soundID];
                soundOutput.Play();
            }
            else
            {
                //Stop
                soundOutput.Stop();
            }
        }
        else
        {
            //Play non-shot sound effect
            soundOutput.loop = false;
            soundOutput.clip = soundEffect[soundID];
            soundOutput.Play();
        }
    }
}