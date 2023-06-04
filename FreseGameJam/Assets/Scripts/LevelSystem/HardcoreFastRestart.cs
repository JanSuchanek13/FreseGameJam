using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcoreFastRestart : MonoBehaviour
{
    [SerializeField] GameObject _loadingUI;

    /// <summary>
    /// this gets called when the start menu is called and the FastReset was set to 1 (from within the hardcore mode).
    /// This will essentially quickly reset all progress (like collected shapes, crowns, time, etc) and instantly restart the
    /// hardcore mode for the player, without having to navigate the startscreen or wait for anything else to load.
    /// </summary>
    void Start()
    {
        if(PlayerPrefs.GetInt("FastReset", 0) != 0)
        {
            PlayerPrefs.SetInt("FastReset", 0);
            FindObjectOfType<Level_Manager>().FastRestartHardcore();
            FindObjectOfType<PlayWasPressed>().StartHardcoreRound();
        }else // if this is not a FastReset, then turn off the loading screen. 
        {
            _loadingUI.SetActive(false);
        }
    }
}
