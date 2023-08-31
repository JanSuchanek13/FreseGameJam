using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LevelScript : MonoBehaviour
{
    // This Script handles the transition between Levels.

    // accessible variables:
    [Header("Level Transition Settings:")]
    [SerializeField] Highscore _highscore;
    [SerializeField] float _timeBeforeLoadingNewLevel = 5.0f;
    [SerializeField] AudioSource _choirSound;
    [SerializeField] AudioSource _victorySound;
    //[SerializeField] GameObject _regularUI;
    //[SerializeField] GameObject _hardcoreUI;


    // public variables:
    public Animator transition;
    public int nextLevel = 3;
    public bool _usingHardcoreMode = false;


    [SerializeField] Image _fadeToBlackBlende;
    [SerializeField] GameObject _thankYouForPlayingOverlay;

    [SerializeField] TextMeshProUGUI _statDisplay_Time;
    [SerializeField] TextMeshProUGUI _statDisplay_Deaths;
    [SerializeField] TextMeshProUGUI _statDisplay_Crowns;
    [SerializeField] int level;

    // local variables:
    float[] _timer = new float[3];
    float[] lastTimer = new float[3];
    int _currentLevel;
    bool _endZoneReached = false;
    bool _clockRunning = true;
    GameObject _gameManager;

    // variables for fading screen:
    float _fadeIntervalls;
    private float _increaceColorAlphaIncrement = 255.0f / 100.0f; // maximum color value devided into 100 increments.


    private void Start()
    {
        // save current level:
        _currentLevel = SceneManager.GetActiveScene().buildIndex;
         
        // add time of last run:
        _timer[0] = PlayerPrefs.GetFloat("timer" + 0); // (currentLevel - 2) & (currentLevel - 2) at end

        // determine time for fading:
        _fadeIntervalls = 1.5f / 100.0f; // tried and tested, this feeld pretty good.

        _gameManager = GameObject.Find("GameManager");

        /*if(FindObjectOfType<HardcoreMode>().useHardcoreMode == true)
        {
        _usingHardcoreMode = true;
        }*/
        //_usingHardcoreMode = FindObjectOfType<HardcoreMode>().useHardcoreMode; to late
    }

    private void Update()
    {
        //if(FindObjectOfType<HardcoreMode>().useHardcoreMode == true)
        if (PlayerPrefs.GetInt("HardcoreMode", 0) == 1 && !_usingHardcoreMode) // in update, as its switched later than start, i think?
        {
        _usingHardcoreMode = true;
        }
        else if (!_endZoneReached && _clockRunning)
        {
            PlayerPrefs.SetFloat("timer" + 0, Time.timeSinceLevelLoad + _timer[0]); //(currentLevel - 2)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // prevent triggering multiple times
        if (!_endZoneReached)
        {
            if (other.tag == "Player")
            {
                // this bool ensures that the end of a level is triggered only once and immediatly!
                _endZoneReached = true;

                if (!_usingHardcoreMode)
                {
                    Debug.Log("normal finished");


                    if (_fadeToBlackBlende != null) // only fade if there is a fadeBlende in place.
                    {
                        StartCoroutine(FadeToBlack());
                        Invoke("DisplayText", 3.5f);
                    }
                    // stop music & play success-music:
                    _gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic(); // lower volume or pause music
                                                                                     //_gameManager.GetComponent<BackgroundSoundPlayer>().TurnOffMusic(); // stop music


                    // changed these "finds" to serialized fields to manually controll end-sounds:
                    //GameObject.Find("AudioSource_Victory_1").GetComponent<AudioSource>().Play();
                    //GameObject.Find("AudioSource_ChoireHymn_1").GetComponent<AudioSource>().Play();
                    _choirSound.Play();
                    _victorySound.Play();

                    //GameObject.Find("UI_Crown_Counter").SetActive(false); // turn off the regular, ingame crown-counter and icon
                    GameObject.Find("Ingame_UI").SetActive(false); // turn off the regular, ingame crown-counter and icon
                    //_regularUI.SetActive(false);

                    //Debug.Log("Level endzone reached!");
                    LevelFinished();

                    // call up the data-entry UI:
                    //FindObjectOfType<GamesomHighscoreManager>().GamescomRegularRunFinished(); // DELETE AFTER GAMESCOM2023!
                }else
                {
                    // turn off the (hardcore-) UI and fade to black:
                    FindObjectOfType<HardcoreMode>().runFinished = true;

                    if (_fadeToBlackBlende != null) // only fade if there is a fadeBlende in place.
                    {
                        StartCoroutine(FadeToBlack());
                        Invoke("DisplayText", 3.5f);
                    }

                    // stop music & play success-music:
                    //_gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic(); // pausing will just lower volume
                    _gameManager.GetComponent<BackgroundSoundPlayer>().TurnOffMusic();
                    _choirSound.Play();
                    _victorySound.Play();

                    //GameObject.Find("Hardcore_UI").SetActive(false); // turn off the Hardcore_UI
                    //_hardcoreUI.SetActive(false);

                    //HardcoreFinished();

                    // call up the data-entry UI:
                    //FindObjectOfType<GamesomHighscoreManager>().GamescomHardcoreRunFinished(); // DELETE AFTER GAMESCOM2023!

                    // this is commented to not allow people enter hardcore data, without providing their email and potentially skewer the results.
                    LevelFinished(); // UNCOMMENT AFTER GAMESCOM2023!
                }
            }
        }
    }

    public void StopTheClock()
    {
        if (PlayerPrefs.GetInt("HardcoreMode", 0) != 0)
        {
            Debug.Log("stopped hardcore clock");
            FindObjectOfType<HardcoreMode>().stopTheClock = true;
        }else
        {
            Debug.Log("stopped regular clock");
            _clockRunning = false;
        }
    }

    public void LevelFinished()
    {
        // save level:
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // this is already done in start o0...

        // determine next level:
        if (currentLevel >= PlayerPrefs.GetInt("levelIsUnlocked"))
        {
            PlayerPrefs.SetInt("levelIsUnlocked", currentLevel + 1);
            Debug.Log("Level " + PlayerPrefs.GetInt("levelIsUnlocked") + " is now unlocked!");
        }

        // check for- and process possible highscore (relevant for regular here):
        _gameManager.GetComponent<Highscore>().CompareHighscore();

        // change level:
        StartCoroutine(LoadLevel(nextLevel)); // may need a _inCoroutine bool to avoid starting loading the next level multiple times -F
    }
    /*
    public void HardcoreFinished()
    {
        // save level:
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // this is already done in start o0...

        // determine next level:
        if (currentLevel >= PlayerPrefs.GetInt("levelIsUnlocked"))
        {
            PlayerPrefs.SetInt("levelIsUnlocked", currentLevel + 1);
            Debug.Log("Level " + PlayerPrefs.GetInt("levelIsUnlocked") + " is now unlocked!");
        }

        // check for- and process possible highscore (relevant for hardcore here):
        _gameManager.GetComponent<Highscore>().CompareHighscore();

        // change level:
        StartCoroutine(LoadLevel(nextLevel)); // may need a _inCoroutine bool to avoid starting loading the next level multiple times -F
    }*/

    /// <summary>
    /// This is to be used if someone didnt want their info collected. It goes back to main without comparing highscores.
    /// </summary>
    public void AbortHighscoreCheck()
    {
        StartCoroutine(LoadLevel(nextLevel));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(_timeBeforeLoadingNewLevel);

        SceneManager.LoadScene(nextLevel);
    }


    // These two functions only get called if their variables were assigned in advance:
    IEnumerator FadeToBlack()
    {
        _fadeToBlackBlende.color += new Color32((byte)0.0f, (byte)0.0f, (byte)0.0f, (byte)_increaceColorAlphaIncrement);

        yield return new WaitForSeconds(_fadeIntervalls);
        
        StartCoroutine(FadeToBlack());
    }

    void DisplayText()
    {
        if (_thankYouForPlayingOverlay != null) // only try to display overlay-text, if there is overlay-text in place.
        {
            // turn off all player input in the background:
            if(FindObjectOfType<InputHandler>() != null)
            {
                FindObjectOfType<InputHandler>().enabled = false;
            }


            _thankYouForPlayingOverlay.SetActive(true);

            // display current levels total runtime:
            if (_statDisplay_Time != null && !_usingHardcoreMode)
            {
                float _totalLevelTime = PlayerPrefs.GetFloat("timer" + level, 0) + PlayerPrefs.GetFloat("lastTimer" + level, 0);
                int _minutes = (int)_totalLevelTime / 60;
                int _seconds = (int)_totalLevelTime - 60 * _minutes;
                int _milliseconds = (int)(1000 * (_totalLevelTime - _minutes * 60 - _seconds));

                _statDisplay_Time.text = string.Format("{0:00}:{1:00}:{2:000}", _minutes, _seconds, _milliseconds);

                // display current levels total deaths:
                if (_statDisplay_Deaths != null)
                {
                    _statDisplay_Deaths.text = PlayerPrefs.GetInt("deaths" + level, 1).ToString();
                }

                // display current levels total crowns:
                if (_statDisplay_Crowns != null)
                {
                    int crowns = PlayerPrefs.GetInt("crowns" + level, 1);
                    _statDisplay_Crowns.text = crowns.ToString();
                }
            }
            else if(_usingHardcoreMode)
            {
                float _totalLevelTime = PlayerPrefs.GetFloat("HardcoreTime" + level, 0);
                int _minutes = (int)_totalLevelTime / 60;
                int _seconds = (int)_totalLevelTime - 60 * _minutes;
                int _milliseconds = (int)(1000 * (_totalLevelTime - _minutes * 60 - _seconds));

                _statDisplay_Time.text = string.Format("{0:00}:{1:00}:{2:000}", _minutes, _seconds, _milliseconds);

                // display current levels total deaths:
                if (_statDisplay_Deaths != null)
                {
                    _statDisplay_Deaths.text = PlayerPrefs.GetInt("HardcoreDeaths" + level, 1).ToString(); // +level could be an issue here
                }

                // display current levels total crowns:
                if (_statDisplay_Crowns != null)
                {
                    int crowns = PlayerPrefs.GetInt("HardcoreCrowns" + level, 1); // +level could be an issue here
                    _statDisplay_Crowns.text = crowns.ToString();
                }
            }
        }
    }
}
