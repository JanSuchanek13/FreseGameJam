using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using TMPro;
using System;


public class GamesomHighscoreManager : MonoBehaviour
{
    //private const int _maxHighscores = 5;
    [Header("Day Settings:")]
    [Tooltip("1 = Mittwoch, 2 = Donnerstag, 3 = Freitag, 4 = Samstag, 5 = Sonntag")]
    [SerializeField] private int _currentDay = 0; // set this in the editor to your current day
    [SerializeField] GameObject _overwriteDayUI;
    [SerializeField] TMP_Dropdown _overwriteDayInputField;
    private DateTime _startDate = new DateTime(2023, 08, 23);

    [Header("UI-Settings for data-entry:")]
    [Tooltip("How many characters/letters can a player use for her/his name?")]
    [SerializeField] int _allowedNrOfCharacters = 10;
    [SerializeField] GameObject _givePermissionUI;
    [SerializeField] TextMeshProUGUI _permissionUIHeader;

    [Space(10)]
    [SerializeField] TMP_InputField _nameInputField;
    [SerializeField] TMP_InputField _emailInputField;
    [SerializeField] TextMeshProUGUI _timeDisplay;
    [SerializeField] TextMeshProUGUI _crownsDisplay;

    private List<HighscoreEntry> _overallSpeedHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _overallCrownsHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayOneSpeedHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayOneCrownsHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayTwoSpeedHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayTwoCrownsHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayThreeSpeedHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayThreeCrownsHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayFourSpeedHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayFourCrownsHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayFiveSpeedHighscoreList = new List<HighscoreEntry>();
    private List<HighscoreEntry> _dayFiveCrownsHighscoreList = new List<HighscoreEntry>();

    private HashSet<string> _gamescomEmails = new HashSet<string>(); // HashSet ensures unique emails

    private void Start()
    {
        _nameInputField.characterLimit = _allowedNrOfCharacters;
        // instructions:
        Debug.Log("Moin! copy this:    " + Application.persistentDataPath + "into the file-path section of the HighscoreDataGatherer-script in the display app");
        Debug.Log("Hit play in the game, open the pause menu and hit the Reset button, now hit play in the app --> all entries should be default entries and 0 data");
        Debug.Log("Now open the pause menu again and hit the CreateRandomEntries button, now 14 rng-chars should populate the highscore lists of day 1 and 2 and overall!");
        Debug.Log("Ctrl+Shift+2 will toggle the UI that allows manual overwrite of the day-id. " +
            "NOTE: This is done automatically too, so first check if the correct day is not already selected (read here in log)");

        if (_currentDay == 0)
        {
            SetDayAutomatically();
        }

        if (PlayerPrefs.GetInt("JsonCreated", 0) == 0) // this never needs to be reset.
        {
            SetListsToDefault();

            // avoid saving this twice:
            PlayerPrefs.SetInt("JsonCreated", 1);
        }

        LoadFromJson();
    }

    #region Set Day ID automatically AND/OR manually:
    void SetDayAutomatically()
    {
        DateTime now = DateTime.Now;
        TimeSpan durationSinceStart = now - _startDate;
        int daysSinceStart = durationSinceStart.Days;

        if (daysSinceStart >= 0) // ensure we're not before the start date
        {
            _currentDay = daysSinceStart + 1; // +1 because if it's the start date, it should be day 1, not day 0
            if (_currentDay > 5) // 5 days event
            {
                _currentDay = 5;
            }
        }
        else // the event hasn't started yet
        {
            _currentDay = 1;
        }

        Debug.Log("Current day: " + _currentDay + ", if this is incorrect, manually overwrite!");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleManualDayOverwrite();
        }
    }
    private void ToggleManualDayOverwrite()
    {
        if (!_overwriteDayUI.activeInHierarchy)
        {
            FindObjectOfType<ButtonFunction>().Pause(); // make UI interactable
            FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn off the pause menu-UI to give access to the permission-UI

            _overwriteDayUI.SetActive(true);
        }
        else
        {
            _overwriteDayUI.SetActive(false);
            
            FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn on the pause menu-UI to give allow PAUSE to close the screen
            FindObjectOfType<ButtonFunction>().Pause(); // make UI interactable
        }
    }
    public void SetDateManually()
    {
        _currentDay = _overwriteDayInputField.value + 1; // accounting for the value starting at 0!
        Debug.Log("current day has been manually overwritten by: " + _currentDay);

        FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn on the pause menu-UI to give allow PAUSE to close the screen
        FindObjectOfType<ButtonFunction>().Pause(); // make UI interactable
    }
