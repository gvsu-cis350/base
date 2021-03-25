using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Class manages RoomListButton behaviours
/// </summary>
public class RoomListButton : MonoBehaviour
{
    //unity reference var
    [SerializeField] TMP_Text text;

    //public var to reference photon's room data
    public RoomInfo info;

    /// <summary>
    /// Method that creates a room button based on passed room info
    /// </summary>
    /// <param name="parInfo"></param>
    public void SetUp(RoomInfo parInfo)
    {
        info = parInfo;
        text.text = info.Name;
    }

    /// <summary>
    /// Method that causes users to join a specific room if they click on a Room Button
    /// </summary>
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
