using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Level_Manager : MonoBehaviour
{

    int levelIsUnlocked;
    public float[] timer = new float[3];
    
    public int[] crowns = new int[3];

    public int[] deaths = new int[3];


    public Button[] buttons;
    public TextMeshProUGUI[] CrownCounters;
    public TextMeshProUGUI[] TimeCounters;
    public TextMeshProUGUI[] DeathCounters;

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



        //crown Counter
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        /*
        crowns[0] = PlayerPrefs.GetInt("crowns", 0);
        crowns[1] = PlayerPrefs.GetInt("crowns", 0);
        crowns[2] = PlayerPrefs.GetInt("crowns", 0);
        */
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            crowns[i] = PlayerPrefs.GetInt("crowns"+ i, 1);
            CrownCounters[i].text = crowns[i].ToString();
        }



        //Timer
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            timer[i] = PlayerPrefs.GetFloat("timer" + i, 0);
            TimeCounters[i].text = timer[i].ToString();
        }

        //Death
        for (int i = 0; i < levelIsUnlocked; i++)
        {
            deaths[i] = PlayerPrefs.GetInt("deaths" + i, 0);
            DeathCounters[i].text = timer[i].ToString();
        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        PlayerPrefs.SetInt("crowns" + (levelIndex - 2), 0);
    }

    public void ResetLevel()
    {
        PlayerPrefs.SetInt("levelIsUnlocked", 1);
        Start();
        foreach(int i in crowns)
        {
            PlayerPrefs.SetInt("crowns" + i, 0);
        }
        foreach (float i in timer)
        {
            PlayerPrefs.SetFloat("timer" + i, 0);
        }
        foreach(int i in deaths)
        {
            PlayerPrefs.SetInt("deaths" + i, 0);
        }
    }

    public void ContinueLevel()
    {
        SceneManager.LoadScene(levelIsUnlocked + 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}


