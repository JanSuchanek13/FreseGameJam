using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    float[] lastTimer = new float[3];
    int currentLevel;
    int pingPongAlpha = 1;
    public CanvasGroup UI;


    private void Start()
    {
        //safe level
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("pressed ESC");
            UI.alpha = pingPongAlpha;
            pingPongAlpha = pingPongAlpha * -1;
        }
    }

    public void SafeTime()
    {
        //safe Time
        lastTimer[(currentLevel - 2)] = PlayerPrefs.GetFloat("lastTimer" + (currentLevel - 2));
        PlayerPrefs.SetFloat("lastTimer" + (currentLevel - 2), Time.timeSinceLevelLoad + PlayerPrefs.GetFloat("lastTimer" + (currentLevel - 2)));
        Debug.Log("Safe Time");
    }
}
