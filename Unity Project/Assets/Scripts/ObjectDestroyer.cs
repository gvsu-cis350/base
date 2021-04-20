/// <summary>
/// TimedObjectDestroyer.cs
/// Author: MutantGopher
/// This script destroys a GameObject after the number of seconds specified in
/// the lifeTime variable.  Useful for things like explosions and rockets.
/// 
/// Extra Pun additions made by Connor Boerma
/// </summary>

using UnityEngine;
using System.Collections;
using Photon.Pun;


public class ObjectDestroyer : MonoBehaviour
{
    //Signal bool to determine if this object will be destroyed with an RPC
	public bool signal = false;
	public float lifeTime = 10.0f;

	/// <summary>
    /// Method to start countdown
    /// </summary>
	void Start()
	{
        //If the is a PhotonView, then check fo signal
        if(GetComponent<PhotonView>() != null)
        {
            if (!signal)
            {
                //Start countdown if no signal
                StartCoroutine(End(lifeTime));
            }
        }
        //Start destroy countdown if no PhotonView
        else
        {
            Destroy(gameObject, lifeTime);
        }
	}

    /// <summary>
    /// RPC to get destroy signal
    /// </summary>
    [PunRPC]
    public void RPC_DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// Coroutine to destroy object with a PhotonView
    /// </summary>
    /// <param name="p_wait">Lfietime of the object</param>
    /// <returns></returns>
    private IEnumerator End(float p_wait)
    {
        yield return new WaitForSeconds(p_wait);

        PhotonNetwork.Destroy(gameObject);
    }
}
