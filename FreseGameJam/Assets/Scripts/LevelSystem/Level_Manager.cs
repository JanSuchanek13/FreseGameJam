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

    // Felix add:
    //[SerializeField] GameObject immersePlayerIntoWorldKit;

    // Start is called before the first frame update
    void Start()
    {
        //Level buttons
        levelIsUnlocked = PlayerPrefs.GetInt("levelIsUnlocked", 1);
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < levelIsUnlocked; i++)
        {
            buttons[i].interactable = true;
        }

        if (levelIsUnlocked > 1)
        {
            continueButton.interactable = false;
        }

        //crown Counter
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        /*
        crowns[0] = PlayerPrefs.GetInt("crowns", 0);
        crowns[1] = PlayerPrefs.GetInt("crowns", 0);
        crowns[2] = PlayerPrefs.GetInt("crowns", 0);
        */
        for (int i = 0; i < levelIsUnlocked; i++)   // shows only stats for unlocked levels
        {
            crowns[i] = PlayerPrefs.GetInt("crowns"+ i, 1);
            if (i < numberOfLevels)     // fixes error cause only 1 Level is shown
            {
                CrownCounters[i].text = crowns[i].ToString();
                HS_CrownCounters[i].text = crowns[i].ToString();
            }
            
        }



        //Timer
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            timer[i] = PlayerPrefs.GetFloat("timer" + i, 0);

            //FelixBeginn:
            //experiment
            /*public string FormatTime( float time )
            {
                int minutes = (int) time / 60 ;
                int seconds = (int) time - 60 * minutes;
                int milliseconds = (int) (1000 * (time - minutes * 60 - seconds));
                return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds );
            }
            void Start()
            {
                Debug.Log( FormatTime( 79.230 ) ) ; // Outputs 01:19:230
            }*/
            //Debug.Log(timer[i].ToString()); // this should show jans number
            int minutes = (int)timer[i] / 60;
            int seconds = (int)timer[i] - 60 * minutes;
            int milliseconds = (int)(1000 * (timer[i] - minutes * 60 - seconds));
            if (i < numberOfLevels)
            {
                TimeCounters[i].text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
                HS_TimeCounters[i].text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds); // to display current stats in Highscore (HS) interface of start screen
            }
            
            //Debug.Log(timer[i].ToString()); // this should STILL show jans number
            //FelixEnd:
            
            //TimeCounters[i].text = timer[i].ToString(); //Jans original
        }

        //Death
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            deaths[i] = PlayerPrefs.GetInt("deaths" + i, 0);
            if (i < numberOfLevels)
            {
                DeathCounters[i].text = deaths[i].ToString();
                HS_DeathCounters[i].text = deaths[i].ToString();
            }
            
        }
    }

    public void LoadLevel(int levelIndex)   //not in use
    {
        SceneManager.LoadScene(levelIndex);
        PlayerPrefs.SetInt("crowns" + (levelIndex - 2), 0);

        PlayerPrefs.SetInt("_reachedEndOfTravel", 0);
    }

    public void ResetLevel()
    {
        highscore.SafeLastStats();
        
        PlayerPrefs.SetInt("levelIsUnlocked", 1);
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
            //PlayerPrefs.SetFloat("lastTimer" + i, 0);
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

        PlayerPrefs.SetFloat("HardcoreTime", 0.0f);

        Start();
    }

    public void ContinueLevel()
    {
        SceneManager.LoadScene(levelIsUnlocked);

        //Felix added for juice:
        //GetComponent<PlayWasPressed>().ImmersePlayer(1);
           // immersePlayerIntoWorldKit.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}


