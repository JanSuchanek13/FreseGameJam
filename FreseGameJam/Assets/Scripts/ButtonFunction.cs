using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    #region variables:
    float[] lastTimer = new float[3];
    int currentLevel;
    //int pingPongAlpha = 1;
    public CanvasGroup UI;

    [SerializeField] GameObject pauseMenu;
    #endregion

    private void Start()
    {
        // save level:
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        // add time of last run:
        lastTimer[(currentLevel - 2)] = PlayerPrefs.GetFloat("lastTimer" + (currentLevel - 2));
    }

    private void Update()
    {
        //safe time per frame
        //PlayerPrefs.SetFloat("lastTimer" + (currentLevel - 2), Time.timeSinceLevelLoad + lastTimer[(currentLevel - 2)]);

        if (Input.GetButtonDown("Cancel"))
        {
            //Debug.Log("pressed ESC");
            //UI.alpha = pingPongAlpha;
            //pingPongAlpha = pingPongAlpha * -1;
            Pause();
        }
    }
    public void Pause()
    {
        if (pauseMenu.activeInHierarchy != true)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void BackToMain()
    {
        Debug.Log("Back to Main pressed!");
        Time.timeScale = 1; // otherwise "continue"-button will start the game paused. Unless we want that?
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Debug.Log("application would now be closed!");
        Time.timeScale = 1; // otherwise "continue"-button will start the game paused. Unless we want that?
        Application.Quit();
    }

    public void SaveNewSettings()
    {
        Debug.Log("you saved your new settings!");
        // save stuff now
    }
    public void SettingsToDefault()
    {
        Debug.Log("you restored the default settings!");
        // restore default settings now
    }
}
