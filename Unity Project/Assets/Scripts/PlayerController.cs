using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    //Reference vars
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSenstivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject itemHolder;
    [SerializeField] Item[] items;

    //item vars
    int itemIndex;
    int previousItemIndex = -1;

    //movement vars
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    //player boundry vars
    Rigidbody rb;
    PhotonView PV;

    //health vars
    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    //refence to parent that instantiates player controller
    PlayerManager playerManager;

    Hashtable customProperties = new Hashtable();

    /// <summary>
    /// Method call which assigns objects to reference vars in script when script is referenced
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    /// <summary>
    /// Method which is called when class is contructed, and deletes references to other user's controllers
    /// </summary>
    private void Start()
    {


        if (PV.IsMine)
        {
            EquipItem(0);
        } 
        else
        {
            //Destroy(clientWeaponCamera);

            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            //PhotonNetwork.MasterClient.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    /// <summary>
    /// Update method called continously based on frame rate of user to handle local inputs
    /// </summary>
    void Update()
    {
        //exit method if we are not on the local user's Photon View id
        if (!PV.IsMine)
            return;

        //check to see if there is a pause state
        if (!playerManager.pauseState)
        {
            //run basic movement methods and weapon switching methods
            Look();
            Move();
            Jump();
            weaponSwitch();

            //check to see if the user fires their gun
            if (Input.GetMouseButtonDown(0))
            {
                items[itemIndex].Use();
            }
        }

        //kill player controller if they fall into the void
        if (transform.position.y < -10f)
        {
            Die();
        }
    }

    /// <summary>
    /// Method which takes mouse inputs and converts that into camera movement in the game
    /// </summary>
    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSenstivity);

        //note the minus sign can be changed to a plus sign to invert mouse movement
        verticalLookRotation -= Input.GetAxisRaw("Mouse Y") * mouseSenstivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    /// <summary>
    /// Method which takes keyboard inputs and stores that into a movement amount var
    /// </summary>
    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    /// <summary>
    /// Method which allows character to jump if they are on the ground
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    /// <summary>
    /// Method call for equiping items pased on a passed integer
    /// </summary>
    /// <param name="parIndex"></param>
    void EquipItem(int parIndex)
    {
        //exit if the previous and passed itemIndexes are the same
        if (parIndex == previousItemIndex)
            return;

        //set the itemIndex to passed value and make that object active in the game
        itemIndex = parIndex;
        items[itemIndex].itemGameObject.SetActive(true);



        //set previously held item to inactive
        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        //make currently held item the previously held item
        previousItemIndex = itemIndex;

        //check to see if we are the local player
        if(PV.IsMine)
        {
            //create a new hashtable
            //            Hashtable hash = new Hashtable();
            //add our item index to the hashtable
            if (customProperties.ContainsKey("itemIndex"))
            {
                customProperties.Remove("itemIndex");
            }
            //hash.Add("itemIndex", itemIndex);
            customProperties.Add("itemIndex", itemIndex);
            //send the hashtable over the photon network
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

            Transform[] children = items[itemIndex].gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform go in children)
            {
                go.gameObject.layer = 10;
            }
        }
    }

    /// <summary>
    /// Method calls whenever properites on the player are updated and then syncs those changes across the network
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //Check to see if we are not the local player (not synced)
        //and check to see if this function matches to the player that we are calling this for
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            //sync weapons if above is true
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    /// <summary>
    /// Public method to force grounded state if need be
    /// </summary>
    /// <param name="parGrounded"></param>
    public void SetGroundedState(bool parGrounded)
    {
        grounded = parGrounded;

    }

    /// <summary>
    /// Method is called at a fixed rate instead of being tied to framerate like update() to move the character model around the game
    /// </summary>
    private void FixedUpdate()
    {
        //exit method if PV ids don't match
        if (!PV.IsMine)
            return;

        //check to see a pause statue and stop movement if there is one
        if (!playerManager.pauseState)
        {
            rb.MovePosition(rb.position + (transform.TransformDirection(moveAmount) * Time.fixedDeltaTime));
        }
        //Allow player to move through the air in a pause state until they are on the ground
        else if (playerManager.pauseState && !grounded)
        {
            rb.MovePosition(rb.position + (transform.TransformDirection(moveAmount) * Time.fixedDeltaTime));
        }
    }

    /// <summary>
    /// Method calls when the local player hits a damagable enitity and this method tells that entity that they need to take damage through photon RPC.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    /// <summary>
    /// Method is established as a Remote Procedure Call where other users are told to take damage when they are hit
    /// </summary>
    /// <param name="damage"></param>
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        //exit method if PV ids don't match
        if (!PV.IsMine)
            return;

        //remove passed damage from current health
        currentHealth -= damage;

        //trigger the die method if current health is not above 1
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Reference the parent playerManager's die method to destroy this player controller
    /// </summary>
    void Die()
    {
        playerManager.Die();
    }

    /// <summary>
    /// Method handles weapon switching from various inputs
    /// </summary>
    private void weaponSwitch()
    {
        //handle inputs from the number keys
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        //handle inputs from the mouse scroll wheel
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }
    }
}
