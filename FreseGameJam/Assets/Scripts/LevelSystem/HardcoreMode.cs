using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HardcoreMode : MonoBehaviour
{
    [Header("Hardcore Mode settings:")] // cant this be turnt off?
    public bool useHardcoreMode = false; // cant this be turnt off?
    //public bool hardcoreRunStarted = false;

    [Tooltip("The display will always show 3,2,1 --> but the time this takes can be adjusted here.")]
    [SerializeField] float _countdownTimeInSeconds = 3.0f;
    [SerializeField] GameObject _hardcoreUI;
    [SerializeField] TextMeshProUGUI _minuteTimer_txt;
    [SerializeField] TextMeshProUGUI _secondTimer_txt;
    [SerializeField] TextMeshProUGUI _milisecondTimer_txt;
    [Space(10)]

    [SerializeField] TextMeshProUGUI _crowns_txt;
    [SerializeField] TextMeshProUGUI _countDown_txt;
    [Space(10)]

    [Header("Things to turn off:")]
    [SerializeField] bool _newHardcoreWorld = true;
    [Space(10)]
    [SerializeField] GameObject _checkpointParent;
    [SerializeField] GameObject _signs; 
    [SerializeField] GameObject _normalUI;
    [SerializeField] GameObject _regularEndGameTrigger;
    [Space(10)]
    [SerializeField] GameObject _ocean;
    [SerializeField] GameObject _waterSpray;
    [SerializeField] GameObject _normalClouds;

    [Header("Things to turn on instead:")]
    [SerializeField] Material _burningSkySkybox;
    [SerializeField] GameObject _darkClouds; // all turn on objects should be combined to a single parent later!
    [SerializeField] GameObject _magmaOcean;
    [SerializeField] GameObject _flameAndAshSpray;
    //new:
    [SerializeField] GameObject _breakingFloor;

    float _timeElapsed;
    int _minutes;
    int _seconds;
    int _milliseconds;
    public bool hardcoreRunStarted = false;

    public bool stopTheClock = false; // this gets updated by the Levelscript when the friend is reached!
    public bool runFinished = false; // this gets updated by the LevelScript when the bottom-trigger is reached falling!

    private void Start()
    {
        if (PlayerPrefs.GetInt("HardcoreMode", 0) == 1)
        {
            useHardcoreMode = true; // cant this be turnt off?
            StartHardcoreRun();
            ResetCurrentHardcoreCrowns(); // this should reset crown display at start of run:
        }else // no longer destroy dropzone at end of game, instead add Script to call the forced move when reaching endzone!
        {
            Destroy(_breakingFloor.GetComponent<BreakingObj>());
            _breakingFloor.GetComponent<RegularGameEnd>().RegularEnd = true;
        }
    }
    public void StartHardcoreRun()
    {
        // adjust the level to HARDCORE:
        FindObjectOfType<InputHandler>().enabled = false;    
        _timeElapsed = 0.0f;

        // change world, UI, and logic:
        ChangeUIAndEnvironment();

        // start countdown to run:
        StartCoroutine(CountDownToGo());
    }

    IEnumerator CountDownToGo()
    {
        float _countDownInterval = _countdownTimeInSeconds / 3;

        _countDown_txt.text = 3.ToString();

        yield return new WaitForSeconds(_countDownInterval);

        _countDown_txt.text = 2.ToString();

        yield return new WaitForSeconds(_countDownInterval);

        _countDown_txt.text = 1.ToString();
        // interrupt music a tad early, so hardcore-music starts playing on "Go":
        //FindObjectOfType<BackgroundSoundPlayer>().PlayHardcoreMusic();

        yield return new WaitForSeconds(_countDownInterval);

        _countDown_txt.text = "GO!";
        BeginnRun();

        // juicy delay before turning off the "GO!":
        yield return new WaitForSeconds(2.0f);
        _countDown_txt.text = null;
    }

    void BeginnRun()
    {
        FindObjectOfType<BackgroundSoundPlayer>().StartHardcoreMusic(); //here the interrupt starts at "Go" so hardcore music doesnt start until counter is at 1 or 2 secs.
        hardcoreRunStarted = true;
        FindObjectOfType<InputHandler>().enabled = true;
    }

    void ChangeUIAndEnvironment()
    {
        // turn off that, which needs turning off:
        _hardcoreUI.SetActive(true);
        _checkpointParent.SetActive(false);
        _normalUI.SetActive(false);
        _regularEndGameTrigger.SetActive(false);

        if (_newHardcoreWorld)
        {
            _normalClouds.SetActive(false);
            _waterSpray.SetActive(false);
            _ocean.SetActive(false);
        }

        // turn off scripts which are not needed in Hardcore:
        DisableScriptOfType<DelaySound>();
        DisableScriptOfType<TriggerSound>();
        FindObjectOfType<FocusPlayerViewOnObject>().enabled = false;

        foreach (Transform child in _signs.transform)
        {
            foreach (Transform childOfChild in child)
            {
                Collider collider = childOfChild.GetComponent<Collider>();

                if (collider != null && collider.isTrigger)
                {
                    childOfChild.gameObject.SetActive(false);
                    break; // exit the inner loop if a trigger collider is found
                }
            }
        }

        // turn on that, which needs turning on:
        if (_newHardcoreWorld)
        {
            _magmaOcean.SetActive(true);
            _flameAndAshSpray.SetActive(true);
            _darkClouds.SetActive(true);
            RenderSettings.skybox = _burningSkySkybox; // swap skybox in runtime
        }
    }

    private void Update()
    {
        if (hardcoreRunStarted && !runFinished)
        {
            if (!stopTheClock)
            {
                _timeElapsed += Time.deltaTime;
                PlayerPrefs.SetFloat("HardcoreTime" + 0, _timeElapsed);

                _minutes = Mathf.FloorToInt(_timeElapsed / 60);
                _seconds = Mathf.FloorToInt(_timeElapsed % 60);
                _milliseconds = Mathf.FloorToInt((_timeElapsed * 1000) % 1000);
            }

            int _crowns = PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0);
            _crowns_txt.text = _crowns.ToString();

            // the chosen way ensures that the number display doesnt flicker during rapid changes:
            _minuteTimer_txt.text = string.Format("{0:00}", _minutes);
            _secondTimer_txt.text = string.Format("{0:00}", _seconds);
            _milisecondTimer_txt.text = string.Format("{0:000}", _milliseconds);

        }
        else if(hardcoreRunStarted && runFinished) // = no longer update time
        {
            _hardcoreUI.SetActive(false);
        }
    }

    public void DisableScriptOfType<ScriptName>() where ScriptName : MonoBehaviour
    {
        ScriptName[] _scripts = FindObjectsOfType<ScriptName>();
        foreach (ScriptName script in _scripts)
        {
            script.enabled = false;
        }
    }

    /// <summary>
    /// This is called by the HealthScript when the player dies in the hardcore mode.
    /// He will immediatly lose all collected crowns and cannot attempt to collect them in the same run again.
    /// </summary>
    public void ResetCurrentHardcoreCrowns()
    {
        PlayerPrefs.SetInt("HardcoreCrowns" + 0, 0);
        int _crowns = PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0);
        _crowns_txt.text = _crowns.ToString();
    }
}
