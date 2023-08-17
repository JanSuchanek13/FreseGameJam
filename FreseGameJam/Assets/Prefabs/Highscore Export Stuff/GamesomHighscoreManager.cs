using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;


public class GamesomHighscoreManager : MonoBehaviour
{
    private const int _maxHighscores = 5;
    [Tooltip("1 = Donnerstag, 2 = Freitag, 3 = Samstag, 4 = Sonntag")]
    [SerializeField] private int currentDay = 1; // set this in the editor to your current day

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

    private HashSet<string> _gamescomEmails = new HashSet<string>(); // HashSet ensures unique emails

    private void Start()
    {
        // instructions:
        Debug.Log("Homeskillets!!! copy this:    " + Application.persistentDataPath + "/SpeedHighscoreData.json     into the file-path (speed) section of the HighscoreDataGatherer-script in the display app");
        Debug.Log("And copy this:    " + Application.persistentDataPath + "/CrownsHighscoreData.json     into the file-path (speed) section of the HighscoreDataGatherer-script in the display app");

        Debug.Log("For this to work you have to first create a default save-file by pressing S while the game is running");
        Debug.Log("Now hit play in the display app!");

        if (PlayerPrefs.GetInt("JsonCreated", 0) == 0) // this never needs to be reset.
        {
            SetListsToDefault();

            // avoid saving this twice:
            PlayerPrefs.SetInt("JsonCreated", 1);
        }

        LoadFromJson();
    }

    public void RegisterRun(string playerName, string playerEmail, float timeTaken, int crownsCollected)
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
        switch (currentDay)
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

    // TEST!!!!
    public void TestInput()
    {
        string[] randomNamesDay1 = { "Alice", "Bob", "Charlie", "Dave", "Ella", "Finn", "Grace" };
        string[] randomNamesDay2 = { "Hugo", "Iris", "Jake", "Kara", "Liam", "Mia", "Nate" };
        System.Random random = new System.Random();

        // Day 1 entries
        currentDay = 1;
        for (int i = 0; i < 7; i++)
        {
            string playerName = randomNamesDay1[i];
            string playerEmail = "test@test.test";
            float timeTaken = (float)(random.NextDouble() * 10); // random float between 0 and 10
            int crownsCollected = random.Next(11); // random int between 0 and 5
            RegisterRun(playerName, playerEmail, timeTaken, crownsCollected);
        }

        // Day 2 entries
        currentDay = 2;
        for (int i = 0; i < 7; i++)
        {
            string playerName = randomNamesDay2[i];
            string playerEmail = "test2@test.test";
            float timeTaken = (float)(random.NextDouble() * 10); // random float between 0 and 10
            int crownsCollected = random.Next(11); // random int between 0 and 5
            RegisterRun(playerName, playerEmail, timeTaken, crownsCollected);
        }

        currentDay = 1; // reset, juuuust in case:
        Debug.Log("---- Entries Created ----");
    }

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

        string dayFourSpeed = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayThreeSpeedHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayThreeSpeedHighscores.json", dayFourSpeed);

        string dayFourCrowns = JsonUtility.ToJson(new SerializableList<HighscoreEntry>(_dayThreeCrownsHighscoreList));
        File.WriteAllText(Application.persistentDataPath + "/DayThreeCrownsHighscores.json", dayFourCrowns);
    }
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