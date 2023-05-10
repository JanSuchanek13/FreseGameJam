using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Highscore : MonoBehaviour
{
    int currentLevel;

    public bool overwriteUI;
    [SerializeField] TextMeshProUGUI _hardcoreTimer;
    [SerializeField] TextMeshProUGUI _hardcoreCrowns;
    [SerializeField] TextMeshProUGUI _hardcoreDeaths;

    [SerializeField] TextMeshProUGUI HTtimer;
    [SerializeField] TextMeshProUGUI HTcrowns;
    [SerializeField] TextMeshProUGUI HTdeaths;

    [SerializeField] TextMeshProUGUI HCtimer;
    [SerializeField] TextMeshProUGUI HCcrowns;
    [SerializeField] TextMeshProUGUI HCdeaths;

    [SerializeField] TextMeshProUGUI Lasttimer;
    [SerializeField] TextMeshProUGUI Lastcrowns;
    [SerializeField] TextMeshProUGUI Lastdeaths;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        //safe level
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //activate this if you want to reset all Highscore Values
        /*
        PlayerPrefs.SetFloat("HTtimer" + (0), 0);
        PlayerPrefs.SetInt("HTcrowns" + (0), 0);
        PlayerPrefs.SetInt("HTdeaths" + (0), 0);
        PlayerPrefs.SetFloat("HCtimer" + (0), 0);
        PlayerPrefs.SetInt("HCcrowns" + (0), 0);
        PlayerPrefs.SetInt("HCdeaths" + (0), 0);
        PlayerPrefs.SetFloat("Lasttimer" + (0), 0);
        PlayerPrefs.SetInt("Lastcrowns" + (0), 0);
        PlayerPrefs.SetInt("Lastdeaths" + (0), 0);
        */


        if (overwriteUI)
        {
            //for Level 1

            //for HARDCORE Highscore
            timer = PlayerPrefs.GetFloat("HighscoreHardcoreTime" + (0));
            int minutes = (int)timer / 60;
            int seconds = (int)timer - 60 * minutes;
            int milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            _hardcoreTimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            _hardcoreCrowns.text = PlayerPrefs.GetInt("HighscoreHardcoreCrowns" + (0)).ToString();
            _hardcoreDeaths.text = PlayerPrefs.GetInt("HighscoreHardcoreDeaths" + (0)).ToString();

            //for Highscore Time
            timer = PlayerPrefs.GetFloat("HTtimer" + (0));
            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            HTtimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            HTcrowns.text = PlayerPrefs.GetInt("HTcrowns" + (0)).ToString();
            HTdeaths.text = PlayerPrefs.GetInt("HTdeaths" + (0)).ToString();

            //for Highscore Crowns
            timer = PlayerPrefs.GetFloat("HCtimer" + (0));
            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            HCtimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            HCcrowns.text = PlayerPrefs.GetInt("HCcrowns" + (0)).ToString();
            HCdeaths.text = PlayerPrefs.GetInt("HCdeaths" + (0)).ToString();

            //for Highscore Last Run
            timer = PlayerPrefs.GetFloat("Lasttimer" + (0));
            minutes = (int)timer / 60;
            seconds = (int)timer - 60 * minutes;
            milliseconds = (int)(1000 * (timer - minutes * 60 - seconds));

            Lasttimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            Lastcrowns.text = PlayerPrefs.GetInt("Lastcrowns" + (0)).ToString();
            Lastdeaths.text = PlayerPrefs.GetInt("Lastdeaths" + (0)).ToString();

        }
    }

    public void SafeLastStats()
    {
        //safe stats of last Run / will be called on ResetLevel from LevelManager
        Debug.Log("Last Run was safed");
        PlayerPrefs.SetFloat("Lasttimer" + (0), PlayerPrefs.GetFloat("timer" + (0)));
        PlayerPrefs.SetInt("Lastcrowns" + (0), PlayerPrefs.GetInt("crowns" + (0)));
        PlayerPrefs.SetInt("Lastdeaths" + (0), PlayerPrefs.GetInt("deaths" + (0)));
        Start();
    }

    public void CompareHighscore()
    {
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

        // for hardcore: (time is key)
        if (PlayerPrefs.GetFloat("HardcoreTime" + 0) < PlayerPrefs.GetFloat("HighscoreHardcoreTime" + 0) || PlayerPrefs.GetFloat("HighscoreHardcoreTime" + 0) == 0)
        {
            PlayerPrefs.SetFloat("HighscoreHardcoreTime" + 0, PlayerPrefs.GetFloat("HardcoreTime" + 0));
            PlayerPrefs.SetInt("HighscoreHardcoreCrowns" + 0, PlayerPrefs.GetInt("HardcoreCrowns" + 0));
            PlayerPrefs.SetInt("HighscoreHardcoreDeaths" + 0, PlayerPrefs.GetInt("HardcoreDeaths" + 0));
        }
    }
}
