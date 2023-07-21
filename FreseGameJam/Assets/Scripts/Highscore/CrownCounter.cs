using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// This script handles crown collection... But also deaths and time for some reason.
/// </summary>
public class CrownCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _crownCounterTMPro;
    [SerializeField] TextMeshProUGUI _timeCounterTMPro;
    [SerializeField] TextMeshProUGUI _deathCounterTMPro;
    [Space(10)]

    //[SerializeField] int _currentlevel = 0;

    [SerializeField] AudioSource _pickUpCrownSound;
    
    int[] _arrayOfCrowns = new int[3];
    int _crowns = 0;
    int _currentLevel = 0;
    CollectedCrowns _collectedCrowns;

    private void Start()
    {
        //CollectedCrowns = GameObject.Find("GameManager").GetComponent<CollectedCrowns>();
        _collectedCrowns = FindObjectOfType<CollectedCrowns>();
        
        _currentLevel = SceneManager.GetActiveScene().buildIndex;

        // update the crown counter in a regular run:
        if (PlayerPrefs.GetInt("HardcoreMode", 0) == 0)
        {
            _crowns = PlayerPrefs.GetInt("crowns" + 0, 0); // dumbed down version. the  + 0 should be the level index. redundant until we have more levels.
            _crownCounterTMPro.text = _crowns.ToString();
        }
    }

    /*
    void Update()
    {
        if (_crownCounterTMPro != null)
        {
            _crowns = PlayerPrefs.GetInt("crowns" + _currentLevel, 1);
            _crownCounterTMPro.text = _crowns.ToString();
        }
        

        if (_timeCounterTMPro != null)
        {
            float fullTime = PlayerPrefs.GetFloat("timer" + _currentLevel, 0) + PlayerPrefs.GetFloat("lastTimer" + _currentLevel, 0);
            _timeCounterTMPro.text = fullTime.ToString();
        }

        if (_deathCounterTMPro != null)
        {
            _deathCounterTMPro.text = PlayerPrefs.GetInt("deaths" + _currentLevel, 1).ToString();
        }
    }*/

    // collect crowns:
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crown"))
        {
            //if (!FindObjectOfType<HardcoreMode>().useHardcoreMode)
            if(PlayerPrefs.GetInt("HardcoreMode", 0) == 0)
            {
                //_currentlevel = SceneManager.GetActiveScene().buildIndex;

                _arrayOfCrowns[0] = PlayerPrefs.GetInt("crowns" + 0, 0);
                //PlayerPrefs.SetInt("crowns" + _currentLevel, _arrayOfCrowns[0] + 1);
                PlayerPrefs.SetInt("crowns" + 0, _arrayOfCrowns[0] + 1); // dumbed down version!!!
                _crowns = PlayerPrefs.GetInt("crowns" + 0, 0);
                _crownCounterTMPro.text = _crowns.ToString();


                _pickUpCrownSound.Play(0);
                other.gameObject.SetActive(false);

                // remember collected crowns of current run:
                _collectedCrowns.MakeBoolArray(false);

                // testing:
                //Debug.Log(PlayerPrefs.GetInt("crowns"));
                Debug.Log("Found crown! Now: " + PlayerPrefs.GetInt("crowns" + 0));
            }else
            {
                _crowns++;
                // this allows to collect HardcoreCrowns in more than one level if needed:
                //PlayerPrefs.SetInt("HardcoreCrowns" + _currentLevel, _arrayOfCrowns[0] + 1);
                PlayerPrefs.SetInt("HardcoreCrowns" + 0, _crowns); // dumbed down version!!!

                _pickUpCrownSound.Play(0);
                other.gameObject.SetActive(false);

                // testing:
                Debug.Log("Found hardcore-crown!");
            }
        }
    }
}
