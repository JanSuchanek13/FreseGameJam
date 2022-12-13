using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{
    public void PlayGameLevel()
    {
        SceneManager.LoadScene(1);
    }


    public void Quit()
    {
        Debug.Log ("Winners never Quit");
        Application.Quit();
    }
}
