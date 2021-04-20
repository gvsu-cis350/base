using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Abstract class which extends the item class
/// </summary>
public abstract class Gun : Item
{
	#region Vars
	#region Abstracts
	public abstract override void Use();
    public abstract override void RefreshItem();
    #endregion

    protected Coroutine reloadTimerCoroutine;
	public AudioClip[] soundEffect;
	protected bool canFire = true;
	#endregion

	/// <summary>
	/// Method returns information on the gun
	/// </summary>
	/// <returns></returns>
	public override Hashtable returnInfo()
	{
		Hashtable info = new Hashtable();
		info.Add("currentAmmo", ((GunInfo)itemInfo).currentAmmo);
		info.Add("maxAmmo", ((GunInfo)itemInfo).maxAmmo);
		return info;
	}

	/// <summary>
	/// Method to reset various variables within a gun based on the level of reset that is passed
	/// </summary>
	/// <param name="level">Level code for resets, 0 = full reset, 1 = reloadtimer reset</param>
	public override void ResetItem(int level)
	{
		//Level 0 fully resets the gun (intended to be called at the start of a match/upon respawn)
		if (level == 0)
		{
			((GunInfo)itemInfo).currentAmmo = ((GunInfo)itemInfo).maxAmmo;
			((GunInfo)itemInfo).reloadTime = 0;
		}
		//Level 1 resets the reload timer for when a player cancels a reload
		else if (level == 1)
        {
			if(reloadTimerCoroutine != null)
            {
				StopCoroutine(reloadTimerCoroutine);
				((GunInfo)itemInfo).reloadTime = 0;
			}
		}
	}

	/// <summary>
	/// Timer coroutine class for reloads
	/// </summary>
	/// <returns></returns>
	public IEnumerator Timer()
	{
		//Wait a second and then decrement the reload time
		yield return new WaitForSeconds(1f);
		((GunInfo)itemInfo).reloadTime -= 1;

		//End timer and reload gun if the timer is over, else continue timer.
		if (((GunInfo)itemInfo).reloadTime <= 0)
		{
			reloadTimerCoroutine = null;
			((GunInfo)itemInfo).currentAmmo = ((GunInfo)itemInfo).maxAmmo;
			((GunInfo)itemInfo).reloadTime = 0;
		}
		else
		{
			reloadTimerCoroutine = StartCoroutine(Timer());
		}
	}
}
