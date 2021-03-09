using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boot : MonoBehaviour
{
    public bool firstConnection = false;

    //public static boot Instance;

    private void Awake()
    {
       // Instance = this;
        DontDestroyOnLoad(transform.gameObject);
        //Debug.Log("Boot Scene Worked");
        SceneManager.LoadScene(1);
    }
}
