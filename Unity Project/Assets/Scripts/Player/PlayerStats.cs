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
    public bool blueTeam;

    public PlayerStats(string user, int a, short k, short d, bool t)
    {
        this.username = user;
        this.actor = a;
        this.kills = k;
        this.deaths = d;
        this.blueTeam = t;
    }
}
