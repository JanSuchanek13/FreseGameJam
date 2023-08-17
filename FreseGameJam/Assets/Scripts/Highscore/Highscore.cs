using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Highscore : MonoBehaviour
{
    // GAMESCOM STUFF:
    //[SerializeField] bool _atGamescom23 = false;
    
    [Space(15)]
    public bool overwriteUI;
    
    [Space(15)]
    [SerializeField] TextMeshProUGUI _hardcoreCrowns_Time;
    [SerializeField] TextMeshProUGUI _hardcoreCrowns_Crowns;
    [SerializeField] TextMeshProUGUI _hardcoreCrowns_Deaths; // kind of redundant
   
    [Space(15)]
    [SerializeField] TextMeshProUGUI _hardcoreTime_Time;
    [SerializeField] TextMeshProUGUI _hardcoreTime_Crowns;
    [SerializeField] TextMeshProUGUI _hardcoreTime_Deaths; // kind of redundant
    
    [Space(15)]
    [SerializeField] TextMeshProUGUI HTtimer;
    [SerializeField] TextMeshProUGUI HTcrowns;
    [SerializeField] TextMeshProUGUI HTdeaths;
   
    [Space(15)]
    [SerializeField] TextMeshProUGUI HCtimer;
    [SerializeField] TextMeshProUGUI HCcrowns;
    [SerializeField] TextMeshProUGUI HCdeaths;
    
    [Space(15)]
    [SerializeField] TextMeshProUGUI Lasttimer;
    [SerializeField] TextMeshProUGUI Lastcrowns;
    [SerializeField] TextMeshProUGUI Lastdeaths;

    float timer;
    int currentLevel;


    // Start is called before the first frame update
    void Start()
    {
        /*
        // Do not delete or turn off while prepping for gamescom!
        if (_atGamescom23)
        {
            Debug.Log("Gamescom23-mode is active. The Highscore-display logic is now enabled.");
        }else
        {
            Debug.Log("Gamescom23-mode is NOT active. The Highscore-display logic is NOT enabled.");
            Debug.Log("To change this just check the atGamescom-box in the inspector of the Highscore-script!");
        }*/

        // save level: // currently redundant
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        UpdateHighscoreUI();
        /*
        if (overwriteUI)
        {
            //for Level 1

            //for HARDCORE-crowns Highscore
            timer = PlayerPrefs.GetFloat("HighscoreHardcoreCrowns_Time" + 0, 0.0f);
            int minutes = (int)timer / 60;
            int seconds = (int)timer - 60 * minutes;
            int milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            _hardcoreCrowns_Time.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            _hardcoreCrowns_Crowns.text = PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0).ToString();
            _hardcoreCrowns_Deaths.text = PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Deaths" + 0, 0).ToString();


            //for HARDCORE-time Highscore
            timer = PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0, 0.0f);

            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            _hardcoreTime_Time.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            _hardcoreTime_Crowns.text = PlayerPrefs.GetInt("HighscoreHardcoreTime_Crowns" + 0, 0).ToString();
            _hardcoreTime_Deaths.text = PlayerPrefs.GetInt("HighscoreHardcoreTime_Deaths" + 0, 0).ToString();


            //for Highscore Time
            timer = PlayerPrefs.GetFloat("HTtimer" + 0, 0.0f);
            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            HTtimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            HTcrowns.text = PlayerPrefs.GetInt("HTcrowns" + (0), 0).ToString();
            HTdeaths.text = PlayerPrefs.GetInt("HTdeaths" + (0), 0).ToString();

            //for Highscore Crowns
            timer = PlayerPrefs.GetFloat("HCtimer" + 0, 0.0f);
            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            HCtimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            HCcrowns.text = PlayerPrefs.GetInt("HCcrowns" + (0), 0).ToString();
            HCdeaths.text = PlayerPrefs.GetInt("HCdeaths" + (0), 0).ToString();

            //for Highscore Last Run
            timer = PlayerPrefs.GetFloat("Lasttimer" + 0, 0.0f);
            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            Lasttimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            Lastcrowns.text = PlayerPrefs.GetInt("Lastcrowns" + (0), 0).ToString();
            Lastdeaths.text = PlayerPrefs.GetInt("Lastdeaths" + (0), 0).ToString();

        }*/
    }

    void UpdateHighscoreUI()
    {
        //for Level 1

        //for HARDCORE-crowns Highscore
        timer = PlayerPrefs.GetFloat("HighscoreHardcoreCrowns_Time" + 0, 0.0f);
        int minutes = (int)timer / 60;
        int seconds = (int)timer - 60 * minutes;
        int milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

        _hardcoreCrowns_Time.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        _hardcoreCrowns_Crowns.text = PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0).ToString();
        _hardcoreCrowns_Deaths.text = PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Deaths" + 0, 0).ToString();


        //for HARDCORE-time Highscore
        timer = PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0, 0.0f);

        minutes = (int)timer / 60;
        seconds = (int)timer - 60 * minutes;
        milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

        _hardcoreTime_Time.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        _hardcoreTime_Crowns.text = PlayerPrefs.GetInt("HighscoreHardcoreTime_Crowns" + 0, 0).ToString();
        _hardcoreTime_Deaths.text = PlayerPrefs.GetInt("HighscoreHardcoreTime_Deaths" + 0, 0).ToString();


        //for Highscore Time
        timer = PlayerPrefs.GetFloat("HTtimer" + 0, 0.0f);
        minutes = (int)timer / 60;
        seconds = (int)timer - 60 * minutes;
        milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

        HTtimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        HTcrowns.text = PlayerPrefs.GetInt("HTcrowns" + (0), 0).ToString();
        HTdeaths.text = PlayerPrefs.GetInt("HTdeaths" + (0), 0).ToString();

        //for Highscore Crowns
        timer = PlayerPrefs.GetFloat("HCtimer" + 0, 0.0f);
        minutes = (int)timer / 60;
        seconds = (int)timer - 60 * minutes;
        milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

        HCtimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        HCcrowns.text = PlayerPrefs.GetInt("HCcrowns" + (0), 0).ToString();
        HCdeaths.text = PlayerPrefs.GetInt("HCdeaths" + (0), 0).ToString();

        //for Highscore Last Run
        timer = PlayerPrefs.GetFloat("Lasttimer" + 0, 0.0f);
        minutes = (int)timer / 60;
        seconds = (int)timer - 60 * minutes;
        milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

        Lasttimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        Lastcrowns.text = PlayerPrefs.GetInt("Lastcrowns" + (0), 0).ToString();
        Lastdeaths.text = PlayerPrefs.GetInt("Lastdeaths" + (0), 0).ToString();
    }

    public void SafeLastStats()
    {
        //safe stats of last Run / will be called on ResetLevel from LevelManager
        Debug.Log("Last Run was saved");
        PlayerPrefs.SetFloat("Lasttimer" + (0), PlayerPrefs.GetFloat("timer" + (0), 0));
        PlayerPrefs.SetInt("Lastcrowns" + (0), PlayerPrefs.GetInt("crowns" + (0), 0));
        PlayerPrefs.SetInt("Lastdeaths" + (0), PlayerPrefs.GetInt("deaths" + (0), 0));
        
        //Start();
        UpdateHighscoreUI();
    }

    public void CompareHighscore()
    {
        // only whilse at gamescom:
        //bool _newSpeedRecord = false;
        //bool _newCrownsRecord = false;


        // for Highscore Time
        if (PlayerPrefs.GetFloat("timer" + 0) < PlayerPrefs.GetFloat("HTtimer" + 0) || PlayerPrefs.GetFloat("HTtimer" + 0) == 0)
        {
            PlayerPrefs.SetFloat("HTtimer" + 0, PlayerPrefs.GetFloat("timer" + 0));
            PlayerPrefs.SetInt("HTcrowns" + 0, PlayerPrefs.GetInt("crowns" + 0));
            PlayerPrefs.SetInt("HTdeaths" + 0, PlayerPrefs.GetInt("deaths" + 0));
        }

        //for Highscore Crowns
        if (PlayerPrefs.GetInt("crowns" + 0) > PlayerPrefs.GetInt("HCcrowns" + 0))
        {
            PlayerPrefs.SetFloat("HCtimer" + 0, PlayerPrefs.GetFloat("timer" + 0));
            PlayerPrefs.SetInt("HCcrowns" + 0, PlayerPrefs.GetInt("crowns" + 0));
            PlayerPrefs.SetInt("HCdeaths" + 0, PlayerPrefs.GetInt("deaths" + 0));
        }

        // update highscore for hardcore crowns;
        if (PlayerPrefs.GetInt("HardcoreCrowns" + 0) > PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0))
        {
            PlayerPrefs.SetFloat("HighscoreHardcoreCrowns_Time" + 0, PlayerPrefs.GetFloat("HardcoreTime" + 0, 0.0f));
            PlayerPrefs.SetInt("HighscoreHardcoreCrowns_Crowns" + 0, PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0));
            PlayerPrefs.SetInt("HighscoreHardcoreCrowns_Deaths" + 0, PlayerPrefs.GetInt("HardcoreDeaths" + 0, 0));

            // GAMESCOM STUFF:
            //_newCrownsRecord = true;
        }

        // update highscore for hardcore time:
        if (PlayerPrefs.GetFloat("HardcoreTime" + 0) < PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0) || PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0) == 0)
        {
            PlayerPrefs.SetFloat("HighscoreHardcoreTime_Time" + 0, PlayerPrefs.GetFloat("HardcoreTime" + 0, 0.0f));
            PlayerPrefs.SetInt("HighscoreHardcoreTime_Crowns" + 0, PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0));
            PlayerPrefs.SetInt("HighscoreHardcoreTime_Deaths" + 0, PlayerPrefs.GetInt("HardcoreDeaths" + 0, 0));

            // GAMESCOM STUFF:
            //_newSpeedRecord = true;
        }

        /*
        // GAMESCOM STUFF:
        if (_atGamescom23)
        {
            HighscoreCompilerJson _compilerScript = FindObjectOfType<HighscoreCompilerJson>();

            if (_newSpeedRecord && !_newCrownsRecord)
            {
                _compilerScript.NewHighscore(0);
            }else if(!_newSpeedRecord && _newCrownsRecord)
            {
                _compilerScript.NewHighscore(1);
            }else if (_newSpeedRecord && _newCrownsRecord)
            {
                _compilerScript.NewHighscore(2);
            }
        }*/
    }


    /// <summary>
    /// This is called by the ResetAllStats-function inside the Level_Manager.
    /// This will reload the Highscore-stats, defauling all values to 0.
    /// Use with care. This is irriversible.
    /// </summary>
    public void ResetAllHighscores()
    {
        CompareHighscore();
        //overwriteUI = true;
        //Start();
        UpdateHighscoreUI();
    }
}
