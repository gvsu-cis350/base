using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to check if a player is on the ground
public class PlayerGroundCheck : MonoBehaviour
{
    //variable to reference the playerController class
    PlayerController playerController;

    //when this class is referenced
    private void Awake()
    {
        //set playerController reference too the playerController from this class' parent object
        playerController = GetComponentInParent<PlayerController>();
    }

    //Method triggers when soemthing enters the Player's bottom box collider
    private void OnTriggerEnter(Collider other)
    {
        //check to see if the player's body entered the box collider
        if (other.gameObject == playerController.gameObject)
            //if it did, return and exit method
            return;
        //if something did enter, set the player's status as on the ground
        playerController.SetGroundedState(true);
    }

    //Method triggers when something exits the Player's bottom box collider
    private void OnTriggerExit(Collider other)
    {
        //check to see if the player's body entered the box collider
        if (other.gameObject == playerController.gameObject)
            //if it did, return and exit method
            return;
        //if something did exit, set the player's status as not on the ground
        playerController.SetGroundedState(false);
    }

    //Method triggers when something stays within the Player's bottom box collider
    private void OnTriggerStay(Collider other)
    {
        //check to see if the player's body entered the box collider
        if (other.gameObject == playerController.gameObject)
            //if it did, return and exit method
            return;
        //if something is in the box, set the player's status as on the ground
        playerController.SetGroundedState(true);
    }

    //Method triggers when a collision entity enters the Player's bottom box collider
    private void OnCollisionEnter(Collision collision)
    {
        //check to see if the player's body entered the box collider
        if (collision.gameObject == playerController.gameObject)
            //if it did, return and exit method
            return;
        //if something did enter, set the player's status as on the ground
        playerController.SetGroundedState(true);
    }

    //Method triggers when a collision entity exits the Player's bottom box collider
    private void OnCollisionExit(Collision collision)
    {
        //check to see if the player's body entered the box collider
        if (collision.gameObject == playerController.gameObject)
            //if it did, return and exit method
            return;
        //if something did exit, set the player's status as not on the ground
        playerController.SetGroundedState(false);
    }

    //Method triggers when a collision entity stays within the Player's bottom box collider
    private void OnCollisionStay(Collision collision)
    {
        //check to see if the player's body entered the box collider
        if (collision.gameObject == playerController.gameObject)
            //if it did, return and exit method
            return;
        //if something is in the box, set the player's status as on the ground
        playerController.SetGroundedState(true);
    }
}
