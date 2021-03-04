using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script class to manage menu collections
public class MenuManager : MonoBehaviour
{
    //create a global instance of this menu to be referenced by everything
    public static MenuManager Instance;

    //Array of menus that this script can access and assigned through unity
    [SerializeField] Menu[] menus;

    //When this is first referenced
    private void Awake()
    {
        //Set a global instance of menuManager to this script
        Instance = this;
    }

    //Method to open menu based on a string
    public void OpenMenu(string menuName)
    {
        //iterate through all of the menus in the menu array
        for(int i = 0; i <menus.Length; i++)
        {
            //if they match the passed menu
            if(menus[i].menuName == menuName)
            {
                //open that menu
                menus[i].Open();
            }
            //else if they are open
            else if (menus[i].open)
            {
                //close that menu
                CloseMenu(menus[i]);
            }
        }
    }

    //method to open menus based on the menu object
    public void OpenMenu(Menu menu)
    {
        //iterate through all of the menus in the menu array
        for(int i = 0; i < menus.Length; i++)
        {
            //if they are open
            if (menus[i].open)
            {
                //close those menus
                CloseMenu(menus[i]);
            }
        }
        //open the passed menu object
        menu.Open();
    }

    //method to close menu
    public void CloseMenu(Menu menu)
    {
        //call the specific menu passed and close it
        menu.Close();
    }
}
