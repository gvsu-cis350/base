using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script class to manage menu collections
/// </summary>
public class MenuManager : MonoBehaviour
{
    #region Vars
    //create a global instance of this menu to be referenced by everything
    public static MenuManager Instance;

    //Array of menus that this script can access and assigned through unity
    [SerializeField] Menu[] menus;
    #endregion

    /// <summary>
    /// Set a globabl instance of menuManager when this script first referenced
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Method to open menu based on a string. Opens menus matching the string name and closes any that are open and don't match
    /// </summary>
    /// <param name="menuName">Name of menu to be opend</param>
    public void OpenMenu(string menuName)
    {
        //iterate though all menus
        for(int i = 0; i <menus.Length; i++)
        {
            //Open menu if name matches
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            //close menu if it is open and doesn't match
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    /// <summary>
    /// Method to open menu based on a passed object. Closes all menus and then opens the matching menu object
    /// </summary>
    /// <param name="menu">Open a passed menu</param>
    public void OpenMenu(Menu menu)
    {
        //Close all menus
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        //Open the specifically passed menu
        menu.Open();
    }

    /// <summary>
    /// Method calls the menu close method on a passed object parameter
    /// </summary>
    /// <param name="menu">Close passed menu</param>
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    /// <summary>
    /// Method can be called to close all menus in the menuManager
    /// </summary>
    public void CloseAllMenus()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }
}
