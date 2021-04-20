using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

/// <summary>
/// Enum for drive type
/// </summary>
[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}

/// <summary>
/// Class is for player controlled cars and allows the player to move the car around
/// </summary>
public class Car : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    #region Vars
    #region Player Vars
    Rigidbody rb;
    public PhotonView CarPV;
    private PhotonView driverPV;
    private bool hasDriver = false;
    private Dictionary<string, Rider> riders = new Dictionary<string, Rider>();
    private Rider[] passengers;
    private Camera cam;
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
    /// Method call which assigns components to reference vars in script when script is referenced
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CarPV = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);
        cam = GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Method unsubscribes this object to the photonNetwork as this object is a room Object
    /// </summary>
    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// Method establishes passenger and vehicle variables
    /// </summary>
    private void Start()
    {
        //Remove driver and player ownership of vehicle
        driverPV = null;
        CarPV.TransferOwnership(null);
        hasDriver = false;

        //Find wheels
        m_Wheels = GetComponentsInChildren<WheelCollider>();

        //Collect seats
        passengers = GetComponentsInChildren<Rider>();
        foreach(Rider r in passengers)
        {
            riders.Add(r.name, r);
        }
    }

    /// <summary>
    /// Update method called to move car around
    /// </summary>
    void Update()
    {
        //exit method if we do not own the car
        if (!CarPV.IsMine)
            return;

        //Protection for master client owning this car upon instantiation
        if (driverPV == null)
        {
            //Turns the camera off for the last driver as they still own this vehicle, but their PV is not logged as the current driver.
            cam.enabled = false;
            return;
        }

        //Enable camera as we are have passed PhotonView checks
        cam.enabled = true;

        //Set wheels up
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
    }

    #region Ownership Transfer
    /// <summary>
    /// Method to gets called when someone tries to get ownership of the vehicle
    /// </summary>
    /// <param name="targetView">The PV of the object which is receiving the request</param>
    /// <param name="requestingPlayer">The player sending the request</param>
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        //Exit this method if the requested car is not this car
        if (targetView != CarPV)
            return;

        //Only give ownership if there isn't a current driver
        if (!hasDriver)
        {
            CarPV.TransferOwnership(requestingPlayer);
            hasDriver = true;
        }
    }

    /// <summary>
    /// Method to handle the specifics of what happens when ownership is transfered
    /// </summary>
    /// <param name="targetView"></param>
    /// <param name="requestingPlayer"></param>
    public void OnOwnershipTransfered(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != CarPV)
            return;
    }

    /// <summary>
    /// Method to handle ownership Transfer failures
    /// </summary>
    /// <param name="targetView"></param>
    /// <param name="requestingPlayer"></param>
    public void OnOwnershipTransferFailed(PhotonView targetView, Player requestingPlayer)
    {
        return;
    }
    #endregion

    /// <summary>
    /// Method to gets called locally when someone enters the driver slot to update the local driverPV
    /// </summary>
    /// <param name="newDriver"></param>
    public void NewDriverRequest(PhotonView newDriver)
    {
        driverPV = newDriver;
        base.photonView.RequestOwnership();
    }

    #region Passenger Methods
    /// <summary>
    /// Method attaches a player to a seat
    /// </summary>
    /// <param name="player">The PV ID of the player that needs to change transform</param>
    /// <param name="seat">The seat that the player has selected</param>
    /// <param name="adding">Wheter they are getting out (false) or entering (true)</param>
    private void ObjectAttachToggle(int player, Rider seat, bool adding)
    {
        //Find the gameObject of the player with the passed ID
        GameObject holder = PhotonView.Find(player).gameObject;

        //Check if they are entering vehicle
        if (adding)
        {
            //Parent them to the seat's playerPosition
            holder.transform.SetParent(seat.playerPostion.transform);
            holder.transform.localPosition = new Vector3(0, 0, 0);
            holder.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        //Exiting vehicle
        else
        {
            //Set parent to the seat's exitPostion
            holder.transform.SetParent(seat.exitPosition.transform);
            holder.transform.localPosition = new Vector3(0, 0, 0);
            holder.transform.localRotation = new Quaternion(0, 0, 0, 0);

            //Unparent the player
            holder.transform.SetParent(null);
        }
    }

    /// <summary>
    /// RPC to track a new player to the vehicle
    /// </summary>
    /// <param name="newSeat">The seat they have selected</param>
    /// <param name="carID">The ID of the parent Car</param>
    /// <param name="player">Their personal PV ID</param>
    [PunRPC]
    public void NewPassenger(string newSeat, int carID, int player)
    {
        //Exit the method if this car doesn't match the passed car ID
        if (carID != CarPV.ViewID)
            return;

        //Attach the player to their seat
        ObjectAttachToggle(player, riders[newSeat], true);

        //Set the seat value to occupied
        riders[newSeat].occupied = true;
    }

    /// <summary>
    /// RPC to track a player leaving a vehicle
    /// </summary>
    /// <param name="newSeat">The Player's seat</param>
    /// <param name="carID">The PV ID of the selected vehicle</param>
    /// <param name="player">The PV ID of the player</param>
    [PunRPC]
    public void ExitVehicle (string newSeat, int carID, int player)
    {
        //Exit the method if the car IDs don't match
        if (carID != CarPV.ViewID)
            return;

        //Eject the player from their seat
        ObjectAttachToggle(player, riders[newSeat], false);

        //Set the seat to being unoccupied
        riders[newSeat].occupied = false;

        //Check if the seat was the driver's seat
        if (newSeat.Equals("Driver"))
        {
            //Reset the driver's seat ids if it was
            driverPV = null;
            CarPV.TransferOwnership(null);
            hasDriver = false;
        }
    }
    #endregion
}
