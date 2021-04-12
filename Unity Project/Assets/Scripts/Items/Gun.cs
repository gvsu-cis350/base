using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Abstract class which extends the item class
/// </summary>
public abstract class Gun : Item
{
    /// <summary>
    /// Reference to the item class use method
    /// </summary>
    public abstract override void Use();
    public abstract override void RefreshItem();
    public abstract override int returnInfo();

	protected Coroutine reloadTimerCoroutine;
	public GameObject soundEffect;

	private void Awake()
	{
		((GunInfo)itemInfo).currentAmmo = ((GunInfo)itemInfo).maxAmmo;
	}

	public IEnumerator Timer()
	{
		yield return new WaitForSeconds(1f);

		((GunInfo)itemInfo).reloadTime -= 1;

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
