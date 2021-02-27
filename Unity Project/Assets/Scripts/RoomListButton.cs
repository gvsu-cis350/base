using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListButton : MonoBehaviour
{
    [SerializeField] TMP_Text text;

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
