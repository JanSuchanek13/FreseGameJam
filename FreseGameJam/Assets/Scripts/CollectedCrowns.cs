using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectedCrowns : MonoBehaviour
{
	int currentLevel;

	//safe active Crowns
	public List<GameObject> Crowns;
	
	public List<bool> boolValuesArr;
    public string bitString = ""; // To store bits representing bool values

    private void Start()
    {
		currentLevel = SceneManager.GetActiveScene().buildIndex;
		
		MakeBoolArray(true);
		

	}


    public void MakeBoolArray(bool Start)
    {
        if (!Start)
        {
			boolValuesArr.Clear();
			for (int i = 0; i < Crowns.Count; i++)
			{
				boolValuesArr.Add(Crowns[i].activeInHierarchy);
			}

			Debug.Log("second");
			DeactivateCrowns();
			//bitString = PlayerPrefs.GetString("bitString" + (currentLevel - 2));
			//Debug.Log("first");
			//UnpackDataFromBitString();
			PackDataToBitString();
		}
        else
        {
			for (int i = 0; i < Crowns.Count; i++)
			{
				boolValuesArr.Add(Crowns[i].activeInHierarchy);
			}

			Debug.Log("second");
			
			bitString = PlayerPrefs.GetString("bitString" + (currentLevel - 2));
			Debug.Log("first");
			UnpackDataFromBitString();
			DeactivateCrowns();
			//PackDataToBitString();
		}
        

	}

	void DeactivateCrowns()
    {
        for (int i = 0; i < boolValuesArr.Count; i++)
        {
			if(boolValuesArr[i] == false)
            {
				Crowns[i].SetActive(false);
            }
        }
    }

	/*
	public void ChangeBoolArray(GameObject Crown)
    {
        for (int i = 0; i < boolValuesArr.Count; i++)
        {
			if(boolValuesArr[i] == Crown)
        }
    }
	*/








	// Read bool array and pack the data into a string.
	void PackDataToBitString()
	{
		char[] bits = new char[boolValuesArr.Count];
		for (int i = 0; i < bits.Length; i++)
		{
			bits[i] = boolValuesArr[i] == true ? '1' : '0';
		}
		bitString = new string(bits);
		Debug.Log("Packed data: " + bitString);

		// safe String
		PlayerPrefs.SetString("bitString" + (currentLevel - 2), bitString);
		Debug.Log("PlayerPrefs data: " + PlayerPrefs.GetString("bitString" + (currentLevel - 2)));

	}

	// Read string and unpack the data into a bool array.
	void UnpackDataFromBitString()
	{
		char[] bits = bitString.ToCharArray();
		//boolValuesArr = new List<bool>[bits.Length];
		for (int i = 0; i < bits.Length; i++)
		{
			boolValuesArr[i] = bits[i] == '1' ? true : false;
			Debug.Log("unpacked data: " + boolValuesArr[i]);
		}
		
	}
}
