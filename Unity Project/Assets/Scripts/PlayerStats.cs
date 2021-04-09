using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStats
{
    public string username;
    public int actor;
    public short kills;
    public short deaths;

    public PlayerStats(string user, int a, short k, short d)
    {
        this.username = user;
        this.actor = a;
        this.kills = k;
        this.deaths = d;
    }
}
