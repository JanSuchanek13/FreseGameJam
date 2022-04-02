using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    float[] timer = new float[3];

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Pass();
        }
    }


    public void Pass()
    {
        


        //safe level
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel >= PlayerPrefs.GetInt("levelIsUnlocked"))
        {
            PlayerPrefs.SetInt("levelIsUnlocked", currentLevel + 1);
        }

        Debug.Log("LEVEL" + PlayerPrefs.GetInt("levelIsUnlocked") + " UNLOCKED");


        //safe Time
        timer[(currentLevel - 2)] = PlayerPrefs.GetFloat("timer" + (currentLevel - 2));
        PlayerPrefs.SetFloat("timer" + (currentLevel - 2), Time.timeSinceLevelLoad);

    }
}
