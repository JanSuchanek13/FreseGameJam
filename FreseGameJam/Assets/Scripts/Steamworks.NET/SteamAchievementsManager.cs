using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamAchievementsManager : MonoBehaviour
{
    // references:
    // https://www.youtube.com/watch?v=mix4Y6LTM_w
    // https://partner.steamgames.com/doc/features/achievements
    // https://partner.steamgames.com/doc/features/achievements/stats_guide

    [Header("Achievement Variables:")]

    // achievement #1: Find first crown ever
    [Header("Achievement #1: Find first crown ever")]
    [SerializeField] string _settings_1 = "none";
    private bool _checkForAchievement1 = true;

    // achievement #2: Find 10 crowns in a run
    [Header("Achievement #2: Find 10 crowns in a run")]
    [Tooltip("The alternative to this is, to only check at the end of every run")]
    [SerializeField] bool _updateInRealtime_Ach_2 = true;
    //[SerializeField] string _settings_2 = "none";
    private bool _checkForAchievement2 = true;

    // achievement #3: Find 20 crowns in a run
    [Header("Achievement #3: Find 20 crowns in a run")]
    [Tooltip("The alternative to this is, to only check at the end of every run")]
    [SerializeField] bool _updateInRealtime_Ach_3 = true;
    //[SerializeField] string _settings_3 = "none";
    private bool _checkForAchievement3 = true;

    // achievement #4: Find 30 crowns in a run
    [Header("Achievement #4: Find 30 crowns in a run")]
    [Tooltip("The alternative to this is, to only check at the end of every run")]
    [SerializeField] bool _updateInRealtime_Ach_4 = true;
    //[SerializeField] string _settings_4 = "none";
    private bool _checkForAchievement4 = true;

    [Header("Achievement #5: Find all crowns in a run")]
    [Tooltip("Drag crown-parent-GO in here so the maximum number of crowns will update and be accurate automatically!")]
    [SerializeField] GameObject _crownsParentGO;
    [Tooltip("List of Crowns in this Level")]
    List<GameObject> _listOfAllCrowns = new List<GameObject>();
    private bool _checkForAchievement5 = true;

    // achievement #6: Reach your lover for the first time
    [Header("Achievement #6: Reach your lover for the first time")]
    [SerializeField] string _settings_6 = "none";
    private bool _checkForAchievement6 = true;

    // achievement #7: Get crane first time
    [Header("Achievement #7: Get crane first time")]
    [SerializeField] string _settings_7 = "none";
    private bool _checkForAchievement7 = true;

    // achievement #8: Get goat first time
    [Header("Achievement #8: Get goat first time")]
    [SerializeField] string _settings_8 = "none";
    private bool _checkForAchievement8 = true;

    // achievement #9: Die 5 times of water in one run (H)
    [Header("Achievement #9: Die 5 times of water in one run (H)")]
    [SerializeField] string _settings_9 = "none";
    HealthSystem _playerHealthSystem; // also used for #10
    private bool _checkForAchievement9 = true;

    // achievement #10: Die 5 times of lava in one run (H)
    [Header("Achievement #10: Die 5 times of lava in one run (H)")]
    [SerializeField] string _settings_10 = "none";
    private bool _checkForAchievement10 = true;

    // achievement #11: Finish a run in under 10 mins
    [Header("Achievement #11: Finish a run in under 10 mins")]
    [SerializeField] string _settings_11 = "none";
    private bool _checkForAchievement11 = true;

    // achievement #12: Push over a tree
    [Header("Achievement #12: Push over a tree")]
    [SerializeField] string _settings_12 = "none";
    private bool _checkForAchievement12 = true;

    // achievement #13: Use every checkpopint
    [Header("Achievement #13: Use every checkpopint")]
    [SerializeField] int _checkpointsInGame = 11; // 0 is the start spawnpoint, not a checkpoint per se
    int _currentCheckpoint = 0;
    int _sumOfCheckpointsReached; // this is needed to controll that all checkpoints were reached - not just the last!
    private bool _checkForAchievement13 = true;

    // achievement #14: Complete a hardcore run
    [Header("Achievement #14: Complete a hardcore run")]
    [SerializeField] string _settings_14 = "none";
    private bool _checkForAchievement14 = true;

    // achievement #15: Become the goatman (H)
    [Header("Achievement #15: Become the goatman (H)")]
    [SerializeField] GameObject _humanForm;
    [SerializeField] GameObject _goatForm;
    private bool _checkForAchievement15 = true;

    // achievement #16: x
    [Header("Achievement #16: x")]
    [SerializeField] string _settings_16 = "none";
    private bool _checkForAchievement16 = true;

    // achievement #17: x
    [Header("Achievement #17: x")]
    [SerializeField] string _settings_17 = "none";
    private bool _checkForAchievement17 = true;
    
    // achievement #18: x
    [Header("Achievement #18: x")]
    [SerializeField] string _settings_18 = "none";
    private bool _checkForAchievement18 = true;
    
    // achievement #19: x
    [Header("Achievement #19: x")]
    [SerializeField] string _settings_19 = "none";
    private bool _checkForAchievement19 = true;

    // achievement #20: x
    [Header("Achievement #20: x")]
    [SerializeField] string _settings_20 = "none";
    private bool _checkForAchievement20 = true;

    // achievement #21: x
    [Header("Achievement #21: x")]
    [SerializeField] string _settings_21 = "none";
    private bool _checkForAchievement21 = true;

    // achievement #22: x
    [Header("Achievement #22: x")]
    [SerializeField] string _settings_22 = "none";
    private bool _checkForAchievement22 = true;

    // achievement #23: x
    [Header("Achievement #23: x")]
    [SerializeField] string _settings_23 = "none";
    private bool _checkForAchievement23 = true;

    // achievement #24: x
    [Header("Achievement #24: x")]
    [SerializeField] string _settings_24 = "none";
    private bool _checkForAchievement24 = true;

    // achievement #25: x
    [Header("Achievement #25: x")]
    [SerializeField] string _settings_25 = "none";
    private bool _checkForAchievement25 = true;

    // achievement #26: x
    [Header("Achievement #26: x")]
    [SerializeField] string _settings_26 = "none";
    private bool _checkForAchievement26 = true;

    // achievement #27: Glide for 10 minutes in any amt of runs
    [Header("Achievement #27: Glide for 10 minutes in any amt of runs")]
    [SerializeField] GameObject _craneForm;
    float _sessionCraneTimeActive;
    private bool _checkForAchievement27 = true;

    // achievement #28: x
    [Header("Achievement #28: x")]
    [SerializeField] string _settings_28 = "none";
    private bool _checkForAchievement28 = true;
    
    // achievement #29: x
    [Header("Achievement #29: x")]
    [SerializeField] string _settings_29 = "none";
    private bool _checkForAchievement29 = true;
    
    // achievement #30: x
    [Header("Achievement #30: x")]
    [SerializeField] string _settings_30 = "none";
    private bool _checkForAchievement30 = true;
    
    // achievement #31: x
    [Header("Achievement #31: x")]
    [SerializeField] string _settings_31 = "none";
    private bool _checkForAchievement31 = true;
    
    // achievement #32: x
    [Header("Achievement #32: x")]
    [SerializeField] string _settings_32 = "none";
    private bool _checkForAchievement32 = true;
    
    // achievement #33: x
    [Header("Achievement #33: x")]
    [SerializeField] string _settings_33 = "none";
    private bool _checkForAchievement33 = true;
    
    // achievement #34: x
    [Header("Achievement #34: x")]
    [SerializeField] string _settings_34 = "none";
    private bool _checkForAchievement34 = true;

    // achievement #35: Try jumping on the moving boat of friend (H)
    [Header("Achievement #34: Try jumping on the moving boat of friend (H)")]
    [SerializeField] string _settings_35 = "none";
    private bool _checkForAchievement35 = true;

    private void Awake()
    {
        GatherRequiredData();

        StartCoroutine("CheckForAchievements");

        // enable to test specific math/solutions/conclusions:
        //Debug.Log("The max sum of checkpoints is: " + (_checkpointsInGame * (_checkpointsInGame + 1) / 2) + " based on having " + _checkpointsInGame + " checkpoints in the game");

        Debug.Log("youve finished: " + PlayerPrefs.GetInt("RunsCompleted" + 0, 0) + " runs");
        Debug.Log("youve glided: " + GetTotalCraneTimeActive() + " seconds");
    }

    private void GatherRequiredData()
    {
        // link to the HealthSystem:
        _playerHealthSystem = FindAnyObjectByType<HealthSystem>();

        // count all crowns in the game:
        _listOfAllCrowns.Clear();
        foreach (Transform child in _crownsParentGO.transform)
        {
            _listOfAllCrowns.Add(child.gameObject);
        }
    }

    /// <summary>
    /// Track ingame progression and check for new achievements every 0.1 seconds. 
    /// Saves performance as opposed to a constant Update-function.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForAchievements()
    {
        yield return new WaitForSeconds(1.0f);

        // ensure the SteamManager was loaded successfully: 
        if (!SteamManager.Initialized)
        {
            StartCoroutine("CheckForAchievements");
            yield break;
        }

        // achievement #1: Find first crown ever
        // explanation: you do NOT need to finish a run to get this
        if(_checkForAchievement1 && PlayerPrefs.GetInt("crowns" + 0, 0) != 0 || PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0) != 0)
        {
            _checkForAchievement1 = false;
            UnlockAchievement(1);
        }

        // achievement #2: Find 10 crowns in a run
        // explanation: You may pick in the settings for this achievement
        // whether it should only count at the end of a run, or in realtime
        if (_checkForAchievement2)
        {
            if (!_updateInRealtime_Ach_2 && (PlayerPrefs.GetInt("HCcrowns" + 0, 0) >= 10 || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) >= 10))
            {
                _checkForAchievement2 = false;
                UnlockAchievement(2);
            }else if (_updateInRealtime_Ach_2 && (PlayerPrefs.GetInt("crowns" + 0, 0) >= 10 || PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0) >= 10))
            {
                _checkForAchievement2 = false;
                UnlockAchievement(2);
            }
        }

        // achievement #3: Find 20 crowns in a run
        // explanation: You may pick in the settings for this achievement
        // whether it should only count at the end of a run, or in realtime
        if (_checkForAchievement3)
        {
            if (!_updateInRealtime_Ach_3 && (PlayerPrefs.GetInt("HCcrowns" + 0, 0) >= 20 || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) >= 20))
            {
                _checkForAchievement3 = false;
                UnlockAchievement(3);
            }else if (_updateInRealtime_Ach_3 && (PlayerPrefs.GetInt("crowns" + 0, 0) >= 20 || PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0) >= 20))
            {
                _checkForAchievement3 = false;
                UnlockAchievement(3);
            }
        }

        // achievement #4: Find 30 crowns in a run
        // explanation: You may pick in the settings for this achievement
        // whether it should only count at the end of a run, or in realtime
        if (_checkForAchievement4)
        {
            if (!_updateInRealtime_Ach_4 && (PlayerPrefs.GetInt("HCcrowns" + 0, 0) >= 30 || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) >= 30))
            {
                _checkForAchievement4 = false;
                UnlockAchievement(4);
            }else if(_updateInRealtime_Ach_4 && (PlayerPrefs.GetInt("crowns" + 0, 0) >= 30 || PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0) >= 30))
            {
                _checkForAchievement4 = false;
                UnlockAchievement(4);
            }
        }

        // achievement #5: Find all crowns in a run
        // explanation: you NEED to finish a run to get this, thus this would be found in highscores
        int _crownsInGame = _listOfAllCrowns.Count;
        if (_checkForAchievement5 && (PlayerPrefs.GetInt("HCcrowns" + 0, 0) == _crownsInGame) || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) == _crownsInGame)
        {
            _checkForAchievement5 = false;
            UnlockAchievement(5);
        }

        // achievement #6: Reach your lover for the first time
        // explanation: When finishing any run for the first time you automatically unlock the next level, using this as indicator
        if (_checkForAchievement6 && PlayerPrefs.GetInt("RunsCompleted" + 0, 0) != 0)
        {
            _checkForAchievement6 = false;
            UnlockAchievement(6);
        }

        // achievement #7: Get crane first time
        // explanation: states change when picking up shapes
        if (_checkForAchievement7 && PlayerPrefs.GetInt("State" + 0, 0) == 1)
        {
            _checkForAchievement7 = false;
            UnlockAchievement(7);
        }

        // achievement #8: Get goat first time
        if (_checkForAchievement8 && PlayerPrefs.GetInt("State" + 0, 0) == 2)
        {
            _checkForAchievement8 = false;
            UnlockAchievement(8);
        }

        // achievement #9: Die 5 times of water in one run (H)
        // explanation: Added custom logic to "HealthSystem" to differentiate between types of deaths.
        // MANDATORY: all death zones on waters must be named "Water_deathZone"
        if (_checkForAchievement9 && _playerHealthSystem.waterDeaths >= 5)
        {
            _checkForAchievement9 = false;
            UnlockAchievement(9);
        }

        // achievement #10: Die 5 times of lava in one run (H)
        // explanation: Added custom logic to "HealthSystem" to differentiate between types of deaths
        // MANDATORY: all death zones on waters must be named "Water_deathZone"
        if (_checkForAchievement10 && _playerHealthSystem.fireDeaths >= 5)
        {
            _checkForAchievement10 = false;
            UnlockAchievement(10);
        }

        // achievement #11: Finish a run in under 10 mins
        // explanation: check if any time highscore exists, if so, that means a run was completed,
        // then checks if any of those were under 10 mins (600f on time.deltaTime).
        // I'm not checking against the hardcore crown runs time, as the fastest hardcore run
        // will always also save under the "HighscoreHardcoreTime_Time"-key!
        if (_checkForAchievement11 && (PlayerPrefs.GetFloat("HTtimer" + 0, 0) != 0 && PlayerPrefs.GetFloat("HTtimer" + 0, 0) < 600.0f)
            || (PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0, 0) != 0 && PlayerPrefs.GetFloat("HardcoreTime" + 0, 0) < 600.0f))
        {
            _checkForAchievement11 = false;
            UnlockAchievement(11);
        }

        // achievement #12: Push over a tree
        // explanation: Added custom logic to "ActivateGravityOnTouch" to check if the knocked item was a tree or palm
        // MANDATORY: all trees must be named "PineTree"; palms must be named "PalmTree"
        if (_checkForAchievement12 && PlayerPrefs.GetInt("TreeKnockedOver" + 0, 0) == 1)
        {
            _checkForAchievement12 = false;
            UnlockAchievement(12);
        }

        // achievement #13: Use every checkpopint
        // explanation: A bit tricky one. I accumulate the sum of the nr of each checkpoint to see
        // if all have been reached. Otherwise one could simply run through the same checkpoint over and over or
        // run through the last one and then getting this achievement.
        // Now only if every checkpoint was used you get the achievement!
        if (_checkForAchievement13 && _currentCheckpoint != PlayerPrefs.GetInt("lastCheckpoint" + 0, 0))
        {
            _sumOfCheckpointsReached += PlayerPrefs.GetInt("lastCheckpoint" + 0);
            _currentCheckpoint = PlayerPrefs.GetInt("lastCheckpoint" + 0);

            if (_sumOfCheckpointsReached == _checkpointsInGame * (_checkpointsInGame + 1) / 2)
            {
                _checkForAchievement13 = false;
                UnlockAchievement(13);
            }
        }

        // achievement #14: Complete a hardcore run
        // explanation: If a hardcore run is completed, it will be saved to the "HighscoreHardcoreTime_Time"-key
        if (_checkForAchievement14 && PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0, 0) != 0.0f)
        {
            _checkForAchievement14 = false;
            UnlockAchievement(14);
        }

        // achievement #15: Become the goatman (H)
        // explanation: check if both models are active at the same time
        if (_checkForAchievement15 && _humanForm.activeInHierarchy && _goatForm.activeInHierarchy)
        {
            _checkForAchievement15 = false;
            FindAnyObjectByType<WearACrown>().UnlockCharacterCrown(010);
            UnlockAchievement(15);
        }

        // achievement #16: Become global HC crown-run champion
        // explanation: check playerPref set by Jan
        if (_checkForAchievement16 && PlayerPrefs.GetInt("WasFastestHardcoreCrownRunner" + 0, 0) == 1)
        {
            _checkForAchievement16 = false;
            FindAnyObjectByType<WearACrown>().UnlockCharacterCrown(001);
            UnlockAchievement(16);
        }

        // achievement #17: Become global HC speed-run champion
        // explanation: check playerPref set by Jan
        if (_checkForAchievement17 && PlayerPrefs.GetInt("WasFastestHardcoreRunner" + 0, 0) == 1)
        {
            _checkForAchievement17 = false;
            UnlockAchievement(17);
        }

        // achievement #18: Complete any 10 runs
        // explanation: int gets updated when finishing any run
        if (_checkForAchievement18 && PlayerPrefs.GetInt("RunsCompleted" + 0, 0) >= 10)
        {
            _checkForAchievement18 = false;
            UnlockAchievement(18);
        }

        // achievement #19: Complete any 50 runs
        // explanation: int gets updated when finishing any run
        if (_checkForAchievement19 && (PlayerPrefs.GetInt("RunsCompleted" + 0, 0) >= 50))
        {
            Debug.Log("youve now finished: " + PlayerPrefs.GetInt("RunsCompleted" + 0, 0) + " runs");

            _checkForAchievement19 = false;
            UnlockAchievement(19);
        }

        // achievement #20: Complete any 100 runs
        // explanation: int gets updated when finishing any run in the LevelScript (when entering the finish-trigger)
        if (_checkForAchievement20 && PlayerPrefs.GetInt("RunsCompleted" + 0, 0) >= 100)
        {
            _checkForAchievement20 = false;
            UnlockAchievement(20);
        }

        // achievement #21: Find 100 crowns over any number of reg. runs
        // explanation: int gets updated whenever a crown is picked up in a regular run
        if (_checkForAchievement21 && PlayerPrefs.GetInt("RegularCrownsFoundOverLifetime" + 0, 0) >= 100)
        {
            _checkForAchievement21 = false;
            UnlockAchievement(21);
        }

        // achievement #22: Find 500 crowns over any number of reg. runs
        // explanation: int gets updated when finishing any run
        if (_checkForAchievement22 && PlayerPrefs.GetInt("RegularCrownsFoundOverLifetime" + 0, 0) >= 500)
        {
            _checkForAchievement22 = false;
            UnlockAchievement(22);
        }

        // achievement #23: Find 1000 crowns over any number of reg. runs
        // explanation: int gets updated whenever a crown is picked up in a regular run
        if (_checkForAchievement23 && PlayerPrefs.GetInt("RegularCrownsFoundOverLifetime" + 0, 0) >= 1000)
        {
            _checkForAchievement23 = false;
            FindAnyObjectByType<WearACrown>().UnlockCharacterCrown(100);
            UnlockAchievement(23);
        }

        // achievement #24: Find 50 crowns over any number of hc. runs
        // explanation: int gets updated whenever a crown is picked up in a hardcore run
        if (_checkForAchievement24 && PlayerPrefs.GetInt("HardcoreCrownsFoundOverLifetime" + 0, 0) >= 50)
        {
            _checkForAchievement24 = false;
            UnlockAchievement(24);
        }

        // achievement #25: Find 100 crowns over any number of hc. runs
        // explanation: int gets updated whenever a crown is picked up in a hardcore run
        if (_checkForAchievement25 && PlayerPrefs.GetInt("HardcoreCrownsFoundOverLifetime" + 0, 0) >= 100)
        {
            _checkForAchievement25 = false;
            UnlockAchievement(25);
        }

        // achievement #26: Find 150 crowns over any number of hc. runs
        // explanation: int gets updated whenever a crown is picked up in a hardcore run
        if (_checkForAchievement26 && PlayerPrefs.GetInt("HardcoreCrownsFoundOverLifetime" + 0, 0) >= 150)
        {
            _checkForAchievement26 = false;
            UnlockAchievement(26);
        }

        // achievement #27: Glide for 10 mins over any number of runs
        // explanation: this float is tracked by checking when the crane form is activ
        // in the Update of this class (see region too)
        if (_checkForAchievement27 && GetTotalCraneTimeActive() >= 600.0f)
        {
            _checkForAchievement27 = false;
            UnlockAchievement(27);
        }

        // achievement #28: Glide for 30 mins over any number of runs
        // explanation: this float is tracked by checking when the crane form is activ
        // in the Update of this class (see region too)
        if (_checkForAchievement28 && GetTotalCraneTimeActive() >= 1800.0f)
        {
            _checkForAchievement28 = false;
            UnlockAchievement(28);
        }

        // achievement #29: Glide for 60 mins over any number of runs
        // explanation: this float is tracked by checking when the crane form is activ
        // in the Update of this class (see region too)
        if (_checkForAchievement29 && GetTotalCraneTimeActive() >= 3600.0f)
        {
            _checkForAchievement29 = false;
            UnlockAchievement(29);
        }

        // achievement #30-34: Find the hidden locations around the world
        // explanation: Whenever you first find/enter a hidden regions trigger the tracking-Int 
        // gets updated. The Explorer-status gets updated in accordance to how many locations the player has found
        if ((_checkForAchievement30 || _checkForAchievement31 || _checkForAchievement32 || _checkForAchievement33 || _checkForAchievement34) 
            && PlayerPrefs.GetInt("HiddenLocationsFound" + 0, 0) <= 1)
        {
            int _nrOfLocationsFound = PlayerPrefs.GetInt("HiddenLocationsFound" + 0, 0);
            switch (_nrOfLocationsFound)
            {
                case 1:
                    _checkForAchievement30 = false;
                    UnlockAchievement(30);
                    break;
                
                case 2:
                    _checkForAchievement31 = false;
                    UnlockAchievement(31);
                    break;

                case 3:
                    _checkForAchievement32 = false;
                    UnlockAchievement(32);
                    break;

                case 4:
                    _checkForAchievement33 = false;
                    UnlockAchievement(33);
                    break;

                case 5:
                    _checkForAchievement34 = false;
                    UnlockAchievement(34);
                    break;
            }
        }

        // achievement #35: Try jumping on friend in boat (H)
        // explanation: This is tracked by the OnTriggerEnter Function of the "SpeedUpNavMeshAgent"-Script
        // on the riverBoat_Friend_Fire GO (your friend in the burning boat: when he accellerates, this is triggered!)
        if (_checkForAchievement35 && PlayerPrefs.GetInt("TriedJumpingOnMovingBoat", 0) != 0)
        {
            _checkForAchievement35 = false;
            UnlockAchievement(35);
        }

        // check again:
        StartCoroutine("CheckForAchievements");
    }

    void Update()
    {
        if (_craneForm.activeInHierarchy && Time.timeScale != 0)
        {
            _sessionCraneTimeActive += Time.deltaTime;
        }


        // ensure the SteamManager was loaded successfully: 
        if (!SteamManager.Initialized)
        {
            return;
        }

        // stats = tracking data, like distance run, which can be tracked as progression
        //int _dataExample = 1;
        //SteamUserStats.SetStat("exampleStatKey", _dataExample);

        // achievement = actually reachable target which does not need tracked progression
        //SteamUserStats.SetAchievement("exampleAchievementKey"); // use keycode for achievement here!

        // store all stats AND achievements to steam
        //SteamUserStats.StoreStats();

        if (Input.GetKeyUp(KeyCode.KeypadEnter)) // save example achievement on ENTER
        {
            UnlockAchievement(0);
            Debug.Log("you've unlocked the test achievement - dont forget to delete it again by hitting ESC!");
        }

        if (Input.GetKeyDown(KeyCode.Delete)) // delete all stats & achievements on ESC
        {
            ResetAllStats(true);
            Debug.Log("good job, you've deleted all achievements!");
        }
    }

    #region Managing global glide time:
    void OnApplicationQuit()
    {
        SaveCraneTime();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // if the application is paused
        {
            SaveCraneTime();
        }
    }

    private float GetTotalCraneTimeActive()
    {
        return PlayerPrefs.GetFloat("GlideTimeOverLifetime", 0) + _sessionCraneTimeActive;
    }

    private void SaveCraneTime()
    {
        float totalTime = GetTotalCraneTimeActive();
        PlayerPrefs.SetFloat("GlideTimeOverLifetime", totalTime);
        PlayerPrefs.Save(); // this ensures data is written immediately.
        Debug.Log("lifetime glide time: " + totalTime);
    }
    #endregion

    /// <summary>
    /// Call this function when unlocking an achievement at any place in the game by populating the correct ID
    /// 1 = Find crown
    /// </summary>
    /// <param name="_achievementID"></param>
    void UnlockAchievement(int _achievementID)
    {
        Debug.Log("UnlockAchievement called with ID " + _achievementID);


        switch (_achievementID)
        {
            case 0: // TEST (deleted since)
                //SteamUserStats.SetAchievement("ach_TEST");
                Debug.Log("Test achievement called");
                break;

            case 1: // achievement: Find first crown ever
                SteamUserStats.SetAchievement("ach1_ANY_CrownsFoundSingleRun_1");
                break;

            case 2: // achievement: Find 10 crowns in a run
                SteamUserStats.SetAchievement("ach2_ANY_CrownsFoundSingleRun_10");
                break;

            case 3: // achievement: Find 20 crowns in a run
                SteamUserStats.SetAchievement("ach3_ANY_CrownsFoundSingleRun_20");
                break;

            case 4: // achievement: Find 30 crowns in a run
                SteamUserStats.SetAchievement("ach4_ANY_CrownsFoundSingleRun_30");
                break;
            
            case 5: // achievement: Find all crowns in a run
                SteamUserStats.SetAchievement("ach5_ANY_CrownsFoundSingleRun_ALL");
                break;

            case 6: // achievement: Reach your lover for the first time
                SteamUserStats.SetAchievement("ach6_ANY_Reunion");
                break;

            case 7: // achievement: Get crane first time
                SteamUserStats.SetAchievement("ach7_ANY_GetCrane");
                break;

            case 8: // achievement: Get goat first time
                SteamUserStats.SetAchievement("ach8_ANY_GetGoat");
                break;

            case 9: // achievement: Die 5 times of water in one run (H)
                SteamUserStats.SetAchievement("ach9_ANY_Drowner_5");
                break;

            case 10: // achievement: Die 5 times of lava in one run (H)
                SteamUserStats.SetAchievement("ach10_ANY_Burner_5");
                break;

            case 11: // achievement: Finish a run in under 10 mins
                SteamUserStats.SetAchievement("ach11_ANY_RunTime_10");
                break;

            case 12: // achievement: Push over a tree
                SteamUserStats.SetAchievement("ach12_ANY_PushTree");
                break;

            case 13: // achievement: Use every checkpopint
                SteamUserStats.SetAchievement("ach13_REG_Checkpoints");
                break;

            case 14: // achievement: Complete a hardcore run
                SteamUserStats.SetAchievement("ach14_HARDC_RunComplete_1");
                break;

            case 15: // achievement: Become the goatman (H)
                SteamUserStats.SetAchievement("ach15_ANY_Goatman");
                break;

            case 16: // achievement: Become global HC crown-run champion
                SteamUserStats.SetAchievement("ach16_HARDC_GlobalHighscoreCrowns");
                break;

            case 17: // achievement: Become global HC speed-run champion
                SteamUserStats.SetAchievement("ach17_HARDC_GlobalHighscoreSpeed");
                break;

            case 18: // achievement: Finish any 10 runs
                SteamUserStats.SetAchievement("ach18_ANY_FinishedRuns_10");
                break;

            case 19: // achievement: Finish any 50 runs
                SteamUserStats.SetAchievement("ach19_ANY_FinishedRuns_50");
                break;

            case 20: // achievement: Finish any 100 runs
                SteamUserStats.SetAchievement("ach20_ANY_FinishedRuns_100");
                break;

            case 21: // achievement: Find 100 crowns over any amount of regular runs (no need to complete)
                SteamUserStats.SetAchievement("ach21_REG_Alltime_CrownsFound_100");
                break;

            case 22: // achievement: Find 500 crowns over any amount of regular runs (no need to complete)
                SteamUserStats.SetAchievement("ach22_REG_Alltime_CrownsFound_500");
                break;

            case 23: // achievement: Find 1000 crowns over any amount of regular runs (no need to complete)
                SteamUserStats.SetAchievement("ach23_REG_Alltime_CrownsFound_1000");
                break;

            case 24: // achievement: Find 50 hardcore crowns over any amount of runs (no need to complete)(H)
                SteamUserStats.SetAchievement("ach24_HARDC_Alltime_CrownsFound_50");
                break;

            case 25: // achievement: Find 100 hardcore crowns over any amount of runs (no need to complete)(H)
                SteamUserStats.SetAchievement("ach25_HARDC_Alltime_CrownsFound_100");
                break;

            case 26: // achievement: Find 150 hardcore crowns over any amount of runs (no need to complete)(H)
                SteamUserStats.SetAchievement("ach26_HARDC_Alltime_CrownsFound_150");
                break;

            case 27: // achievement: Glide a total of 10 minutes over any amount of runs
                SteamUserStats.SetAchievement("ach27_ANY_Alltime_GlideTime_10min");
                break;

            case 28: // achievement: Glide a total of 30 minutes over any amount of runs
                SteamUserStats.SetAchievement("ach28_ANY_Alltime_GlideTime_30min");
                break;

            case 29: // achievement: Glide a total of 60 minutes over any amount of runs
                SteamUserStats.SetAchievement("ach29_ANY_Alltime_GlideTime_60min");
                break;

            case 30: // achievement: Find one hidden location
                SteamUserStats.SetAchievement("ach30_ANY_Alltime_Explorer_lvl1");
                break;

            case 31: // achievement: Find two hidden locations
                SteamUserStats.SetAchievement("ach31_ANY_Alltime_Explorer_lvl2");
                break;

            case 32: // achievement: Find three hidden locations
                SteamUserStats.SetAchievement("ach32_ANY_Alltime_Explorer_lvl3");
                break;

            case 33: // achievement: Find four hidden locations
                SteamUserStats.SetAchievement("ach33_ANY_Alltime_Explorer_lvl4");
                break;

            case 34: // achievement: Find five hidden locations
                SteamUserStats.SetAchievement("ach34_ANY_Alltime_Explorer_lvl5");
                break;

            case 35: // achievement: Try jumping on moving boat (H)
                SteamUserStats.SetAchievement("ach35_ANY_JumpOnDrivingBoat");
                break;

            default:
                break;
        }

        SteamUserStats.StoreStats();
    }

    /// <summary>
    /// Reset either and or all stats and achievements.
    /// --> Stats           = tracking data, like nr of crowns found, which can be tracked as progression (this can be used to unlock achievements)
    /// --> Achievements    = Single point of achievement to be called
    /// </summary>
    /// <param name="_deleteAchievementsToo"></param>
    public void ResetAllStats(bool _deleteAchievementsToo)
    {
        if (_deleteAchievementsToo)
        {
            SteamUserStats.ResetAllStats(true); // resets all stats AND Achievements
        }else
        {
            SteamUserStats.ResetAllStats(false); // resets only stats
        }
    }
}
