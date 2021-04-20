using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enum for Gamemodes
/// </summary>
public enum GameMode
{
    FFA = 0,
    TDM = 1
}

/// <summary>
/// Static methods for gamemode and team
/// </summary>
public class GameSettings : MonoBehaviour
{
    public static GameMode GameMode = GameMode.FFA;
    public static bool IsBlueTeam = false;
}
