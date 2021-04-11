using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class type which stems from gun and establishes single shot weapons
/// </summary>
public class SingleShotGun : Gun
{
    //unity reference var
    [SerializeField] Camera cam;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloading");
            ((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
            reloadTimerCoroutine = StartCoroutine(Timer());
        }
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
            Debug.Log("bang");
            Shoot();
            ((GunInfo)itemInfo).currentAmmo--;
        }
        else if (((GunInfo)itemInfo).currentAmmo <= 0)
        {
            //dryfire
        }
    }

    /// <summary>
    /// Method creates a raycast from the center of the user's screen to hit target
    /// </summary>
    void Shoot()
    {
        //Initial raycast setup
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        //detect if the ray hit an object
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //check to see if we are playing TDM
            if((GameSettings.GameMode == GameMode.TDM) && (hit.collider.gameObject.GetComponent<PlayerControllerModelled>()))
            {
                //Check if the shooter and the shootee are on different teams
                if ((hit.collider.gameObject.GetComponent<PlayerControllerModelled>().blueTeam != GameSettings.IsBlueTeam))// || (hit.collider.gameObject.GetComponent<PlayerController>().blueTeam != GameSettings.IsBlueTeam))
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
            //    Debug.Log("We hit " + hit.collider.gameObject.name);
        }
    }
}