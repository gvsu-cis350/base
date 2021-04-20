using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to verify if a player is on the ground
/// </summary>
public class PlayerGroundCheck : MonoBehaviour
{
    //variable to reference the playerController class
    PlayerController playerController;

    /// <summary>
    /// Method activates when the method is referenced and assigns playerController to a var
    /// </summary>
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    #region Trigger Methods
    /// <summary>
    /// Method changes bool when soemthing enters the Player's bottom box collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //exit method if player enters their own collider
        if (other.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(true);
    }

    /// <summary>
    /// Method changes bool when something exits the Player's bottom box collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        //exit method if player enters their own collider
        if (other.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(false);
    }

    /// <summary>
    /// Method changes bool when something stays within the Player's bottom box collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //exit method if player enters their own collider
        if (other.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(true);
    }
    #endregion

    #region Collision Methods
    /// <summary>
    /// Method changes bool when a collision entity enters the Player's bottom box collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //exit method if player enters their own collider
        if (collision.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(true);
    }

    /// <summary>
    /// Method changes bool when a collision entity exits the Player's bottom box collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        //exit method if player enters their own collider
        if (collision.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(false);
    }

    /// <summary>
    /// Method changes bool when a collision entity stays within the Player's bottom box collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        //exit method if player enters their own collider
        if (collision.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(true);
    }
    #endregion
}
