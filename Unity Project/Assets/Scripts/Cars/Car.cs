using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;


[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}


/// <summary>
/// Class is the controller for player controller and allows the player to move around
/// </summary>
public class Car : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks // , IDamageable
{
    #region Vars
    #region Inspector Reference Vars
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSenstivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    #endregion


    #region Location and Roatation Vars
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    #endregion

    #region Player Vars
    Rigidbody rb;
    public PhotonView CarPV;
    private bool hasDriver = false;
    private Dictionary<string, Rider> riders = new Dictionary<string, Rider>();
    private Rider[] passengers;
    #endregion

    #region Health Vars
    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    #endregion

    #region Wheel Vars
    [Tooltip("Maximum steering angle of the wheels")]
    public float maxAngle = 30f;
    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 300f;
    [Tooltip("Maximum brake torque applied to the driving wheels")]
    public float brakeTorque = 30000f;
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;

    private WheelCollider[] m_Wheels;
    #endregion
    #endregion

    /// <summary>
    /// Method call which assigns objects to reference vars in script when script is referenced
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CarPV = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    /// <summary>
    /// Method which is called when class is contructed, and deletes references to other user's controllers
    /// </summary>
    private void Start()
    {
        ExitVehicle();
        m_Wheels = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];

            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }


        passengers = GetComponentsInChildren<Rider>();
        foreach(Rider r in passengers)
        {
            riders.Add(r.name, r);
        }
    }

    

    /// <summary>
    /// Update method called continously based on frame rate of user to handle local inputs
    /// </summary>
    void Update()
    {
        //exit method if we are not on the local user's Photon View id
        if (!CarPV.IsMine)
            return;

        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        float angle = maxAngle * Input.GetAxis("Horizontal");
        float torque = maxTorque * Input.GetAxis("Vertical");

        float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;

        foreach (WheelCollider wheel in m_Wheels)
        {
            // A simple car where front wheels steer while rear ones drive.
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = angle;

            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
            }

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            // Update visual wheels if any.
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "a0l" || wheel.name == "a1l" || wheel.name == "a2l")
                {
                    shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                    shapeTransform.position = p;
                }
                else
                {
                    shapeTransform.position = p;
                    shapeTransform.rotation = q;
                }
            }
        }

        //Move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitVehicle();
        }
    }

    #region Movement
    /// <summary>
    /// Method which takes keyboard inputs and stores that into a movement amount var
    /// </summary>
    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
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

    /// <summary>
    /// Method is called at a fixed rate instead of being tied to framerate like update() to move the character model around the game
    /// </summary>
    private void FixedUpdate()
    {
        //Debug.Log(hasDriver);
        //exit method if PV ids don't match
        if (!CarPV.IsMine)
            return;

        //rb.MovePosition(rb.position + (transform.TransformDirection(moveAmount) * Time.fixedDeltaTime));

    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != CarPV)
            return;
        if (!hasDriver)
        {
            CarPV.TransferOwnership(requestingPlayer);
            hasDriver = true;
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != CarPV)
            return;
    }

    public void NewDriverRequest()
    {
        base.photonView.RequestOwnership();
    }
    public void ExitVehicle()
    {
        CarPV.TransferOwnership(0);
        hasDriver = false;
    }


    [PunRPC]
    public void newPassenger(string newSeat, int carID)
    {
        if (carID != CarPV.ViewID)
            return;

        riders[newSeat].occupied = true;
    }

    [PunRPC]
    public void passengerExit(string newSeat, int carID)
    {
        if (carID != CarPV.ViewID)
            return;

        riders[newSeat].occupied = false;
    }
}