#endregion

    public enum HighscoreStatus
    {
        None,
        SpeedOverall,
        CrownsOverall,
        DoubleOverall,
        SpeedDayTop5,
        CrownsDayTop5,
        DoubleDayTop5
    }

    /// <summary>
    /// Call this function when a hardcore run was finished at the gamescom 2023.
    /// </summary>
    public void GamescomHardcoreRunFinished()
    {
        float _rawTime = PlayerPrefs.GetFloat("HardcoreTime" + 0, 0.0f);
        int _crowns = PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0);
        HighscoreStatus highscoreStatus = CheckForHighscoreAchieved(_rawTime, _crowns);

        if (highscoreStatus != HighscoreStatus.None)
        {
            // Set the UI header based on the highscore status
            switch (highscoreStatus)
            {
                case HighscoreStatus.SpeedOverall:
                    _permissionUIHeader.text = "SPEED HIGHSCORE!";
                    break;
                case HighscoreStatus.CrownsOverall:
                    _permissionUIHeader.text = "CROWN HIGHSCORE!";
                    break;
                case HighscoreStatus.DoubleOverall:
                    _permissionUIHeader.text = "DOUBLE HIGHSCORE!";
                    break;
                case HighscoreStatus.SpeedDayTop5:
                    _permissionUIHeader.text = "Top 5 Speed!";
                    break;
                case HighscoreStatus.CrownsDayTop5:
                    _permissionUIHeader.text = "Top 5 Crowns!";
                    break;
                case HighscoreStatus.DoubleDayTop5:
                    _permissionUIHeader.text = "Top 5 Speed AND Crowns!";
                    break;
                default:
                    _permissionUIHeader.text = "";  // Default empty just in case
                    break;
            }

            FindObjectOfType<ButtonFunction>().Pause(); // make UI interactable
            FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn off the pause menu-UI to give access to the permission-UI

            int minutes = (int)_rawTime / 60;
            int seconds = (int)_rawTime - 60 * minutes;
            int milliseconds = (int)(1000 * (_rawTime - minutes * 60 - seconds));
            _timeDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            _crownsDisplay.text = _crowns.ToString();

            _givePermissionUI.SetActive(true);
        }
        else // No highscore achieved:
        {
            FindObjectOfType<LevelScript>().AbortHighscoreCheck(); // this could be ambiguous as we have more than one instance in the level!
            Debug.Log("No highscore achieved this run.");
        }
    }
    private HighscoreStatus CheckForHighscoreAchieved(float time, int crowns)
    {
        bool isSpeedHighscoreOverall = _overallSpeedHighscoreList.Count < 5 || _overallSpeedHighscoreList[4].time == 0 || time < _overallSpeedHighscoreList[4].time;
        bool isCrownsHighscoreOverall = _overallCrownsHighscoreList.Count < 5 || _overallCrownsHighscoreList[4].crowns == 0 || crowns > _overallCrownsHighscoreList[4].crowns;

        if (isSpeedHighscoreOverall && isCrownsHighscoreOverall)
            return HighscoreStatus.DoubleOverall;
        if (isSpeedHighscoreOverall)
            return HighscoreStatus.SpeedOverall;
        if (isCrownsHighscoreOverall)
            return HighscoreStatus.CrownsOverall;

        // Existing Day Check Logic
        List<HighscoreEntry> speedList;
        List<HighscoreEntry> crownsList;

        switch (_currentDay)
        {
            case 1:
                speedList = _dayOneSpeedHighscoreList;
                crownsList = _dayOneCrownsHighscoreList;
                break;

            case 2:
                speedList = _dayTwoSpeedHighscoreList;
                crownsList = _dayTwoCrownsHighscoreList;
                break;

            case 3:
                speedList = _dayThreeSpeedHighscoreList;
                crownsList = _dayThreeCrownsHighscoreList;
                break;

            case 4:
                speedList = _dayFourSpeedHighscoreList;
                crownsList = _dayFourCrownsHighscoreList;
                break;

            case 5:
                speedList = _dayFiveSpeedHighscoreList;
                crownsList = _dayFiveCrownsHighscoreList;
                break;

            default:
                Debug.Log("Error! Wrong day selected!");
                speedList = _dayOneSpeedHighscoreList;
                crownsList = _dayOneCrownsHighscoreList;
                break;
        }

        bool isSpeedHighscoreDay = speedList.Count < 5 || speedList[4].time == 0 || time < speedList[4].time;
        bool isCrownsHighscoreDay = crownsList.Count < 5 || crownsList[4].crowns == 0 || crowns > crownsList[4].crowns;

        if (isSpeedHighscoreDay && isCrownsHighscoreDay)
            return HighscoreStatus.DoubleDayTop5;
        if (isSpeedHighscoreDay)
            return HighscoreStatus.SpeedDayTop5;
        if (isCrownsHighscoreDay)
            return HighscoreStatus.CrownsDayTop5;

        return HighscoreStatus.None;
    }

    /// <summary>
    /// Call this on the "No"-option of the data-entry UI in case someone does not wish to have their data collected.
    /// This will delete the results of their run! Beware!
    /// </summary>
    public void PermissionNotGranted()
    {
        FindObjectOfType<LevelScript>().AbortHighscoreCheck(); // this could be ambiguous as we have more than one instance in the level!

        // unpause the game: (since the pause-UI is inactive, this will unpause)
        FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn onthe pause menu-UI to give access to the permission-UI
        FindObjectOfType<ButtonFunction>().Pause();
    }

    /// <summary>
    /// Call this function when a player is done entering his/her data into the intake-sheet.
    /// Note, collecting this data is only possible after they have agreed to us using their info!
    /// This is a Button--function.
    /// </summary>
    public void DoneEnteringData()
    {
        string _name = _nameInputField.text;
        string _email = _emailInputField.text;
        float _time = PlayerPrefs.GetFloat("HardcoreTime" + 0, 0.0f);
        int _crowns = PlayerPrefs.GetInt("HardcoreCrowns" + 0);

        RegisterRun(_name, _email, _time, _crowns);

        // unpause the game: (since the pause-UI is inactive, this will unpause)
        FindObjectOfType<ButtonFunction>().TogglePauseScreen(); // turn onthe pause menu-UI to give access to the permission-UI
        FindObjectOfType<ButtonFunction>().Pause();

        // finish the wrap-up and go back to main menu (also enter potential highscore in the actual game):
        FindObjectOfType<LevelScript>().LevelFinished();
    } 

    private void RegisterRun(string playerName, string playerEmail, float timeTaken, int crownsCollected)
    {
        HighscoreEntry newEntry = new HighscoreEntry
        {
            name = playerName,
            email = playerEmail,
            time = timeTaken,
            crowns = crownsCollected
        };

        // Update Overall Lists
        UpdateList(_overallSpeedHighscoreList, newEntry, e => e.time, e => -e.crowns);
        UpdateList(_overallCrownsHighscoreList, newEntry, e => -e.crowns, e => e.time);

        // Update Day-specific Lists
        switch (_currentDay)
        {
            case 1:
                UpdateList(_dayOneSpeedHighscoreList, newEntry, e => e.time, e => -e.crowns);
                UpdateList(_dayOneCrownsHighscoreList, newEntry, e => -e.crowns, e => e.time);
                break;
            case 2:
                UpdateList(_dayTwoSpeedHighscoreList, newEntry, e => e.time, e => -e.crowns);
                UpdateList(_dayTwoCrownsHighscoreList, newEntry, e => -e.crowns, e => e.time);
                break;
            case 3:
                UpdateList(_dayThreeSpeedHighscoreList, newEntry, e => e.time, e => -e.crowns);
                UpdateList(_dayThreeCrownsHighscoreList, newEntry, e => -e.crowns, e => e.time);
                break;
            case 4:
                UpdateList(_dayFourSpeedHighscoreList, newEntry, e => e.time, e => -e.crowns);
                UpdateList(_dayFourCrownsHighscoreList, newEntry, e => -e.crowns, e => e.time);
                break;
            case 5:
                UpdateList(_dayFiveSpeedHighscoreList, newEntry, e => e.time, e => -e.crowns);
                UpdateList(_dayFiveCrownsHighscoreList, newEntry, e => -e.crowns, e => e.time);
                break;
        }

        // Add email to set
        _gamescomEmails.Add(playerEmail);

        // Save the data to JSON after every update
        SaveToJson();
    }

    private void UpdateList(List<HighscoreEntry> list, HighscoreEntry entry, params System.Func<HighscoreEntry, object>[] orderBy)
    {
        list.Add(entry);

        int currentIndex = list.Count - 1;  // Index of the recently added entry

        while (currentIndex > 0 && CompareEntries(list[currentIndex], list[currentIndex - 1], orderBy) < 0)
        {
            // Swap entries
            HighscoreEntry temp = list[currentIndex];
            list[currentIndex] = list[currentIndex - 1];
            list[currentIndex - 1] = temp;

            currentIndex--;
        }
    }

    private int CompareEntries(HighscoreEntry e1, HighscoreEntry e2, System.Func<HighscoreEntry, object>[] criteria)
    {
        // Treat entries with a time of 0 as non-existent
        if (e1.time == 0) return 1;
        if (e2.time == 0) return -1;

        foreach (var criterion in criteria)
        {
            int comparison = Comparer<object>.Default.Compare(criterion(e1), criterion(e2));
            if (comparison != 0) return comparison;
        }
        return 1;  // If all criteria matched, e1 should be ranked below e2
    }

    private void SaveToJson()
    {
        // Saving all lists
        SaveListToJson(_overallSpeedHighscoreList, "/OverallSpeedHighscores.json");
        SaveListToJson(_overallCrownsHighscoreList, "/OverallCrownsHighscores.json");

        SaveListToJson(_dayOneSpeedHighscoreList, "/DayOneSpeedHighscores.json");
        SaveListToJson(_dayOneCrownsHighscoreList, "/DayOneCrownsHighscores.json");

        SaveListToJson(_dayTwoSpeedHighscoreList, "/DayTwoSpeedHighscores.json");
        SaveListToJson(_dayTwoCrownsHighscoreList, "/DayTwoCrownsHighscores.json");

        SaveListToJson(_dayThreeSpeedHighscoreList, "/DayThreeSpeedHighscores.json");
        SaveListToJson(_dayThreeCrownsHighscoreList, "/DayThreeCrownsHighscores.json");

        SaveListToJson(_dayFourSpeedHighscoreList, "/DayFourSpeedHighscores.json");
        SaveListToJson(_dayFourCrownsHighscoreList, "/DayFourCrownsHighscores.json");

        SaveListToJson(_dayFiveSpeedHighscoreList, "/DayFiveSpeedHighscores.json");
        SaveListToJson(_dayFiveCrownsHighscoreList, "/DayFiveCrownsHighscores.json");

        // Saving emails
        string emailFilePath = Application.persistentDataPath + "/GamescomEmails.json";
        File.WriteAllText(emailFilePath, JsonUtility.ToJson(new SerializableList<string>(_gamescomEmails.ToList())));
    }

    private void LoadFromJson()
    {
        // Loading all lists
        _overallSpeedHighscoreList = LoadListFromJson("/OverallSpeedHighscores.json");
        _overallCrownsHighscoreList = LoadListFromJson("/OverallCrownsHighscores.json");

        _dayOneSpeedHighscoreList = LoadListFromJson("/DayOneSpeedHighscores.json");
        _dayOneCrownsHighscoreList = LoadListFromJson("/DayOneCrownsHighscores.json");

        _dayTwoSpeedHighscoreList = LoadListFromJson("/DayTwoSpeedHighscores.json");
        _dayTwoCrownsHighscoreList = LoadListFromJson("/DayTwoCrownsHighscores.json");

        _dayThreeSpeedHighscoreList = LoadListFromJson("/DayThreeSpeedHighscores.json");
        _dayThreeCrownsHighscoreList = LoadListFromJson("/DayThreeCrownsHighscores.json");

        _dayFourSpeedHighscoreList = LoadListFromJson("/DayFourSpeedHighscores.json");
        _dayFourCrownsHighscoreList = LoadListFromJson("/DayFourCrownsHighscores.json");

        _dayFiveSpeedHighscoreList = LoadListFromJson("/DayFiveSpeedHighscores.json");
        _dayFiveCrownsHighscoreList = LoadListFromJson("/DayFiveCrownsHighscores.json");

        // Loading emails
        string emailFilePath = Application.persistentDataPath + "/GamescomEmails.json";
        if (File.Exists(emailFilePath))
        {
            string jsonData = File.ReadAllText(emailFilePath);
            _gamescomEmails = new HashSet<string>(JsonUtility.FromJson<SerializableList<string>>(jsonData).items);
        }
    }

    private List<HighscoreEntry> LoadListFromJson(string filePath)
    {
        string fullPath = Application.persistentDataPath + filePath;
        if (File.Exists(fullPath))
        {
            string jsonData = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<SerializableList<HighscoreEntry>>(jsonData).items;
        }

        return new List<HighscoreEntry>();
    }

    private void SaveListToJson(List<HighscoreEntry> list, string filePath)
    {
        string jsonData = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(list));
        string fullPath = Application.persistentDataPath + filePath;
        File.WriteAllText(fullPath, jsonData);
    }

    #region Testing:
    /// <summary>
    /// Create several fake entries over two days to check functionality and list-sorting.
    /// Using low possible values helps test sorting at close proximity.
    /// This is a Button--function.
    /// </summary>
    public void TestInput()
    {
        string[] randomNamesDay1 = { "Alice", "Bob", "Charlie", "Dave", "Ella", "Finn", "Grace" };
        string[] randomNamesDay2 = { "Hugo", "Iris", "Jake", "Kara", "Liam", "Mia", "Nate" };
        System.Random random = new System.Random();

        // Day 1 entries
        _currentDay = 1;
        for (int i = 0; i < 10; i++)
        {
            string playerName = randomNamesDay1[i];
            string playerEmail = "test@test.test";
            float timeTaken = (float)(random.NextDouble() * 6); // random float between 0 and 5
            int crownsCollected = random.Next(6); // random int between 0 and 5
            RegisterRun(playerName, playerEmail, timeTaken, crownsCollected);
        }

        // Day 2 entries
        _currentDay = 2;
        for (int i = 0; i < 10; i++)
        {
            string playerName = randomNamesDay2[i];
            string playerEmail = "test2@test.test";
            float timeTaken = (float)(random.NextDouble() * 6); // random float between 0 and 5
            int crownsCollected = random.Next(6); // random int between 0 and 5
            RegisterRun(playerName, playerEmail, timeTaken, crownsCollected);
        }

        _currentDay = 1; // reset, juuuust in case:
        Debug.Log("---- Entries Created ----");
    }

    /// <summary>
    /// This function creates "default" entries for all lists so that the display-app has data to use and display.
    /// Even if nobody has played hardcore yet.
    /// This is a Button--function.
    /// </summary>
    public void SetListsToDefault()
    {
        HighscoreEntry defaultEntry = new HighscoreEntry
        {
            name = "[name]",
            email = "test@test.test",
            time = 0,
            crowns = 0
        };

        List<HighscoreEntry> defaultList = Enumerable.Repeat(defaultEntry, 5).ToList();

        _overallSpeedHighscoreList = new List<HighscoreEntry>(defaultList);
        _overallCrownsHighscoreList = new List<HighscoreEntry>(defaultList);

        _dayOneSpeedHighscoreList = new List<HighscoreEntry>(defaultList);
        _dayOneCrownsHighscoreList = new List<HighscoreEntry>(defaultList);

        _dayTwoSpeedHighscoreList = new List<HighscoreEntry>(defaultList);
        _dayTwoCrownsHighscoreList = new List<HighscoreEntry>(defaultList);

        _dayThreeSpeedHighscoreList = new List<HighscoreEntry>(defaultList);
        _dayThreeCrownsHighscoreList = new List<HighscoreEntry>(defaultList);

        _dayFourSpeedHighscoreList = new List<HighscoreEntry>(defaultList);
        _dayFourCrownsHighscoreList = new List<HighscoreEntry>(defaultList);

        _dayFiveSpeedHighscoreList = new List<HighscoreEntry>(defaultList);
        _dayFiveCrownsHighscoreList = new List<HighscoreEntry>(defaultList);

        // Save to JSON, so changes are reflected in saved files
        SaveAllToJSON();
    }

    private void SaveAllToJSON()
    {
        // Save each list to its respective JSON file
        string speedHighscores = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_overallSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/OverallSpeedHighscores.json", speedHighscores);
        string crownsHighscores = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_overallCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/OverallCrownsHighscores.json", crownsHighscores);

        string dayOneSpeed = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayOneSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayOneSpeedHighscores.json", dayOneSpeed);
        string dayOneCrowns = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayOneCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayOneCrownsHighscores.json", dayOneCrowns);

        string dayTwoSpeed = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayTwoSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayTwoSpeedHighscores.json", dayTwoSpeed);
        string dayTwoCrowns = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayTwoCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayTwoCrownsHighscores.json", dayTwoCrowns);

        string dayThreeSpeed = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayThreeSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayThreeSpeedHighscores.json", dayThreeSpeed);
        string dayThreeCrowns = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayThreeCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayThreeCrownsHighscores.json", dayThreeCrowns);

        string dayFourSpeed = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayFourSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayFourSpeedHighscores.json", dayFourSpeed);
        string dayFourCrowns = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayFourCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayFourCrownsHighscores.json", dayFourCrowns);

        string dayFiveSpeed = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayFiveSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayFiveSpeedHighscores.json", dayFiveSpeed);
        string dayFiveCrowns = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayFiveCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayFiveCrownsHighscores.json", dayFiveCrowns);
    }
    #endregion
}

[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public string email;
    public float time;
    public int crowns;
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> items;

    public SerializableList(List<T> items)
    {
        this.items = items;
    }
}