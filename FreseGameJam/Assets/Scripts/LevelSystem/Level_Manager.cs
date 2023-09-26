using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Level_Manager : MonoBehaviour
{
    public int numberOfLevels = 1; //delete this if we have 3 Level
    [SerializeField] Highscore highscore;

    public int levelIsUnlocked;
    public Button continueButton;
    public float[] timer = new float[3];
    
    public int[] crowns = new int[3];
    // newly added to track hardcore crowns:

    public int[] deaths = new int[3];

    public int[] checkpoints = new int[3];

    public int[] states = new int[3];


    public Button[] buttons;
    public TextMeshProUGUI[] CrownCounters;
    public TextMeshProUGUI[] TimeCounters;
    public TextMeshProUGUI[] DeathCounters;

    public TextMeshProUGUI[] HS_CrownCounters;
    public TextMeshProUGUI[] HS_TimeCounters;
    public TextMeshProUGUI[] HS_DeathCounters;

    public bool hardcoreActive = false;

    void Start()
    {
        // currently redundant - used to unlock future levels:
        levelIsUnlocked = PlayerPrefs.GetInt("levelIsUnlocked", 1);
        
        // disable all buttons:
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        // enable all buttons that have been unlocked:
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            buttons[i].interactable = true;
        }

        // lock continue-button if the level has not been played yet:
        if (levelIsUnlocked > 1)
        {
            continueButton.interactable = false;
        }

        //crown Counter // redundant?
        //int currentLevel = SceneManager.GetActiveScene().buildIndex;

        // update current-run-info in the highscore screen? I think
        for (int i = 0; i < levelIsUnlocked; i++)   // shows only stats for unlocked levels
        {
            // what happens here?
            //crowns[i] = PlayerPrefs.GetInt("crowns"+ i, 1);
            crowns[i] = PlayerPrefs.GetInt("crowns" + i, 0);
            //Debug.Log("crowns current: " + crowns[i]);

            if (i < numberOfLevels)     // fixes error cause only 1 Level is shown
            {
                // show cur crowns in play window:
                CrownCounters[i].text = crowns[i].ToString();

                // show cur crowns in highscore window:
                HS_CrownCounters[i].text = crowns[i].ToString();
            }
            //Debug.Log("crowns current text: " + CrownCounters[i].text);

        }

        //Timer
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            timer[i] = PlayerPrefs.GetFloat("timer" + i, 0);

            int minutes = (int)timer[i] / 60;
            int seconds = (int)timer[i] - 60 * minutes;
            int milliseconds = (int)(1000 * (timer[i] - minutes * 60 - seconds));

            if (i < numberOfLevels)
            {
                // show cur time in play window:
                TimeCounters[i].text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
                
                // show cur time in highscore window:
                HS_TimeCounters[i].text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds); // to display current stats in Highscore (HS) interface of start screen
            }
        }

        //Death
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            deaths[i] = PlayerPrefs.GetInt("deaths" + i, 0);

            if (i < numberOfLevels)
            {
                // show cur deaths in play window:
                DeathCounters[i].text = deaths[i].ToString();

                // show cur deaths in highscore window:
                HS_DeathCounters[i].text = deaths[i].ToString();
            }
        }
    }

    // start a new run in a chosen level:
    public void LoadLevel(int levelIndex)
    {
        //SceneManager.LoadScene(levelIndex);

        // reset the crowns in a specific level:
        // this has to be here for the levelIndex.
        if (PlayerPrefs.GetInt("HardcoreMode", 0) != 0)
        {
            hardcoreActive = true;

            //ResetHardcore();
        }else
        {
            PlayerPrefs.SetInt("crowns" + (levelIndex - 2), 0);
        }

        /* // this is the same as the thing above
        if (!hardcoreActive)
        {
            PlayerPrefs.SetInt("crowns" + (levelIndex - 2), 0);
        }*/

        SceneManager.LoadScene(levelIndex);
    }

    public void ResetHardcore()
    {
        //PlayerPrefs.SetInt("levelIsUnlocked", 1); // this enables continue button!
        PlayerPrefs.SetInt("_boatPosition", 0);
        PlayerPrefs.SetInt("HardcoreCrowns" + 0, 0);
        PlayerPrefs.SetInt("HardcoreDeaths" + 0, 0);
        PlayerPrefs.SetFloat("HardcoreTime" + 0, 0.0f);

        // tell the boat that it hasn't reached the end of its travel yet (even if it has),
        // so it can spawn and relocate to that position:
        PlayerPrefs.SetInt("_reachedEndOfTravel", 0);

        // reset previously collected shapes:
        foreach (int i in states)
        {
            PlayerPrefs.SetInt("HardcoreStates" + i, 0);
        }
    }

    public void ResetLevel()
    {

        // what does this do?
        if(levelIsUnlocked == 2)
        {
            //highscore.SafeLastStats();
        }

        PlayerPrefs.SetInt("levelIsUnlocked", 1); // this enables continue button!

        // tell the boat that it hasn't reached the end of its travel yet (even if it has),
        // so it can spawn and relocate to that position:
        PlayerPrefs.SetInt("_reachedEndOfTravel", 0);

        PlayerPrefs.SetInt("_cutScene_0_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_1_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_2_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_3_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_4_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_5_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_6_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_7_HasAlreadyPlayed", 0);
        PlayerPrefs.SetInt("_cutScene_8_HasAlreadyPlayed", 0); // not used
        PlayerPrefs.SetInt("_cutScene_9_HasAlreadyPlayed", 0); // not used
        PlayerPrefs.SetInt("_cutScene_10_HasAlreadyPlayed", 0); // not used

        PlayerPrefs.SetInt("_boatPosition", 0);
        
        foreach(int i in crowns)
        {
            PlayerPrefs.SetInt("crowns" + i, 0);

            //reset for collected Crowns
            char[] bits = PlayerPrefs.GetString("bitString" + (i)).ToCharArray();
            for (int j = 0; j < bits.Length; j++)
            {
                bits[j] = '1';
            }
            PlayerPrefs.SetString("bitString" + (i), bits.ArrayToString());
        }

        foreach (float i in timer)
        {
            PlayerPrefs.SetFloat("timer" + i, 0);
        }

        foreach(int i in deaths)
        {
            PlayerPrefs.SetInt("deaths" + i, 0);
        }

        foreach (int i in checkpoints)
        {
            PlayerPrefs.SetInt("lastCheckpoint" + i, 0);
        }

        foreach (int i in states)
        {
            PlayerPrefs.SetInt("State" + i, 0);
        }

        
        //PlayerPrefs.SetFloat("HardcoreTime", 0.0f);

        Start();
    }

    public void ContinueLevel()
    {
        SceneManager.LoadScene(levelIsUnlocked);
    }

    public void FastRestartHardcore()
    {
        //PlayerPrefs.SetInt("levelIsUnlocked", 1); // this enables continue button!
        PlayerPrefs.SetInt("_boatPosition", 0);
        PlayerPrefs.SetInt("HardcoreCrowns" + 0, 0);
        PlayerPrefs.SetFloat("HardcoreTime" + 0, 0.0f);

        foreach (int i in states)
        {
            PlayerPrefs.SetInt("HardcoreStates" + i, 0);
        }
    }

    /// <summary>
    /// This button-function allows to reset all stats and current progression in the main menu for testing (non-recoverable).
    /// Use with care and caution!
    /// </summary>
    public void ResetAllStats()
    {
        // reset current progression:
        ResetHardcore();
        ResetLevel();

        // disable the continue-button (normally this happens when finishing the game!)
        PlayerPrefs.SetInt("levelIsUnlocked", 2);

        // reset last regular runs progression:
        PlayerPrefs.SetFloat("Lasttimer" + (0), 0.0f);
        PlayerPrefs.SetInt("Lastcrowns" + (0), 0);
        PlayerPrefs.SetInt("Lastdeaths" + (0), 0);
    
        // reset all highscores:
        PlayerPrefs.SetFloat("HTtimer" + 0, 0.0f);
        PlayerPrefs.SetInt("HTcrowns" + 0, 0);
        PlayerPrefs.SetInt("HTdeaths" + 0, 0);

        PlayerPrefs.SetFloat("HCtimer" + 0, 0.0f);
        PlayerPrefs.SetInt("HCcrowns" + 0, 0);
        PlayerPrefs.SetInt("HCdeaths" + 0, 0);

        PlayerPrefs.SetFloat("HighscoreHardcoreCrowns_Time" + 0, 0.0f);
        PlayerPrefs.SetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0);
        PlayerPrefs.SetInt("HighscoreHardcoreCrowns_Deaths" + 0, 0);
            
        PlayerPrefs.SetFloat("HighscoreHardcoreTime_Time" + 0, 0.0f);
        PlayerPrefs.SetInt("HighscoreHardcoreTime_Crowns" + 0, 0);
        PlayerPrefs.SetInt("HighscoreHardcoreTime_Deaths" + 0, 0);

        FindObjectOfType<Highscore>().ResetAllHighscores();

        Start();
        Debug.Log("All highscores and progression have been reset!");
    }
}


