using System.Collections;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Class is the controller for player controller and allows the player to move around
/// </summary>
public class PlayerControllerModelled : MonoBehaviourPunCallbacks, IDamageable
{
    #region Vars
    #region Inspector Reference Vars
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSenstivity, speed, jumpForce, smoothTime;
    [SerializeField] GameObject itemHolder;
    [SerializeField] GameObject weaponPivot;
    [SerializeField] Item[] items;
    [SerializeField] GameObject Helmet, Body;
    [SerializeField] Material BlueHelmet, BlueBody, RedHelmet, RedBody;
    [SerializeField] GameObject playerModel;
    [SerializeField] RigBuilder rigBuilder;
    [SerializeField] Transform weaponLeftGrip, weaponRightGrip;
    [SerializeField] Camera cam;
    #endregion

    #region Item Vars
    private bool allWeapons;
    private int primaryWeapon;
    private int secondaryWeapon;
    public int itemIndex;
    int previousItemIndex = -1;
    #endregion

    #region Location and Rotation Vars
    private float verticalLookRotation;
    private float vehicleLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    #endregion

    #region Player Vars
    Rigidbody rb;
    PhotonView PV;
    public PlayerManager playerManager;
    Hashtable customProperties = new Hashtable();
    Animator Animation;
    public bool blueTeam = GameSettings.IsBlueTeam;
    private bool inVehicle = false;
    private Rider currentSeat = null;
    #endregion

    #region Health and Shield Vars
    //Values are based off of Halo 3's data on the halopedia wiki under health
    private const float maxHealth = 45f;
    private float currentHealth = maxHealth;
    private const float maxShields = 70f;
    private float currentShields = maxShields;
    private int healthRechargeWait = 10;
    private int healthRechargePerSecond = 9;
    private int shieldRechargeWait = 5;
    private int shieldRechargePerSecond = 35;
    private int lastHit = 10;
    #endregion
    #endregion

