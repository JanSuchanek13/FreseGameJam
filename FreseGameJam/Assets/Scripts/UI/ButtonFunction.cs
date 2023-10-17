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
    [SerializeField] GameObject permissionButton;
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
    private InputHandler inputHandler;
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
        // Get Input Handler reference:
        inputHandler = GameObject.Find("Third Person Player_GameLevel_1").GetComponent<InputHandler>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || playerInput.CharacterControls.Option.triggered)
        {
            Pause();
        }

        if (playerInput.CharacterControls.ResetCam.triggered)
        {
            FindObjectOfType<HealthSystem>().RefocusCamera();
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
         
        if (playerInput.CharacterControlsController.SkipCutscene.triggered || playerInput.CharacterControlsKeyboard.SkipCutscene.triggered)
        {
            SkipCutscene();
        }
    }

    /// <summary>
    /// By Felix: Added this to allow calling the "Pause"-function but turning the pause-UI on/off if need be.
    /// For example when achieving a highscore and wanting to collect the players information this gets called
    /// (from the HighscoreCompiler-class)
    /// </summary>
    public void TogglePauseScreen()
    {
        if (pauseMenu.activeInHierarchy != true)
        {
            pauseMenu.SetActive(true);
        }else
        {
            pauseMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(permissionButton);
        }
    }

    /// <summary>
    /// If player leaves the game application this should automatically call the pause function.
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        Pause();
    }

    public void Pause()
    {
        if (_retryHardcoreRun_UI.activeInHierarchy != true) // this allows to close the retry-hardcore UI when applicable by pressing ESC
        {
            if (pauseMenu.activeInHierarchy != true)
            {
                // deactivate Player Input
                inputHandler.enabled = false;

                // lower volume or pause music:
                GameObject.Find("GameManager").GetComponent<BackgroundSoundPlayer>().PauseMusic();

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
            }else
            {
                // activate Player Input
                inputHandler.enabled = true;

                // increase volume or unpause music:
                GameObject.Find("GameManager").GetComponent<BackgroundSoundPlayer>().UnpauseMusic();

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
