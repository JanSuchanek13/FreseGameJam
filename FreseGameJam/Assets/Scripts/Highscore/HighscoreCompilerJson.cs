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

    int _typeOfHighscore;
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
            //SaveCrownsHighscoreToJson();
            
            // simulate achieving a new (speed) highscore
            NewHighscore(0);
        }

        /*if (Input.GetKeyDown(KeyCode.L))
        {
            //LoadCurrentHighscoresFromJson();
        }*/
    }

    /// <summary>
    /// This should be called by the Highscore-script whenever a new highscore is achieved,
    /// or an empty highscore-slot has been filled. 0 = speed | 1 = crowns | 2 = both
    /// </summary>
    /// <param name="speedHighscore"></param>
    public void NewHighscore(int typeOfHighscore)
    {
        Debug.Log("NewHighscore called!");

        _typeOfHighscore = typeOfHighscore;

        FindObjectOfType<ButtonFunction>().Pause(); // make UI interactable
        FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn off the pause menu-UI to give access to the permission-UI

        _givePermissionUI.SetActive(true);
    }

    /// <summary>
    /// This should be called by the "DONE" button after a player has entered their information in the UI.
    /// Collects all the players/highscores information and then saves it into a Json.
    /// </summary>
    public void CompileHighscoreData()
    {
        switch (_typeOfHighscore)
        {
            case 0: // new speed highscore:
                currentSpeedHighscores.speedName = _nameInputField.text;
                currentSpeedHighscores.speedEmail = _emailInputField.text;
                currentSpeedHighscores.speedTime = GetHighscoreTime();
                currentSpeedHighscores.speedCrowns = GetHighscoreCrowns();

                // sort the new highscore into the list of existing ones:
                //SortHighscores(_typeOfHighscore);

                // save info:
                SaveSpeedHighscoreToJson();
                break;

            case 1: // new crowns highscore:
                currentCrownHighscores.crownsName = _nameInputField.text;
                currentCrownHighscores.crownsEmail = _emailInputField.text;
                currentCrownHighscores.crownsTime = GetHighscoreTime();
                currentCrownHighscores.crownsCrowns = GetHighscoreCrowns();

                // sort the new highscore into the list of existing ones:
                //SortHighscores(_typeOfHighscore);

                // save info:
                SaveCrownsHighscoreToJson();
                break;

            case 2: // new double highscore:
                currentSpeedHighscores.speedName = _nameInputField.text;
                currentSpeedHighscores.speedEmail = _emailInputField.text;
                currentSpeedHighscores.speedTime = GetHighscoreTime();
                currentSpeedHighscores.speedCrowns = GetHighscoreCrowns();

                currentCrownHighscores.crownsName = _nameInputField.text;
                currentCrownHighscores.crownsEmail = _emailInputField.text;
                currentCrownHighscores.crownsTime = GetHighscoreTime();
                currentCrownHighscores.crownsCrowns = GetHighscoreCrowns();

                // sort the new highscore into the list of existing ones:
                //SortHighscores(_typeOfHighscore);

                // save info as both highscores:
                SaveSpeedHighscoreToJson();
                SaveCrownsHighscoreToJson();
                break;
        }

        // unpause the game: (since the pause-UI is inactive, this will unpause)
        FindObjectOfType<ButtonFunction>().Pause();
    }

    public void SaveSpeedHighscoreToJson()
    {
        string _currenSpeedHighscores = JsonUtility.ToJson(currentSpeedHighscores);
        string _filePath = Application.persistentDataPath + "/SpeedHighscoreData.json";
        System.IO.File.WriteAllText(_filePath, _currenSpeedHighscores);
        
        Debug.Log("speed-savefile has been created");
    }

    public void SaveCrownsHighscoreToJson()
    {
        string _currentCrownsHighscores = JsonUtility.ToJson(currentCrownHighscores);
        string _filePath = Application.persistentDataPath + "/CrownsHighscoreData.json";
        System.IO.File.WriteAllText(_filePath, _currentCrownsHighscores);

        Debug.Log("crowns-savefile has been created");
    }

    float GetHighscoreTime()
    {
        float _time = 0.0f;

        switch (_typeOfHighscore)
        {
            case 0:
                _time = PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0);
                break;

            case 1:
                _time = PlayerPrefs.GetFloat("HighscoreHardcoreCrowns_Time" + 0);
                break;

            case 2:
                _time = PlayerPrefs.GetFloat("HardcoreTime" + 0);// this should work, as it can only ever be called after a highscore has been achieved!
                break;
        }

        return _time;
    }

    int GetHighscoreCrowns()
    {
        int _crowns = 0;

        switch (_typeOfHighscore)
        {
            case 0:
                _crowns = PlayerPrefs.GetInt("HighscoreHardcoreTime_Crowns" + 0);
                break;

            case 1:
                _crowns = PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0);
                break;

            case 2:
                _crowns = PlayerPrefs.GetInt("HardcoreCrowns" + 0); // this should work, as it can only ever be called after a highscore has been achieved!
                break;
        }

        return _crowns;
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

[System.Serializable]
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
