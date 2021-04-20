using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class adds the 'menu' attribute to assigned objects
/// </summary>
public class Menu : MonoBehaviour
{
    #region Vars
    //string to store the name of a menu for references in scripts
    public string menuName;

    //boolean to determine if the menu is active
    public bool open;
    #endregion

    /// <summary>
    /// Method to set a menu object to active
    /// </summary>
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Method to set a menu object to inactive
    /// </summary>
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}

