﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/// <summary>
/// Class to create and manage playerlistitems
/// </summary>
public class PlayerListItem : MonoBehaviourPunCallbacks
{
    //unity reference var
    [SerializeField] TMP_Text text;
    //empty reference var
    Player player;

    /// <summary>
    /// Method which creates new player instances
    /// </summary>
    /// <param name="parPlayer"></param>
    public void SetUp(Player parPlayer)
    {
        player = parPlayer;
        text.text = parPlayer.NickName;
    }

    /// <summary>
    /// Method which triggers to destroy a playerlistitem for the room when a player leaves a room
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Method which triggers to destory a playerlistitem for a local user when the user leaves a room
    /// </summary>
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
