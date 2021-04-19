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
    //unity reference var
    [SerializeField] Camera cam;

    //sound effect holder
    private GameObject temp;

    /// <summary>
    /// Method starts the reloading of the gun when called
    /// </summary>
    public override void RefreshItem()
    {
        temp = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Sounds", soundEffect[1].name), weaponLeftGrip.position, weaponLeftGrip.rotation, 0, new object[] { boot.bootObject.localPV.ViewID });
        temp.transform.SetParent(itemGameObject.transform);
        ((GunInfo)itemInfo).reloadTime = ((GunInfo)itemInfo).maxReloadTime;
        this.reloadTimerCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// Method references the base use method from item class and calls the shoot method
    /// </summary>
    public override void Use()
    {
        //Ammo available and not reloading
        if ((((GunInfo)itemInfo).currentAmmo > 0) && (((GunInfo)itemInfo).reloadTime <= 0))
        {
            RecoilShoot();
            ((GunInfo)itemInfo).currentAmmo--;

            //Create sound effect on gun and attach it to the gun
            temp = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Sounds", soundEffect[0].name), weaponLeftGrip.position, weaponLeftGrip.rotation, 0, new object[] { boot.bootObject.localPV.ViewID });
            temp.transform.SetParent(itemGameObject.transform);
        }
        //ammo unavailable
        else if (((GunInfo)itemInfo).currentAmmo <= 0)
        {
            //dryfire
        }
    }

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
            //Debug.Log("We hit " + hit.collider.gameObject.name);
        }
    }



    /// <summary>
    /// Method creates a raycast from center of screen with an accuracy cone
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
//                Debug.Log("We hit " + hit.collider.gameObject.name);
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
}