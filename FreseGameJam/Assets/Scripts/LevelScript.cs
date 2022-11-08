using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelScript : MonoBehaviour
{
    /// <summary>
    /// This Script handles the transition between Levels.
    /// </summary>

    // public variables:
    [Header("Level Transition Settings:")]
    [SerializeField] Highscore _highscore;
    [SerializeField] float _timeBeforeLoadingNewLevel = 5.0f;

    public Animator transition;
    public int nextLevel = 3;
    [SerializeField] Image _fadeToBlackBlende;
    [SerializeField] GameObject _thankYouForPlayingOverlay;


    // local variables:
    float[] timer = new float[3];
    float[] lastTimer = new float[3];
    int currentLevel;
    
    // variables for fading screen:
    float _fadeIntervalls;
    private float _increaceColorAlphaIncrement = 255.0f / 100.0f; // maximum color value devided into 100 increments.
    //private float _color_R_component;
    //private float _color_G_component;
    //private float _color_B_component;
    //private float _color_A_component;
    private void Start()
    {
        // save current level:
        currentLevel = SceneManager.GetActiveScene().buildIndex;
         
        // add time of last run:
        timer[(currentLevel - 2)] = PlayerPrefs.GetFloat("timer" + (currentLevel - 2));

        // determine time for fading:
        //_fadeIntervalls = (_timeBeforeLoadingNewLevel * 0.3f) / 100.0f; // this works too, but does it need to depend on the total time?! I think not!
        _fadeIntervalls = 1.5f / 100.0f; // tried and tested, this feeld pretty good.
    }

    private void Update()
    {
        // save time per frame
        PlayerPrefs.SetFloat("timer" + (currentLevel - 2), Time.timeSinceLevelLoad + timer[(currentLevel - 2)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(_fadeToBlackBlende != null) // only fade if there is a fadeBlende in place.
            {
                StartCoroutine(FadeToBlack());
                Invoke("DisplayText", 3.5f);
            }

            Debug.Log("Level endzone reached!");
            Pass();
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
        _highscore.CompareHighscore();

        // change level:
        StartCoroutine(LoadLevel(nextLevel)); // may need a _inCoroutine bool to avoid starting loading the next level multiple times -F
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start"); // what is loaded here? -F

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
        }
    }
}
