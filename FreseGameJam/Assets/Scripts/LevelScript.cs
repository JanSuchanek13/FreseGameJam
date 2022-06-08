using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    public Animator transition;
    public int nextLevel = 3;

    float[] timer = new float[3];
    float[] lastTimer = new float[3];
    int currentLevel;

    private void Start()
    {
        //safe level
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //add time of last run
        lastTimer[(currentLevel - 2)] = PlayerPrefs.GetFloat("lastTimer" + (currentLevel - 2));
    }

    private void Update()
    {
        //safe time per frame
        PlayerPrefs.SetFloat("lastTimer" + (currentLevel - 2), Time.timeSinceLevelLoad + lastTimer[(currentLevel - 2)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hit");
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

       
        



        //change Level
        StartCoroutine(LoadLevel(nextLevel));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(nextLevel);
    }
}
