using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestyMcTestFace : MonoBehaviour
{
    /*
    //[SerializeField] string _filePath = "C:/Users/fmund/AppData/LocalLow/Lone Flower Games/Origami Lovers/InventoryData.json";
    [SerializeField] string _filePath = "xxx";
    // Start is called before the first frame update
    void Start()
    {
        /*
        float _savedHcTime = PlayerPrefs.GetFloat("HCtimer" + 0, 0.0f);
        Debug.Log("HCtimer is: " + _savedHcTime);

        float _currentRunTime = PlayerPrefs.GetFloat("timer" + 0, 0.0f);
        Debug.Log("current time is: " + _currentRunTime);

    }

    public ListOfSpeedHighscores _listOfSpeedHighscores = new ListOfSpeedHighscores();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveToJson();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJson();
        }
    }

    public void SaveToJson()
    {
        string _speedHighscores = JsonUtility.ToJson(_listOfSpeedHighscores);
        string filePath = Application.persistentDataPath + "/InventoryData.json"; // this could be anything "x" with ".json" in the end!
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, _speedHighscores);
        Debug.Log("savefile has been created");
    }

    public void LoadFromJson()
    {
        //string filePath = Application.persistentDataPath + "/InventoryData.json"; // this could be anything "x" with ".json" in the end!

        //string filePath = _filePath;
        string inventoryData = System.IO.File.ReadAllText(_filePath);

        _listOfSpeedHighscores = JsonUtility.FromJson<ListOfSpeedHighscores>(inventoryData);
        Debug.Log("Load Complete!");
    }
}


[System.Serializable]
public class ListOfSpeedHighscores
{
    public string _name;
    public stromg isFulL;
    public List<HighscoreEntry> items = new List<HighscoreEntry>();
}

[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public string description;
}*/

}