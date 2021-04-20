using System.Collections;
using System.Collections.Generic;
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
    /// Method which is called when class is contructed, and deletes references to other user's controllers
    /// </summary>
    private void Start()
    {

        if (PV.IsMine)
        {
            if (GameSettings.GameMode == GameMode.TDM)
            {
                PV.RPC("SyncTeam", RpcTarget.All, GameSettings.IsBlueTeam);
                /*
                if (GameSettings.IsAwayTeam)
                {
                    ui_team.text = "red team";
                    ui_team.color = Color.red;
                }
                else
                {
                    ui_team.text = "blue team";
                    ui_team.color = Color.blue;
                }
                */
            }

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("AllWeapons"))
            {
                allWeapons = (bool)PhotonNetwork.CurrentRoom.CustomProperties["AllWeapons"];

            }

            if (allWeapons)
            {
                primaryWeapon = 0;
            }
            else
            { 
                primaryWeapon = playerManager.primaryWeaponPM;
                secondaryWeapon = playerManager.secondaryWeaponPM;
            }
            
            //subscribe the mouse senstivity method to settings update event
            GameEvents.current.onSettingsUpdate += updateMouse;
            //Equip the first item available
            EquipItem(primaryWeapon);
            //Remove the material entry in the hashmap if there is one
            if (customProperties.ContainsKey("Team"))
                customProperties.Remove("Team");
            if (GameSettings.GameMode == GameMode.TDM)
            {
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
            Helmet.layer = 12;
            playerManager.ammoCounter.text = items[itemIndex].returnInfo().ToString();


        }
        else
        {
            //remove components that will conflict with the local copies of those componenets
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
    void Update()
    {
        //exit method if we are not on the local user's Photon View id
        if (!PV.IsMine)
            return;

        //check to see if there is a the current game state is set to playing
        if ((int)playerManager.state == 2)
        {
            //run basic movement methods and weapon switching methods depending on vehicle status
            

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
            

            weaponSwitch();

            //check to see if the user fires their gun
            if (Input.GetMouseButtonDown(0))
            {
                items[itemIndex].Use();
            }

            playerManager.ammoCounter.text = "" + items[itemIndex].returnInfo()["currentAmmo"].ToString() + "/" + items[itemIndex].returnInfo()["maxAmmo"].ToString();

            if (Input.GetKeyDown(KeyCode.R))
            {
                items[itemIndex].RefreshItem();
            }
        }
        else
        {
            //make sure that the character only animates the idle animation while paused
            Animation.SetFloat("InputX", 0);
            Animation.SetFloat("InputZ", 0);
        }


        if (currentShields <= 0)
        {
            playerManager.shields.gameObject.SetActive(false);
            playerManager.depletedShields.gameObject.SetActive(true);
        }
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

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!inVehicle)
            {
                //Initial raycast setup
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = cam.transform.position;

                //detect if the ray hit an object
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.GetComponentInParent<Car>())
                    {
                        //Check if the seat is occupied and only allow player to enter it if it is empty
                        if(!hit.collider.gameObject.GetComponent<Rider>().occupied)
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
            else
            {
                ExitVehicle();
            }
        }
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
        if (PV.IsMine)
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
            Transform[] fschildren = items[itemIndex].gameObject.GetComponentsInChildren<Transform>();
            //Set them to only be rendered by the fixed FOV camera
            foreach (Transform go in fschildren)
            {
                go.gameObject.layer = 10;
            }

        }

        //Set new hand locations based on stored hand locations
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

        //Check if all weapons are equiped or only 2
        if (allWeapons)
        {
            //handle inputs from the number keys
            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(i);
                    break;
                }
            }

            //handle inputs from the mouse scroll wheel going up
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
            //handle inputs from the mouse scroll wheel going down
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
        else
        {
            //Check for number keys
            if(Input.GetKeyDown((1).ToString()))
                {
                    items[itemIndex].ResetItem(1);
                    EquipItem(primaryWeapon);
                }
            if (Input.GetKeyDown((2).ToString()))
            {
                items[itemIndex].ResetItem(1);
                EquipItem(secondaryWeapon);
            }

            //handle inputs from the mouse scroll wheel going up
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
            //handle inputs from the mouse scroll wheel goign down
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
        //exit method if PV ids don't match
        if (!PV.IsMine)
            return;

        if (!inVehicle)
        {
            //check to see a the game state is set to playing
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
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage, boot.bootObject.localPV.ViewID);
    }

    /// <summary>
    /// Method is established as a Remote Procedure Call where other users are told to take damage when they are hit
    /// </summary>
    /// <param name="damage"></param>
    [PunRPC]
    void RPC_TakeDamage(float damage, int shooter)
    {
        //exit method if PV ids don't match
        if (!PV.IsMine)
            return;

        float remainingDamage = damage;

        lastHit = 0;
        StartCoroutine(tookDamage());
        if(currentShields < remainingDamage)
        {
            remainingDamage -= currentShields;
            currentShields = 0;

            currentHealth -= remainingDamage;
        }
        else if(currentShields >= remainingDamage)
        {
            currentShields -= remainingDamage;
        }

        //trigger the die method if current health is not above 1
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
    public void changeAppearance(bool t)
    {
        if (t)
        {
            Helmet.GetComponent<SkinnedMeshRenderer>().material = BlueHelmet;
            Body.GetComponent<SkinnedMeshRenderer>().material = BlueBody;
        }
        else
        {
            Helmet.GetComponent<SkinnedMeshRenderer>().material = RedHelmet;
            Body.GetComponent<SkinnedMeshRenderer>().material = RedBody;
        }
    }

    /// <summary>
    /// Method forces all players to update their custom properties whenever a new player joins in order to ensure that there is proper syncing
    /// </summary>
    /// <param name="newPlayer"></param>
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
    public void TrySync()
    {
        if (!photonView.IsMine) return;

        //photonView.RPC("SyncProfile", RpcTarget.All, Launcher.myProfile.username, Launcher.myProfile.level, Launcher.myProfile.xp);

        if (GameSettings.GameMode == GameMode.TDM)
        {
            PV.RPC("SyncTeam", RpcTarget.All, GameSettings.IsBlueTeam);
        }
    }

    [PunRPC]
    private void SyncTeam(bool p_blueTeam)
    {
        blueTeam = p_blueTeam;

        if (blueTeam)
        {
            // ColorTeamIndicators(Color.red);
        }
        else
        {
            //   ColorTeamIndicators(Color.blue);
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator tookDamage()
    {
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
    private void EnterVehicle(Rider seat)
    {
        inVehicle = true;
        currentSeat = seat;
        Animation.SetFloat("InputX", moveAmount.x);
        Animation.SetFloat("InputZ", moveAmount.z);
        Animation.SetLayerWeight(2, 1);
        if (currentSeat.name.Equals("Driver"))
        {
            cam.enabled = false;
            cam.transform.GetChild(0).gameObject.SetActive(false);
        }
        cameraHolder.transform.localEulerAngles = new Vector3(0, 0, 0);

        currentSeat.parentCar.CarPV.RPC("NewPassenger", RpcTarget.All, currentSeat.name, currentSeat.parentCar.CarPV.ViewID, this.PV.ViewID);

        Destroy(rb);
    }

    private void ExitVehicle()
    {
        currentSeat.parentCar.CarPV.RPC("ExitVehicle", RpcTarget.All, currentSeat.name, currentSeat.parentCar.CarPV.ViewID, this.PV.ViewID);

        currentSeat = null;
        rb = this.gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        inVehicle = false;
        Animation.SetLayerWeight(2, 0);
        cam.enabled = true;
        cam.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion
}
