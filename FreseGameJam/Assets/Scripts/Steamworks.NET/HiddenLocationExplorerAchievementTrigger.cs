using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLocationExplorerAchievementTrigger : MonoBehaviour
{
    [Tooltip("This has to be 1-5!")]
    [SerializeField] int _thisHiddenLocationID = 0;

    /// <summary>
    /// Turn this trigger-obj off, if it has previously found.
    /// </summary>
    private void Awake()
    {
        if(PlayerPrefs.GetInt("HiddenLocationNr" + _thisHiddenLocationID, 0) != 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// When player runs into hidden location trigger it will check if it has been found before, 
    /// and if its infact getting triggered by a GO with the "Player"-tag.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("HiddenLocationNr" + _thisHiddenLocationID, 0) == 0)
        {
            // update this hiddenlocations behavior for future runs etc:
            PlayerPrefs.SetInt("HiddenLocationNr" + _thisHiddenLocationID, 1);

            // update current count of found locations:
            int _currentlyFoundHiddenLocations = PlayerPrefs.GetInt("HiddenLocationsFound" + 0, 0);
            _currentlyFoundHiddenLocations++;
            PlayerPrefs.SetInt("HiddenLocationsFound" + 0, _currentlyFoundHiddenLocations);

            // debug for testing:
            Debug.Log("A hidden location was found! Note: This location cannot be found again unless you reset stats!");

            // update steam stats tracking:
            FindAnyObjectByType<SteamStatsManager>().UpdateStat("stat_ANY_secretLocationsFound", 1);
            Debug.Log("Added: " + 1 + " to secret locations found on steam.");
        }
    }
}
