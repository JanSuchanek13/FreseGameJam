using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreCompilerJson : MonoBehaviour
{
    [Tooltip("Enter the exact file path where the highscore-stats Json gets saved on the gaming-device here! (Exact means EXACT!)")]
    [SerializeField] string _filePath = "C:/Users/fmund/AppData/LocalLow/Lone Flower Games/Origami Lovers/HighscoreData.json";

    [Space(10)]
    [Tooltip("Day 1 is the 24th; day 2 is the 25th; day 3 is the 26th; day 4 is 27th. Switch at the start of every day!")]
    [SerializeField] int _day = 1;

    [Space(10)]
    [Tooltip("This determins the length of the intervals to check for new highscores. Adjust as needed.")]
    [SerializeField] float _updateInterval = 30.0f;

    [Space(30)]
    [Tooltip("DO NOT TOUCH:")]
    public HighscoreData currentHighscores = new HighscoreData(); // this contains the most recent highscores of the ongoing day.

    /*
    private void Start()
    {
        // Start checking the highscores at an interval:
        StartCoroutine("CheckData");

        // Load data of previous days:
        if (_day != 1)
        {
            //LoadPreviousDays();
        }
    }
    IEnumerator CheckData()
    {
        LoadCurrentHighscoresFromJson();

        yield return new WaitForSeconds(_updateInterval);

        StartCoroutine("CheckData");
    }

    public void LoadCurrentHighscoresFromJson()
    {
        string highscoreData = System.IO.File.ReadAllText(_filePath);
        currentHighscores = JsonUtility.FromJson<HighscoreData>(highscoreData);

        // testing:
        Debug.Log("Load Complete!");
    }*/
    //TESTING!
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveToJson();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //LoadCurrentHighscoresFromJson();
        }
    }

    public void SaveToJson()
    {
        string _currentHighscores = JsonUtility.ToJson(currentHighscores);
        string _filePath = Application.persistentDataPath + "/HighscoreData.json"; // this could be anything "x" with ".json" in the end!
        Debug.Log(_filePath);
        System.IO.File.WriteAllText(_filePath, _currentHighscores);
        Debug.Log("savefile has been created");
    }
}

[System.Serializable]
public class HighscoreData
{
    [Header("Speed Highscore:")]
    public string speedName;
    public string speedEmail;
    public float speedTime;
    public int speedCrowns;

    [Space(10)]
    [Header("Crowns Highscore:")]
    public string crownsName;
    public string crownsEmail;
    public float crownsTime;
    public int crownsCrowns;
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
