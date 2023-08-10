using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreCompilerJson : MonoBehaviour
{
    /*
    [Tooltip("Enter the exact file path where the highscore-stats Json gets saved on the gaming-device here! (Exact means EXACT!)")]
    [SerializeField] string _filePath = "xxx";

    [Space(10)]
    [Tooltip("Day 1 is the 24th; day 2 is the 25th; day 3 is the 26th; day 4 is 27th. Switch at the start of every day!")]
    [SerializeField] int _day = 1;

    [Space(10)]
    [Tooltip("This determins the length of the intervals to check for new highscores. Adjust as needed.")]
    [SerializeField] float _updateInterval = 30.0f;
    */
    //[Space(30)]
    [SerializeField] GameObject _givePermissionUI;

    [Space(10)]
    [SerializeField] TMP_InputField _nameInputField;
    [SerializeField] TMP_InputField _emailInputField;
    float _highscoreTime;
    int _highscoreCrowns;
    Highscore _highscoreScript;

    [Tooltip("DO NOT TOUCH:")]
    public SpeedHighscoreData currentSpeedHighscores = new SpeedHighscoreData();
    public CrownsHighscoreData currentCrownHighscores = new CrownsHighscoreData();

    private void Start()
    {
        // instructions:
        Debug.Log("Homeskillets!!! copy this:    " + Application.persistentDataPath + "/HighscoreData.json     into the file-path section of the HighscoreDataGatherer-script in the display app");
        Debug.Log("For this to work you have to first create a default save-file by pressing S while the game is running");
        Debug.Log("Now hit play in the display app!");

        // setup variables:
        _highscoreScript = FindObjectOfType<Highscore>();
    }

    //TESTING!
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveCrownsHighscoreToJson();
        }

        /*if (Input.GetKeyDown(KeyCode.L))
        {
            //LoadCurrentHighscoresFromJson();
        }*/
    }

    public void NewHighscore()
    {
        _givePermissionUI.SetActive(true);
    }

    public void SaveSpeedHighscoreToJson()
    {
        GetHighscoreTime(true);
        GetHighscoreCrowns(true);

        string _currenSpeedHighscores = JsonUtility.ToJson(currentSpeedHighscores);
        string _filePath = Application.persistentDataPath + "/SpeedHighscoreData.json";
        System.IO.File.WriteAllText(_filePath, _currenSpeedHighscores);
        
        Debug.Log("speed-savefile has been created");
    }

    public void SaveCrownsHighscoreToJson()
    {
        GetHighscoreTime(false);
        GetHighscoreCrowns(false);

        string _currentCrownsHighscores = JsonUtility.ToJson(currentCrownHighscores);
        string _filePath = Application.persistentDataPath + "/CrownsHighscoreData.json";
        System.IO.File.WriteAllText(_filePath, _currentCrownsHighscores);

        Debug.Log("crowns-savefile has been created");
    }

    void GetHighscoreTime(bool _speedHighscore)
    {
        //_highscoreScript
    }

    void GetHighscoreCrowns(bool _speedHighscore)
    {

    }
}

[System.Serializable]
public class SpeedHighscoreData
{
    [Header("Speed Highscore:")]
    public string speedName;
    public string speedEmail;
    public float speedTime;
    public int speedCrowns;

    //public List<IndividualEntry> individualEntries = new List<IndividualEntry>();
}

public class CrownsHighscoreData
{
    [Header("Crowns Highscore:")]
    public string crownsName;
    public string crownsEmail;
    public float crownsTime;
    public int crownsCrowns;

    //public List<IndividualEntry> individualEntries = new List<IndividualEntry>();
}

/// <summary>
/// Use this to save the daily data (best-of-day) in a list of entries which gets sorted by 
/// </summary>
[System.Serializable]
public class DailyData
{
    public List<IndividualEntry> individualEntries = new List<IndividualEntry>();
}

[System.Serializable]
public class IndividualEntry
{
    public string name;
    public string email;

    public float time;
    public int crowns;
}
