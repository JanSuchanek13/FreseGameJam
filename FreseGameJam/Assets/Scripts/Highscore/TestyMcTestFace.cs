using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestyMcTestFace : MonoBehaviour
{

    //[SerializeField] string _filePath = "C:/Users/fmund/AppData/LocalLow/Lone Flower Games/Origami Lovers/InventoryData.json";
    [SerializeField] string _filePath = "xxx";
    // Start is called before the first frame update
    void Start()
    {
        /*
        float _savedHcTime = PlayerPrefs.GetFloat("HCtimer" + 0, 0.0f);
        Debug.Log("HCtimer is: " + _savedHcTime);

        float _currentRunTime = PlayerPrefs.GetFloat("timer" + 0, 0.0f);
        Debug.Log("current time is: " + _currentRunTime);*/

    }

    public Inventory inventory = new Inventory();

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
        string inventoryData = JsonUtility.ToJson(inventory);
        string filePath = Application.persistentDataPath + "/InventoryData.json"; // this could be anything "x" with ".json" in the end!
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, inventoryData);
        Debug.Log("savefile has been created");
    }

    public void LoadFromJson()
    {
        //string filePath = Application.persistentDataPath + "/InventoryData.json"; // this could be anything "x" with ".json" in the end!

        //string filePath = _filePath;
        string inventoryData = System.IO.File.ReadAllText(_filePath);

        inventory = JsonUtility.FromJson<Inventory>(inventoryData);
        Debug.Log("Load Complete!");
    }
}


[System.Serializable]
public class Inventory
{
    public int gold;
    public bool isFulL;
    public List<Items> items = new List<Items>();
}

[System.Serializable]
public class Items
{
    public string name;
    public string description;
}

