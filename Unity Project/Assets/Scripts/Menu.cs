using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//class adds the 'menu' attribute to 
public class Menu : MonoBehaviour
{
    //string to store the name of a menu for references in scripts
    public string menuName;

    //boolean to determine if the menu is active
    public bool open;

    //class which opens menu on call
    public void Open()
    {
        //flip open bool on
        open = true;
        //set the menu to active
        gameObject.SetActive(true);
    }

    //class which closes menu on call
    public void Close()
    {
        //flip open bool off
        open = false;
        //set the menu to non-active
        gameObject.SetActive(false);
    }
}

