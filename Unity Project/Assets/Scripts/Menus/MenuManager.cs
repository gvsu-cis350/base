using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script class to manage menu collections
/// </summary>
public class MenuManager : MonoBehaviour
{
    //create a global instance of this menu to be referenced by everything
    public static MenuManager Instance;

    //Array of menus that this script can access and assigned through unity
    [SerializeField] Menu[] menus;

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
    /// <param name="menuName"></param>
    public void OpenMenu(string menuName)
    {
        for(int i = 0; i <menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    /// <summary>
    /// Method to open menu based on a passed object. Closes all menus and then opens the matching menu object
    /// </summary>
    /// <param name="menu"></param>
    public void OpenMenu(Menu menu)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    /// <summary>
    /// Method calls the menu close method on a passed object parameter
    /// </summary>
    /// <param name="menu"></param>
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
