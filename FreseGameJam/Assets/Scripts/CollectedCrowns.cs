using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectedCrowns : MonoBehaviour
{

	[Header("safe active Crowns")]

	[Tooltip("List of Crowns in this Level")]
	public List<GameObject> Crowns;
	[Tooltip("List of bools represent active Crowns in this Level")]
	public List<bool> boolValuesArr;
	[Tooltip("Sting of bits represent boolValueArr")]
	public string bitString = ""; // To store bits representing bool values

	[Header("REFERENCES")]

	[Tooltip("Reference of the current Level")]
	int currentLevel;

	private void Start()
    {
		currentLevel = SceneManager.GetActiveScene().buildIndex;
		
		MakeBoolArray(true);
		

	}

	/// <summary>
	/// We make a Bool Array out of enabled/disabled Crowns in this scene
	/// </summary>
	/// <param name="Start"></param>
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

			bitString = PlayerPrefs.GetString("bitString" + (currentLevel - 2));
			UnpackDataFromBitString();
			DeactivateCrowns();
			//PackDataToBitString();
		}
        

	}

	/// <summary>
	/// We disable all Crowns that are false in the BoolArray
	/// </summary>
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

	/// <summary>
	/// Read bool array and pack the data into a string of bits
	/// </summary>
	void PackDataToBitString()
	{
		char[] bits = new char[boolValuesArr.Count];
		for (int i = 0; i < bits.Length; i++)
		{
			bits[i] = boolValuesArr[i] == true ? '1' : '0';
		}
		bitString = new string(bits);
		//Debug.Log("Packed data: " + bitString);

		// safe String
		PlayerPrefs.SetString("bitString" + (currentLevel - 2), bitString);
		//Debug.Log("PlayerPrefs data: " + PlayerPrefs.GetString("bitString" + (currentLevel - 2)));

	}

	/// <summary>
	/// Read string and unpack the data into a bool array
	/// </summary>
	void UnpackDataFromBitString()
	{
		char[] bits = bitString.ToCharArray();
		//boolValuesArr = new List<bool>[bits.Length];
		for (int i = 0; i < bits.Length; i++)
		{
			boolValuesArr[i] = bits[i] == '1' ? true : false;
			//Debug.Log("unpacked data: " + boolValuesArr[i]);
		}
		
	}
}
