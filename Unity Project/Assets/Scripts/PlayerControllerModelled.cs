using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.Animations.Rigging;

public class PlayerControllerModelled : MonoBehaviourPunCallbacks, IDamageable
{
    //Reference vars
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSenstivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject firstItemHolder, thirdItemHolder;
    [SerializeField] Item[] firstItems;
    [SerializeField] Material Host, Regular;
    [SerializeField] GameObject firstPersonModel;
    [SerializeField] MultiPositionConstraint cameraRot;

    Animator firstAnimation;

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

    //health and shield vars
    const float maxHealth = 100f;
    float currentHealth = maxHealth;
//    const float maxShields = 75f;
//    float currentShields = maxShields;

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
        firstAnimation = firstPersonModel.GetComponent<Animator>();

    }

    /// <summary>
    /// Method which is called when class is contructed, and deletes references to other user's controllers
    /// </summary>
    private void Start()
    {
        if (PV.IsMine)
        {
            GameEvents.current.onSettingsUpdate += updateMouse;
            EquipItem(0);
            if (customProperties.ContainsKey("app"))
            {
                customProperties.Remove("app");
            }
            if (PhotonNetwork.IsMasterClient)
            {
                customProperties.Add("app", 1);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
            }
            else
            {
                customProperties.Add("app", 2);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
            }
            Transform[] fschildren = firstPersonModel.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform go in fschildren)
            {
                go.gameObject.layer = 10;
            }
        } 
        else
        {
            Destroy(firstPersonModel.gameObject.GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            //Destroy(firstPersonModel);
            //Destroy(firstItemHolder);
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
                firstItems[itemIndex].Use();
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
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60f, 58f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;

        float zOffset;

        if(cameraHolder.transform.localEulerAngles.x > 180)
        {
            zOffset = (((cameraHolder.transform.rotation.eulerAngles.x - 360) / -58) * -0.12f);
//            Debug.Log(cameraHolder.transform.rotation.eulerAngles.x / -58);
//            Debug.Log(zOffset);
            cameraRot.data.offset = new Vector3(0, 0, zOffset);
        } else
        {
            zOffset = ((cameraHolder.transform.rotation.eulerAngles.x / 60) * 0.08f);
            cameraRot.data.offset = new Vector3(0, 0, zOffset);
        }

        //cameraRot.data.offset = new Vector3(0, 0, (((float)cameraHolder.transform.rotation.eulerAngles.x) / 60));
        
        //cameraDown.weight = ((float) cameraHolder.transform.rotation.eulerAngles.x) / 60;
    }

    /// <summary>
    /// Method which takes keyboard inputs and stores that into a movement amount var
    /// </summary>
    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        firstAnimation.SetFloat("InputX", moveAmount.x);
        firstAnimation.SetFloat("InputZ", moveAmount.z);
/*
        if ((moveDir.x != 0) || (moveDir.z != 0))
        {
            firstAnimation.SetBool("Walking", true);
        } 
        else
        {
            firstAnimation.SetBool("Walking", false);
        }
*/
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
        firstItems[itemIndex].itemGameObject.SetActive(true);

        //set previously held item to inactive
        if (previousItemIndex != -1)
        {
            firstItems[previousItemIndex].itemGameObject.SetActive(false);
        }

        //make currently held item the previously held item
        previousItemIndex = itemIndex;

        //check to see if we are the local player
        if(PV.IsMine)
        {
            //add our item index to the hashtable
            if (customProperties.ContainsKey("itemIndex"))
            {
                customProperties.Remove("itemIndex");
            }
            //hash.Add("itemIndex", itemIndex);
            customProperties.Add("itemIndex", itemIndex);
            //send the hashtable over the photon network
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

            //Select all of the items held in the local player's hand
            Transform[] fschildren = firstItems[itemIndex].gameObject.GetComponentsInChildren<Transform>();
            //Set them to only be rendered by the fixed FOV camera
            foreach (Transform go in fschildren)
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
            if (changedProps.ContainsKey("itemIndex"))
            {
                EquipItem((int)changedProps["itemIndex"]);
            }
            
            if (changedProps.ContainsKey("app"))
            {
                changeAppearance((int)changedProps["app"]);
            }
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
        for (int i = 0; i < firstItems.Length; i++)
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
            if (itemIndex >= firstItems.Length - 1)
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
                EquipItem(firstItems.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }
    }

    public void changeAppearance(int mat)
    {
        if(mat == 1)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = Host;
        } 
        else if(mat == 2)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = Regular;
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
    }

    private void updateMouse()
    {
        mouseSenstivity = boot.bootObject.currentSettings.mouseSensitvity;
    }
}
