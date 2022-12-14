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

    // public variables:
    public Animator transition;
    public int nextLevel = 3;

    [SerializeField] Image _fadeToBlackBlende;
    [SerializeField] GameObject _thankYouForPlayingOverlay;

    [SerializeField] TextMeshProUGUI _statDisplay_Time;
    [SerializeField] TextMeshProUGUI _statDisplay_Deaths;
    [SerializeField] TextMeshProUGUI _statDisplay_Crowns;
    [SerializeField] int level;

    // local variables:
    float[] timer = new float[3];
    float[] lastTimer = new float[3];
    int currentLevel;
    bool _endZoneReached = false;
    GameObject _gameManager;

    // variables for fading screen:
    float _fadeIntervalls;
    private float _increaceColorAlphaIncrement = 255.0f / 100.0f; // maximum color value devided into 100 increments.


    private void Start()
    {
        // save current level:
        currentLevel = SceneManager.GetActiveScene().buildIndex;
         
        // add time of last run:
        timer[0] = PlayerPrefs.GetFloat("timer" + 0.0f); // (currentLevel - 2) & (currentLevel - 2) at end

        // determine time for fading:
        //_fadeIntervalls = (_timeBeforeLoadingNewLevel * 0.3f) / 100.0f; // this works too, but does it need to depend on the total time?! I think not!
        _fadeIntervalls = 1.5f / 100.0f; // tried and tested, this feeld pretty good.

        _gameManager = GameObject.Find("GameManager");
    }

    private void Update()
    {
        // save time per frame
        if (!_endZoneReached)
        {
            PlayerPrefs.SetFloat("timer" + (currentLevel - 2), Time.timeSinceLevelLoad + timer[0]); //(currentLevel - 2)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_endZoneReached)
        {
            if (other.tag == "Player")
            {
                if (_fadeToBlackBlende != null) // only fade if there is a fadeBlende in place.
                {
                    StartCoroutine(FadeToBlack());
                    Invoke("DisplayText", 3.5f);
                }
                // stop music & play success-music:
                _gameManager.GetComponent<BackgroundSoundPlayer>().PauseMusic(); // turn off background music

                // changed these "finds" to serialized fields to manually controll end-sounds:
                //GameObject.Find("AudioSource_Victory_1").GetComponent<AudioSource>().Play();
                //GameObject.Find("AudioSource_ChoireHymn_1").GetComponent<AudioSource>().Play();
                _choirSound.Play();
                _victorySound.Play();

                GameObject.Find("UI_Crown_Counter").SetActive(false); // turn off the regular, ingame crown-counter and icon

                //Debug.Log("Level endzone reached!");
                Pass();
                
                // this bool ensures that the end of a level is triggered only once and immediatly!
                _endZoneReached = true;
            }
        }
    }

    public void Pass()
    {
        // save level:
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel >= PlayerPrefs.GetInt("levelIsUnlocked"))
        {
            PlayerPrefs.SetInt("levelIsUnlocked", currentLevel + 1);
            Debug.Log("Level " + PlayerPrefs.GetInt("levelIsUnlocked") + " is now unlocked!");
        }

        // save highscore:
        //_highscore.CompareHighscore();
        _gameManager.GetComponent<Highscore>().CompareHighscore();

        // change level:
        StartCoroutine(LoadLevel(nextLevel)); // may need a _inCoroutine bool to avoid starting loading the next level multiple times -F
    }


    IEnumerator LoadLevel(int levelIndex)
    {
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
            _thankYouForPlayingOverlay.SetActive(true);

            // display current levels total runtime:
            if (_statDisplay_Time != null)
            {
                float _totalLevelTime = PlayerPrefs.GetFloat("timer" + level, 0) + PlayerPrefs.GetFloat("lastTimer" + level, 0);
                int _minutes = (int)_totalLevelTime / 60;
                int _seconds = (int)_totalLevelTime - 60 * _minutes;
                int _milliseconds = (int)(1000 * (_totalLevelTime - _minutes * 60 - _seconds));

                _statDisplay_Time.text = string.Format("{0:00}:{1:00}:{2:000}", _minutes, _seconds, _milliseconds);
            }

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
    }
}
