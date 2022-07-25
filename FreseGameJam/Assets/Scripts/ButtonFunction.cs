using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    #region variables:
    // ask Felix!

    [Header("Button & Key Settings:")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject[] arrayOfAllOtherMenus;

    // local variables:
    float[] _lastTimer = new float[3];
    float _defaultVolumeSettings;
    float _defaultMouseSensitivitySettings;
    int _currentLevel;
    #endregion

    private void Start()
    {
        // save level:
        _currentLevel = SceneManager.GetActiveScene().buildIndex;

        // add time of last run:
        _lastTimer[(_currentLevel - 2)] = PlayerPrefs.GetFloat("lastTimer" + (_currentLevel - 2));

        // Get the default settings for the game from the Game Manager:
        //_defaultVolumeSettings = GameObject.Find("Game Manager").GetComponent<GameManager>().defaultVolumeSettings;
        //_defaultMouseSensitivitySettings = GameObject.Find("Game Manager").GetComponent<GameManager>().defaultMouseSensitivitySettings;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
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

            // make sure all other UI elements get turnt off when pressing escape.
            foreach(GameObject _uiElement in arrayOfAllOtherMenus)
            {
                _uiElement.SetActive(false);
            }
        }
    }

    public void BackToMain()
    {
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

        // restore default settings now:
        PlayerPrefs.SetFloat("volumeSettings", _defaultVolumeSettings);
        PlayerPrefs.SetFloat("mouseSensitivitySettings", _defaultMouseSensitivitySettings);
    }
}
