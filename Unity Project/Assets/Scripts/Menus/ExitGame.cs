using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to close the game when called
/// </summary>
public class ExitGame : MonoBehaviour
{
    /// <summary>
    /// Single method which closes the game
    /// </summary>
    public void quitGame()
    {
        Application.Quit();
    }
}
