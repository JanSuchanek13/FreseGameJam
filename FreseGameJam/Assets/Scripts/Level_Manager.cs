using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{

    int levelIsUnlocked;


    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        levelIsUnlocked = PlayerPrefs.GetInt("levelIsUnlocked", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < levelIsUnlocked; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void ResetLevel()
    {
        PlayerPrefs.SetInt("levelIsUnlocked", 1);
        Start();
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


