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

        //add time of last run
        lastTimer[(currentLevel - 2)] = PlayerPrefs.GetFloat("lastTimer" + (currentLevel - 2));
    }

    private void Update()
    {
        //safe time per frame
        //PlayerPrefs.SetFloat("lastTimer" + (currentLevel - 2), Time.timeSinceLevelLoad + lastTimer[(currentLevel - 2)]);

        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("pressed ESC");
            UI.alpha = pingPongAlpha;
            pingPongAlpha = pingPongAlpha * -1;
        }
    }

    
}