    #region Creation Methods
    /// <summary>
    /// Method call which assigns objects to reference vars in script when script is referenced
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        Animation = playerModel.GetComponent<Animator>();
        customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (PV.IsMine)
        {
            foreach (Item i in items)
            {
                i.ResetItem(0);
            }
        }
    }

    /// <summary>
    /// Method which is called on 1st frame after construction, updates player with proper settings
    /// </summary>
    private void Start()
    {
        //Remove the material entry in the hashmap if there is one for this player
        if (customProperties.ContainsKey("Team"))
            customProperties.Remove("Team");

        //If this is my playerController
        if (PV.IsMine)
        {
            //Sync team if TDM mode
            if (GameSettings.GameMode == GameMode.TDM)
            {
                PV.RPC("SyncTeam", RpcTarget.All, GameSettings.IsBlueTeam);
            }

            //Get settings for 2 weapons or all weapons
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("AllWeapons"))
            {
                allWeapons = (bool)PhotonNetwork.CurrentRoom.CustomProperties["AllWeapons"];
            }

            //Ignore 2 weapon settings if all weapons
            if (allWeapons)
            {
                primaryWeapon = 0;
            }
            //Set up 2 weapon system
            else
            { 
                primaryWeapon = playerManager.primaryWeaponPM;
                secondaryWeapon = playerManager.secondaryWeaponPM;
            }
            
            //Subscribe the mouse senstivity method to settings update event
            GameEvents.current.onSettingsUpdate += updateMouse;

            //Equip the first item available
            EquipItem(primaryWeapon);

            //If TDM mode
            if (GameSettings.GameMode == GameMode.TDM)
            {
                //Change apperance based on team
                customProperties.Add("Team", blueTeam);
                changeAppearance(blueTeam);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
            }

            //Set the entire player model to the static FOV camera layer
            Transform[] fschildren = playerModel.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform go in fschildren)
            {
                go.gameObject.layer = 10;
            }

            //Remove helemt from disaplaying on the user's cameras
            Helmet.layer = 12;
        }
        else
        {
            //Remove components of local copies of playerControllers that this player doesn't own
            rigBuilder.layers.RemoveAt(1);
            Destroy(playerModel.gameObject.GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }

        //Invoke the rigbuilder to enable at a slight delay due to the issues between Photon and the rigging system
        Invoke(nameof(delayedRigBuilder), 0.001f);
    }

    /// <summary>
    /// Method to enable the rig builder so that hand, body, and weapon tracking work
    /// </summary>
    private void delayedRigBuilder()
    {
        rigBuilder.enabled = true;
    }
    #endregion

    /// <summary>
    /// Update method called continously based on frame rate of user to handle local inputs
    /// </summary>
    private void Update()
    {
        //Exit method if we are not on the local user's Photon View id
        if (!PV.IsMine)
            return;

        //Check to see if there is a the current game state is set to playing
        if ((int)playerManager.state == 2)
        {
            //Run basic movement methods and weapon switching methods depending on vehicle status
            if (!inVehicle)
            {
                Look();
                Move();
                Jump();
            }
            else if (!currentSeat.name.Equals("Driver"))
            {
                Look();
            }
            
            //Method to handle weapon switching
            weaponSwitch();

            //Check to see if the user fires their gun
            if (Input.GetMouseButtonDown(0))
            {
                items[itemIndex].Use();
            }

            //Update the ammo counter
            playerManager.ammoCounter.text = "" + items[itemIndex].returnInfo()["currentAmmo"].ToString() + "/" + items[itemIndex].returnInfo()["maxAmmo"].ToString();

            //Check for reload input
            if (Input.GetKeyDown(KeyCode.R))
            {
                items[itemIndex].RefreshItem();
            }
        }
        else
        {
            //Make sure that the character only animates the idle animation while paused
            Animation.SetFloat("InputX", 0);
            Animation.SetFloat("InputZ", 0);
        }

        //Show depleted shields if applicable
        if (currentShields <= 0)
        {
            playerManager.shields.gameObject.SetActive(false);
            playerManager.depletedShields.gameObject.SetActive(true);
        }
        //Show regular sheilds if applicalble
        else
        {
            playerManager.shields.gameObject.SetActive(true);
            playerManager.depletedShields.gameObject.SetActive(false);
            playerManager.shields.value = currentShields;
        }

        //kill player controller if they fall into the void
        if (transform.position.y < -10f)
        {
            Die();
        }

        //Run method to check if player attempted to enter/exit vehicle
        VehicleCheck();
    }

    #region Movement
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
    }

    /// <summary>
    /// Method which takes keyboard inputs and stores that into a movement amount var
    /// </summary>
    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        //Sprint Code
        //moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * speed, ref smoothMoveVelocity, smoothTime);

        //Tell the animator which direction the character is moving in
        Animation.SetFloat("InputX", moveAmount.x);
        Animation.SetFloat("InputZ", moveAmount.z);

        /* WIP VELOCITY BASED MOVEMENT
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveDir = transform.TransformDirection(moveDir);

        //moveAmount = moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed) * smoothTime;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        //Tell the animator which direction the character is moving in
        //Note that this doesn't match perfectly with the movement of the character as that movement was changed from positional changes to velocity changes however there was not enough time to find a solution
        Animation.SetFloat("InputX", Input.GetAxisRaw("Horizontal") * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed) * smoothTime);
        Animation.SetFloat("InputZ", Input.GetAxisRaw("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed) * smoothTime);
        */
    }

    /// <summary>
    /// Method which allows character to jump if they are on the ground
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
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
    #endregion

    #region Weapons
    /// <summary>
    /// Method call for equiping items pased on a passed integer
    /// </summary>
    /// <param name="parIndex">Index of the weapon that we are equiping</param>
    void EquipItem(int parIndex)
    {
        //Exit if the previous and passed itemIndexes are the same
        if (parIndex == previousItemIndex)
            return;

        //Set the itemIndex to passed value and make that object active in the game
        itemIndex = parIndex;
        items[itemIndex].itemGameObject.SetActive(true);

        //Set previously held item to inactive
        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        //Make the previously held item equal to the currently held item
        previousItemIndex = itemIndex;

        //Check to see if this is my player
        if (PV.IsMine)
        {
            //Add our item index to the custom properties for my player
            if (customProperties.ContainsKey("itemIndex"))
                customProperties.Remove("itemIndex");
            customProperties.Add("itemIndex", itemIndex);

            //Send the hashtable over the photon network
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

            //Select all of the items held in the local player's hand
            Transform[] fschildren = items[itemIndex].gameObject.GetComponentsInChildren<Transform>();
            //Set them to only be rendered by the fixed FOV camera
            foreach (Transform go in fschildren)
            {
                go.gameObject.layer = 10;
            }

        }

        //Set new hand locations based on stored hand locations for the Animation Rig
        weaponLeftGrip.transform.localPosition = items[itemIndex].weaponLeftGrip.transform.localPosition;
        weaponLeftGrip.transform.localRotation = items[itemIndex].weaponLeftGrip.transform.localRotation;
        weaponRightGrip.transform.localPosition = items[itemIndex].weaponRightGrip.transform.localPosition;
        weaponRightGrip.transform.localRotation = items[itemIndex].weaponRightGrip.transform.localRotation;
    }

    /// <summary>
    /// Method handles weapon switching from various inputs
    /// </summary>
    private void weaponSwitch()
    {
        //Note all reset items functions take 1 as a code to stop and reset the reload timer of that weapon

        //Check if all weapons are equiped or only 2 weapons are allowed
        if (allWeapons)
        {
            //Handle inputs from the number keys
            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(i);
                    break;
                }
            }

            //Handle inputs from the mouse scroll wheel going up
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if (itemIndex >= items.Length - 1)
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(0);
                }
                else
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(itemIndex + 1);
                }
            }
            //Handle inputs from the mouse scroll wheel going down
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (itemIndex <= 0)
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(items.Length - 1);
                }
                else
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(itemIndex - 1);
                }
            }
        }
        //We are only allowed 2 weapons
        else
        {
            //Check for number keys
            if (Input.GetKeyDown((1).ToString()))
            {
                items[itemIndex].ResetItem(1);
                EquipItem(primaryWeapon);
            }
            if (Input.GetKeyDown((2).ToString()))
            {
                items[itemIndex].ResetItem(1);
                EquipItem(secondaryWeapon);
            }

            //Handle inputs from the mouse scroll wheel going up
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if(itemIndex == primaryWeapon)
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(secondaryWeapon);
                }
                else
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(primaryWeapon);
                }
            }
            //Handle inputs from the mouse scroll wheel going down
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (itemIndex == primaryWeapon)
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(secondaryWeapon);
                }
                else
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(primaryWeapon);
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Method is called at a fixed rate instead of being tied to framerate like update() to move the character model around the game
    /// </summary>
    private void FixedUpdate()
    {
        //Exit method if this player isn't mine
        if (!PV.IsMine)
            return;

        //Don't move if we are in a vehicle
        if (!inVehicle)
        {
            //Check to see a the game state is set to playing
            if ((int)playerManager.state == 2)
            {
                //rb.velocity = moveAmount;
                rb.MovePosition(rb.position + (transform.TransformDirection(moveAmount) * Time.fixedDeltaTime));
            }
            //Allow player to move through the air in a pause state until they are on the ground
            else if (((int)playerManager.state != 2) && !grounded)
            {
                //rb.velocity = moveAmount;
                rb.MovePosition(rb.position + (transform.TransformDirection(moveAmount) * Time.fixedDeltaTime));
            }
        }
    }

    #region Damage and Death
    /// <summary>
    /// Method calls when the local player hits a damagable enitity and this method tells that entity that they need to take damage through photon RPC.
    /// </summary>
    /// <param name="damage">Amount of damage that this player needs to take</param>
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage, boot.bootObject.localPV.ViewID);
    }

    /// <summary>
    /// RPC method for this player to take damage
    /// </summary>
    /// <param name="damage">Amount of damage that needs to be taken</param>
    /// <param name="shooter">The PhotonView ID of the player dealing the damage to this player</param>
    [PunRPC]
    void RPC_TakeDamage(float damage, int shooter)
    {
        //Exit method if PV ids don't match
        if (!PV.IsMine)
            return;

        //Var to seperate damage between shields and helath
        float remainingDamage = damage;

        //Reset the time that this player last took damage and start the regen coroutine
        lastHit = 0;
        StartCoroutine(tookDamage());

        //Check if the sheilds can handle the full damage
        if(currentShields < remainingDamage)
        {
            //They can't
            remainingDamage -= currentShields;
            currentShields = 0;

            //So deal damage to the health
            currentHealth -= remainingDamage;
        }
        else if(currentShields >= remainingDamage)
        {
            //They can and take the full damage
            currentShields -= remainingDamage;
        }

        //Trigger the die method if current health is 0 or below
        if (currentHealth <= 0)
        {
            playerManager.killedPlayer(shooter);
            Die();
        }
    }

    /// <summary>
    /// Reference the parent playerManager's die method to destroy this player controller
    /// </summary>
    void Die()
    {
        //Exit the vehicle before death
        if (inVehicle)
            ExitVehicle();
        playerManager.Die();
    }
    #endregion

    #region Custom Properties
    /// <summary>
    /// Method calls whenever properites on the player are updated and then syncs those changes across the network
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //Check to see if we are not the local player (not synced)
        //and check to see if this function matches to the player that we are calling this for
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            //sync weapons if above is true
            if (changedProps.ContainsKey("itemIndex"))
            {
                EquipItem((int)changedProps["itemIndex"]);
            }

            if (changedProps.ContainsKey("Team"))
            {
                changeAppearance((bool)changedProps["Team"]);
            }
        }
    }

    /// <summary>
    /// Method to change the material of the player controller
    /// </summary>
    /// <param name="t">The bool for the team that this player need to go too</param>
    public void changeAppearance(bool team)
    {
        //Blue team
        if (team)
        {
            Helmet.GetComponent<SkinnedMeshRenderer>().material = BlueHelmet;
            Body.GetComponent<SkinnedMeshRenderer>().material = BlueBody;
        }
        //Red team
        else
        {
            Helmet.GetComponent<SkinnedMeshRenderer>().material = RedHelmet;
            Body.GetComponent<SkinnedMeshRenderer>().material = RedBody;
        }
    }

    /// <summary>
    /// Method forces all players to update their custom properties whenever a new player joins in order to ensure that there is proper syncing
    /// </summary>
    /// <param name="newPlayer">The new player who joined</param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
    }
    #endregion

    /// <summary>
    /// Method which updates the mouse sensitivity whenever the settings are updated
    /// </summary>
    private void updateMouse()
    {
        mouseSenstivity = boot.bootObject.currentSettings.mouseSensitvity;
    }

    #region Sync
    /// <summary>
    /// Method to attempt to sync this player view to everyone else
    /// </summary>
    public void TrySync()
    {
        //Exit if this PhotonView is not mine
        if (!photonView.IsMine) return;

        //Send data if the gameMode is TDM
        if (GameSettings.GameMode == GameMode.TDM)
        {
            PV.RPC("SyncTeam", RpcTarget.All, GameSettings.IsBlueTeam);
        }
    }

    /// <summary>
    /// Method sets team based on sync method
    /// </summary>
    /// <param name="p_blueTeam">Which team is passed</param>
    [PunRPC]
    private void SyncTeam(bool p_blueTeam)
    {
        blueTeam = p_blueTeam;
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine to monitor when this character last took damage
    /// </summary>
    /// <returns></returns>
    private IEnumerator tookDamage()
    {
        //Wait 1 second
        yield return new WaitForSeconds(1f);

        //Start the rechargeShields loop
        if(lastHit == shieldRechargeWait)
        {
            StartCoroutine(rechargeShields());
        }

        //Start the rechargeHealth loop
        if (lastHit == healthRechargeWait)
        {
            StartCoroutine(rechargeHealth());
        }

        //Make sure that last hit is going to be greater than the recharge rates but not increase infinitely
        if (lastHit < (healthRechargeWait + shieldRechargeWait))
        {
            lastHit++;
            StartCoroutine(tookDamage());
        }
    }

    /// <summary>
    /// Method to start recharging Health
    /// </summary>
    /// <returns></returns>
    private IEnumerator rechargeHealth()
    {
        //Wait a tenth of a second
        yield return new WaitForSeconds(0.1f);

        //Add health if current health is below max health
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRechargePerSecond / 10f;
        }

        //Make health a constant or continue to regenerate health
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            StartCoroutine(rechargeHealth());
        }

    }

    /// <summary>
    /// Method to start recharging Shields
    /// </summary>
    /// <returns></returns>
    private IEnumerator rechargeShields()
    {
        //Method to wait a tenth of a second
        yield return new WaitForSeconds(0.1f);

        //Add shields if the current shields are below max shields
        if (currentShields < maxShields)
        {
            currentShields += shieldRechargePerSecond / 10f;
        }

        //Make shields a constant or contine to regenerate shields
        if (currentShields >= maxShields)
        {
            currentShields = maxShields;
        }
        else
        {
            StartCoroutine(rechargeShields());
        }
    }
    #endregion

    #region Vehicles
    /// <summary>
    /// Method checks for input and enters or exits a vehicle if applciable
    /// </summary>
    private void VehicleCheck()
    {
        //If the e key was pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            //If we are not in a vehicle
            if (!inVehicle)
            {
                //Initial raycast setup
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = cam.transform.position;

                //Detect if the ray hit an object
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    //Set the range that a player can get into a vehicle
                    if(hit.distance <= 5)
                    {
                        //Check if the object is a child of a car and is a rider object
                        if (hit.collider.gameObject.GetComponentInParent<Car>() && hit.collider.gameObject.GetComponent<Rider>())
                        {
                            //Check if the seat is occupied and only allow player to enter it if it is empty
                            if (!hit.collider.gameObject.GetComponent<Rider>().occupied)
                            {
                                //Check if the seat is the driver seat
                                if (hit.collider.gameObject.GetComponent<Rider>().driver)
                                {
                                    //Send new driver request
                                    hit.collider.gameObject.GetComponentInParent<Car>().NewDriverRequest(boot.bootObject.localPV);
                                }

                                //enter seat
                                EnterVehicle(hit.collider.gameObject.GetComponent<Rider>());
                            }
                        }
                    }
                }
            }
            else
            {
                ExitVehicle();
            }
        }
    }

    /// <summary>
    /// Method for player to call when they enter a seat in a vehicle
    /// </summary>
    /// <param name="seat">The seat in the vehicle that they are entering</param>
    private void EnterVehicle(Rider seat)
    {
        //Record that we are in a vehicle
        inVehicle = true;

        //Record the seat we are in
        currentSeat = seat;

        //Stop walk animations
        Animation.SetFloat("InputX", moveAmount.x);
        Animation.SetFloat("InputZ", moveAmount.z);

        //Set riding animation
        Animation.SetLayerWeight(2, 1);

        //Check if we are driver
        if (currentSeat.name.Equals("Driver"))
        {
            //Disable and use the vehicle camera
            cam.enabled = false;
            cam.transform.GetChild(0).gameObject.SetActive(false);
        }

        //Reset look rotation
        cameraHolder.transform.localEulerAngles = new Vector3(0, 0, 0);

        //Tell everyone which seat we are in and how to sync us to that seat
        currentSeat.parentCar.CarPV.RPC("NewPassenger", RpcTarget.All, currentSeat.name, currentSeat.parentCar.CarPV.ViewID, this.PV.ViewID);

        //Remove the player rigidbody as they will use the vehicle's rigidbody
        Destroy(rb);
    }

    /// <summary>
    /// Method for player to call when they exit a seat in a vehicle
    /// </summary>
    private void ExitVehicle()
    {
        //Call rpc to tell everyone how to sync our postion and that we are no longer in a vehicle
        currentSeat.parentCar.CarPV.RPC("ExitVehicle", RpcTarget.All, currentSeat.name, currentSeat.parentCar.CarPV.ViewID, this.PV.ViewID);

        //Remove our stored vehicle settings
        currentSeat = null;
        inVehicle = false;

        //Create new rigidbody for the player
        rb = this.gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        //Disable the riding animations
        Animation.SetLayerWeight(2, 0);

        //Enable the player's camera
        cam.enabled = true;
        cam.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion
}
