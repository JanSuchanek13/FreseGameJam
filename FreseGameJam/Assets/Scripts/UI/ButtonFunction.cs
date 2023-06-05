using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;


public class ButtonFunction : MonoBehaviour
{
    #region variables:
    // ask Felix!

    [Header("Button & Key Settings:")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject[] arrayOfAllOtherMenus;
    [SerializeField] GameObject settingsButton;
    [Space(10)]
    [SerializeField] GameObject _retryHardcoreRun_UI;

    // local variables:
    float[] _lastTimer = new float[3];
    float _defaultVolumeSettings;
    float _defaultMouseSensitivitySettings;
    int _currentLevel;

    CloseQuarterCamera _closeQuarterCamera;

    [Header("REFERENCES")]
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    #endregion


    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Awake()
    {
        playerInput = new PlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

  

    private void Start()
    {
        // save level:
        _currentLevel = SceneManager.GetActiveScene().buildIndex;

        // add time of last run:
        _lastTimer[0] = PlayerPrefs.GetFloat("lastTimer" + 0.0f); //(_currentLevel - 2) & (_currentLevel - 2) at end

        // Get the default settings for the game from the Game Manager:
        _defaultVolumeSettings = GameObject.Find("GameManager").GetComponent<GameManager>().defaultVolumeSettings;
        _defaultMouseSensitivitySettings = GameObject.Find("GameManager").GetComponent<GameManager>().defaultMouseSensitivitySettings;

        // Get CloseQuaterCamera-script reference:
        _closeQuarterCamera = GameObject.Find("Third Person Player_GameLevel_1").GetComponent<CloseQuarterCamera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || playerInput.CharacterControls.Option.triggered)
        {
            Pause();
        }

        if (playerInput.CharacterControlsController.Retry.triggered || playerInput.CharacterControlsKeyboard.Retry.triggered)
        {
            if (PlayerPrefs.GetInt("HardcoreMode", 0) != 0)
            {
                if (!_retryHardcoreRun_UI.activeInHierarchy)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    // Force the mouse to be in the max corner of the screen
                    Vector2 warpPosition = Screen.safeArea.max;
                    Mouse.current.WarpCursorPosition(warpPosition);
                    InputState.Change(Mouse.current.position, warpPosition);
                    _retryHardcoreRun_UI.SetActive(true);
                    //Time.timeScale = 0;

                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(settingsButton);

                    // disable camera movement in pause UI
                    Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                }else
                {
                    Retry();
                }
            }
        }

        if (playerInput.CharacterControls.CamToggle.triggered)
        {
            ToggleCloseQuaterCamera();
        }

        // JAN: uncomment this after allocating and mapping the SkipCutscene command in the playerInput!
         
        if (playerInput.CharacterControlsController.SkipCutscene.triggered || playerInput.CharacterControlsKeyboard.SkipCutscene.triggered)
        {
            SkipCutscene();
        }
    }
    public void Pause()
    {
        if(_retryHardcoreRun_UI.activeInHierarchy != true) // this allows to close the retry-hardcore UI when applicable by pressing ESC
        {
            if (pauseMenu.activeInHierarchy != true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Force the mouse to be in the max corner of the screen
                Vector2 warpPosition = Screen.safeArea.max;
                Mouse.current.WarpCursorPosition(warpPosition);
                InputState.Change(Mouse.current.position, warpPosition);
                pauseMenu.SetActive(true);
                Time.timeScale = 0;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(settingsButton);

                // disable camera movement in pause UI
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;

                // enable camera movement in pause UI
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;

                // make sure all other UI elements get turnt off when pressing escape.
                foreach (GameObject _uiElement in arrayOfAllOtherMenus)
                {
                    _uiElement.SetActive(false);
                }
            }
        }else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _retryHardcoreRun_UI.SetActive(false);
            //Time.timeScale = 1;

            // enable camera movement in pause UI
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;

            // make sure all other UI elements get turnt off when pressing escape.
            foreach (GameObject _uiElement in arrayOfAllOtherMenus)
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

    public void ToggleCloseQuaterCamera()
    {
        if (_closeQuarterCamera.closeQuarterCameraIsActive != true)
        {
            _closeQuarterCamera.ZoomIn();
            return;
        }else
        {
            _closeQuarterCamera.ZoomOut();
            return;
        }
    }

    public void SkipCutscene()
    {
        _closeQuarterCamera.gameObject.GetComponent<FocusPlayerViewOnObject>().SkipCutscene();
    }

    public void RetryRoundTrigger()
    {
        //confirm intention

    }

    public void Retry()
    {
        // reset level:
        Debug.Log("this was called");
        PlayerPrefs.SetInt("FastReset", 1);
        SceneManager.LoadScene(0);



        // restart level:
        //PlayerPrefs.SetInt("HardcoreMode", 1);
        //GetComponent<Level_Manager>().LoadLevel(1);
    }
}
