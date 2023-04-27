using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HardcoreMode : MonoBehaviour
{
    [Header("Hardcore Mode settings:")]
    public bool useHardcoreMode = false;
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
    [SerializeField] GameObject _checkpointParent;
    [SerializeField] GameObject _signs;
    [SerializeField] GameObject _normalUI;

    float _timeElapsed;
    bool _runStarted = false;
    public bool runFinished = false; // this gets updated by the LevelScript!

    private void Start()
    {
        int _playHardcore = PlayerPrefs.GetInt("HardcoreMode", 0);
        if (_playHardcore == 1)
        {
            useHardcoreMode = true;
            StartHardcoreRun();
        }
    }
    public void StartHardcoreRun()
    {
        // Adjust the level to HARDCORE:
        FindObjectOfType<InputHandler>().enabled = false;    
        _timeElapsed = 0.0f;
        _hardcoreUI.SetActive(true);
        _checkpointParent.SetActive(false);
        _normalUI.SetActive(false);
        DisableScriptOfType<DelaySound>();
        //DisableScriptOfType<DelaySound>().parent.;
        //GameObject  FindObjectsOfType<DelaySound>()
        DisableScriptOfType<TriggerSound>();
        FindObjectOfType<FocusPlayerViewOnObject>().enabled = false;

        foreach(Transform child in _signs.transform)
        {
            foreach(Transform childOfChild in child)
            {
            Collider collider = childOfChild.GetComponent<Collider>();
            if(collider != null && collider.isTrigger)
            {
            //child.gameObject.SetActive(false);
            childOfChild.gameObject.SetActive(false);
            break; // exit the inner loop if a trigger collider is found
            }
        }
}

        // Start countdown to run:
        StartCoroutine(CountDownToGo());
    }

    IEnumerator CountDownToGo()
    {
        //float _currentCountDown = _countdownTime;
        float _countDownInterval = _countdownTimeInSeconds / 3;

        _countDown_txt.text = 3.ToString();

        yield return new WaitForSeconds(_countDownInterval);

        _countDown_txt.text = 2.ToString();

        yield return new WaitForSeconds(_countDownInterval);

        _countDown_txt.text = 1.ToString();

        yield return new WaitForSeconds(_countDownInterval);

        _countDown_txt.text = "GO!";
        BeginnRun();

        // juicy delay before turning off the "GO!":
        yield return new WaitForSeconds(2.0f);
        _countDown_txt.text = null;
    }

    void BeginnRun()
    {
        _runStarted = true;
        FindObjectOfType<InputHandler>().enabled = true;
    }

    private void Update()
    {
        if (_runStarted && !runFinished)
        {
            int _crowns = PlayerPrefs.GetInt("crowns" + 0, 1);
            //transform crowns counted into Hardcore crowns:
            PlayerPrefs.SetInt("HardcoreCrowns" + 0, _crowns);
            _crowns_txt.text = _crowns.ToString();

            _timeElapsed += Time.deltaTime;
            PlayerPrefs.SetFloat("HardcoreTime" + 0, _timeElapsed);

            //_timer_txt.text += Time.deltaTime.ToString();
            //float _totalLevelTime = PlayerPrefs.GetFloat("timer" + 0, 0) + PlayerPrefs.GetFloat("lastTimer" + 0, 0);
            int _minutes = Mathf.FloorToInt(_timeElapsed / 60);
            int _seconds = Mathf.FloorToInt(_timeElapsed % 60);
            int _milliseconds = Mathf.FloorToInt((_timeElapsed * 1000) % 1000);

            // the chosen way ensures that the number display doesnt flicker during rapid changes:
            //_minuteTimer_txt.text = string.Format("{0:00}:{1:00}:{2:000}", _minutes, _seconds, _milliseconds);
            _minuteTimer_txt.text = string.Format("{0:00}", _minutes);
            _secondTimer_txt.text = string.Format("{0:00}", _seconds);
            _milisecondTimer_txt.text = string.Format("{0:000}", _milliseconds);
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

    /*
    public void ResetRun()
    {

    }*/
}
