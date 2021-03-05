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

    //public 
    public RoomInfo info;
    public void SetUp(RoomInfo parInfo)
    {
        info = parInfo;
        text.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
