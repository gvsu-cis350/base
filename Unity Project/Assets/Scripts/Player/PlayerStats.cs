using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class to store player info in a match which can be easily transfered to other players
/// </summary>
public class PlayerStats
{
    //Vars to store for users
    public string username;
    public int actor;
    public short kills;
    public short deaths;
    public bool blueTeam;

    /// <summary>
    /// Contructor to create new playerInfo
    /// </summary>
    /// <param name="user"></param>
    /// <param name="a"></param>
    /// <param name="k"></param>
    /// <param name="d"></param>
    /// <param name="t"></param>
    public PlayerStats(string user, int a, short k, short d, bool t)
    {
        this.username = user;
        this.actor = a;
        this.kills = k;
        this.deaths = d;
        this.blueTeam = t;
    }
}
