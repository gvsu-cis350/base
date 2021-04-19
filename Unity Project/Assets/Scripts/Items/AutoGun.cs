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
    //unity reference var
    [SerializeField] Camera cam;

    public float firerate;
    private bool isFiring = false;

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
    /// Method starts the reloading of the gun when called
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
        StartCoroutine(nextShoot());
    }

    private IEnumerator nextShoot()
    {
        yield return new WaitForSeconds(0.1f);

        //Ammo available and not reloading
        if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0))
        {
            Shoot();
            ((GunInfo)itemInfo).currentAmmo--;

            //Create sound effect on gun
            if (!isFiring)
            {
                isFiring = true;
                weaponPV.RPC("PlaySoundAuto", RpcTarget.All, 0, weaponPV.ViewID, isFiring);
            }
        }
        //ammo unavailable
        else if (((GunInfo)itemInfo).currentAmmo <= 0)
        {
            //dryfire
        }

        Debug.Log(Input.GetMouseButton(0));

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(nextShoot());
        }
        else
        {
            isFiring = false;
            weaponPV.RPC("PlaySoundAuto", RpcTarget.All, 0, weaponPV.ViewID, isFiring);
        }
    }

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
            //Debug.Log("We hit " + hit.collider.gameObject.name);
        }
    }

    [PunRPC]
    public void PlaySoundAuto(int soundID, int weaponPVID, bool firing)
    {
        if (weaponPVID != weaponPV.ViewID)
            return;

        if(soundID == 0)
        {
            if (firing)
            {
                soundOutput.clip = soundEffect[soundID];
                soundOutput.Play();
            }
            else
            {
                soundOutput.Stop();
            }
        }
        else
        {
            soundOutput.clip = soundEffect[soundID];
            soundOutput.Play();
        }
    }
}