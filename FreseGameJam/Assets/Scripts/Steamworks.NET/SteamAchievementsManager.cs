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

    // achievement #2: Find 10 crowns in a run
    [Header("Achievement #2: FiVisualnd 10 crowns in a run")]
    [SerializeField] string _settings_2 = "none";

    // achievement #3: Find 20 crowns in a run
    [Header("Achievement #3: Find 20 crowns in a run")]
    [SerializeField] string _settings_3 = "none";

    // achievement #4: Find 30 crowns in a run
    [Header("Achievement #4: Find 30 crowns in a run")]
    [SerializeField] string _settings_4 = "none";

    [Header("Achievement #5: Find all crowns in a run")]
    [Tooltip("Drag crown-parent-GO in here so the maximum number of crowns will update and be accurate automatically!")]
    [SerializeField] GameObject _crownsParentGO;
    [Tooltip("List of Crowns in this Level")]
    List<GameObject> _listOfAllCrowns = new List<GameObject>();

    // achievement #6: Reach your lover for the first time
    [Header("Achievement #6: Reach your lover for the first time")]
    [SerializeField] string _settings_6 = "none";

    // achievement #7: Get crane first time
    [Header("Achievement #7: Get crane first time")]
    [SerializeField] string _settings_7 = "none";

    // achievement #8: Get goat first time
    [Header("Achievement #8: Get goat first time")]
    [SerializeField] string _settings_8 = "none";

    // achievement #9: Die 5 times of water in one run (H)
    [Header("Achievement #9: Die 5 times of water in one run (H)")]
    [SerializeField] string _settings_9 = "none";
    HealthSystem _playerHealthSystem; // also used for #10

    // achievement #10: Die 5 times of lava in one run (H)
    [Header("Achievement #10: Die 5 times of lava in one run (H)")]
    [SerializeField] string _settings_10 = "none";

    // achievement #11: Finish a run in under 10 mins
    [Header("Achievement #11: Finish a run in under 10 mins")]
    [SerializeField] string _settings_11 = "none";

    // achievement #12: Push over a tree
    [Header("Achievement #12: Push over a tree")]
    [SerializeField] string _settings_12 = "none";

    // achievement #13: Use every checkpopint
    [Header("Achievement #13: Use every checkpopint")]
    [SerializeField] int _checkpointsInGame = 11; // 0 is the start spawnpoint, not a checkpoint per se
    int _currentCheckpoint = 0;
    int _sumOfCheckpointsReached; // this is needed to controll that all checkpoints were reached - not just the last!

    // achievement #14: Complete a hardcore run
    [Header("Achievement #14: Complete a hardcore run")]
    [SerializeField] string _settings_14 = "none";

    // achievement #15: Become the goatman (H)
    [Header("Achievement #15: Become the goatman (H)")]
    [SerializeField] GameObject _humanForm;
    [SerializeField] GameObject _goatForm;


    private void Awake()
    {
        GatherRequiredData();

        StartCoroutine("CheckForAchievements");

        // enable to test specific math/solutions/conclusions:
        //Debug.Log("The max sum of checkpoints is: " + (_checkpointsInGame * (_checkpointsInGame + 1) / 2) + " based on having " + _checkpointsInGame + " checkpoints in the game");
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
        yield return new WaitForSeconds(0.1f);

        // ensure the SteamManager was loaded successfully: 
        if (!SteamManager.Initialized)
        {
            StartCoroutine("CheckForAchievements");
            yield break;
        }

        // achievement #1: Find first crown ever
        // explanation: you do NOT need to finish a run to get this
        if(PlayerPrefs.GetInt("crowns" + 0, 0) != 0 || PlayerPrefs.GetInt("HardcoreCrowns" + 0, 0) != 0)
        {
            UnlockAchievement(1);
        }

        // achievement #2: Find 10 crowns in a run
        // explanation: you NEED to finish a run to get this, thus this would be found in highscores
        if ((PlayerPrefs.GetInt("HCcrowns" + 0, 0) >= 10) || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) >= 10)
        {
            UnlockAchievement(2);
        }

        // achievement #3: Find 20 crowns in a run
        // explanation: you NEED to finish a run to get this, thus this would be found in highscores
        if ((PlayerPrefs.GetInt("HCcrowns" + 0, 0) >= 20) || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) >= 20)
        {
            UnlockAchievement(3);
        }

        // achievement #4: Find 30 crowns in a run
        // explanation: you NEED to finish a run to get this, thus this would be found in highscores
        if ((PlayerPrefs.GetInt("HCcrowns" + 0, 0) >= 30) || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) >= 30)
        {
            UnlockAchievement(4);
        }

        // achievement #5: Find all crowns in a run
        // explanation: you NEED to finish a run to get this, thus this would be found in highscores
        int _crownsInGame = _listOfAllCrowns.Count;
        if ((PlayerPrefs.GetInt("HCcrowns" + 0, 0) == _crownsInGame) || PlayerPrefs.GetInt("HighscoreHardcoreCrowns_Crowns" + 0, 0) == _crownsInGame)
        {
            UnlockAchievement(5);
        }

        // achievement #6: Reach your lover for the first time
        // explanation: When finishing any run for the first time you automatically unlock the next level, using this as indicator
        if (PlayerPrefs.GetInt("levelIsUnlocked" + 0, 0) != 0)
        {
            UnlockAchievement(6);
        }

        // achievement #7: Get crane first time
        // explanation: states change when picking up shapes
        if (PlayerPrefs.GetInt("State" + 0, 0) == 1)
        {
            UnlockAchievement(7);
        }

        // achievement #8: Get goat first time
        if (PlayerPrefs.GetInt("State" + 0, 0) == 2)
        {
            UnlockAchievement(8);
        }

        // achievement #9: Die 5 times of water in one run (H)
        // explanation: Added custom logic to "HealthSystem" to differentiate between types of deaths.
        // MANDATORY: all death zones on waters must be named "Water_deathZone"
        if (_playerHealthSystem.waterDeaths >= 5)
        {
            UnlockAchievement(9);
        }

        // achievement #10: Die 5 times of lava in one run (H)
        // explanation: Added custom logic to "HealthSystem" to differentiate between types of deaths
        // MANDATORY: all death zones on waters must be named "Water_deathZone"
        if (_playerHealthSystem.fireDeaths >= 5)
        {
            UnlockAchievement(10);
        }

        // achievement #11: Finish a run in under 10 mins
        // explanation: check if any time highscore exists, if so, that means a run was completed,
        // then checks if any of those were under 10 mins (600f on time.deltaTime).
        // I'm not checking against the hardcore crown runs time, as the fastest hardcore run
        // will always also save under the "HighscoreHardcoreTime_Time"-key!
        if ((PlayerPrefs.GetFloat("HTtimer" + 0, 0) != 0 && PlayerPrefs.GetFloat("HTtimer" + 0, 0) < 600.0f)
            || (PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0, 0) != 0 && PlayerPrefs.GetFloat("HardcoreTime" + 0, 0) < 600.0f))
        {
            UnlockAchievement(11);
        }

        // achievement #12: Push over a tree
        // explanation: Added custom logic to "ActivateGravityOnTouch" to check if the knocked item was a tree or palm
        // MANDATORY: all trees must be named "PineTree"; palms must be named "PalmTree"
        if (PlayerPrefs.GetInt("TreeKnockedOver" + 0, 0) == 1)
        {
            UnlockAchievement(12);
        }

        // achievement #13: Use every checkpopint
        // explanation: A bit tricky one. I accumulate the sum of the nr of each checkpoint to see
        // if all have been reached. Otherwise one could simply run through the same checkpoint over and over or
        // run through the last one and then getting this achievement.
        // Now only if every checkpoint was used you get the achievement!
        if (_currentCheckpoint != PlayerPrefs.GetInt("lastCheckpoint" + 0, 0))
        {
            _sumOfCheckpointsReached += PlayerPrefs.GetInt("lastCheckpoint" + 0);
            _currentCheckpoint = PlayerPrefs.GetInt("lastCheckpoint" + 0);

            if (_sumOfCheckpointsReached == _checkpointsInGame * (_checkpointsInGame + 1) / 2)
            {
                UnlockAchievement(13);
            }
        }

        // achievement #14: Complete a hardcore run
        // explanation: If a hardcore run is completed, it will be saved to the "HighscoreHardcoreTime_Time"-key
        if (PlayerPrefs.GetFloat("HighscoreHardcoreTime_Time" + 0, 0) != 0.0f)
        {
            UnlockAchievement(14);
        }

        // achievement #15: Become the goatman (H)
        // explanation: check if both models are active at the same time
        if (_humanForm.activeInHierarchy && _goatForm.activeInHierarchy)
        {
            UnlockAchievement(15);
        }

        // check again:
        StartCoroutine("CheckForAchievements");
    }

    void Update()
    {
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

    /// <summary>
    /// Call this function when unlocking an achievement at any place in the game by populating the correct ID
    /// 1 = Find crown
    /// </summary>
    /// <param name="_achievementID"></param>
    public void UnlockAchievement(int _achievementID)
    {
        switch (_achievementID)
        {
            case 0: // TEST
                SteamUserStats.SetAchievement("ach_TEST");
                Debug.Log("Test achievement called");
                break;

            case 1: // achievement: Find first crown ever
                SteamUserStats.SetAchievement("ach_REG_CrownsFound_1");
                break;

            case 2: // achievement: Find 10 crowns in a run
                SteamUserStats.SetAchievement("ach_REG_CrownsFound_10");
                break;

            case 3: // achievement: Find 20 crowns in a run
                SteamUserStats.SetAchievement("ach_REG_CrownsFound_20");
                break;

            case 4: // achievement: Find 30 crowns in a run
                SteamUserStats.SetAchievement("ach_REG_CrownsFound_30");
                break;
            
            case 5: // achievement: Find all crowns in a run
                SteamUserStats.SetAchievement("ach_REG_CrownsFound_ALL");
                break;

            case 6: // achievement: Reach your lover for the first time
                SteamUserStats.SetAchievement("ach_REG_Reunion");
                break;

            case 7: // achievement: Get crane first time
                SteamUserStats.SetAchievement("ach_REG_GetCrane");
                break;

            case 8: // achievement: Get goat first time
                SteamUserStats.SetAchievement("ach_REG_GetGoat");
                break;

            case 9: // achievement: Die 5 times of water in one run (H)
                SteamUserStats.SetAchievement("ach_REG_Drowner_5");
                break;

            case 10: // achievement: Die 5 times of lava in one run (H)
                SteamUserStats.SetAchievement("ach_REG_Burner_5");
                break;

            case 11: // achievement: Finish a run in under 10 mins
                SteamUserStats.SetAchievement("ach_REG_RunTime_10");
                break;

            case 12: // achievement: Push over a tree
                SteamUserStats.SetAchievement("ach_REG_PushTree");
                break;

            case 13: // achievement: Use every checkpopint
                SteamUserStats.SetAchievement("ach_REG_Checkpoints");
                break;

            case 14: // achievement: Complete a hardcore run
                SteamUserStats.SetAchievement("ach_HARDC_RunComplete_1");
                break;

            case 15: // achievement: Become the goatman (H)
                SteamUserStats.SetAchievement("ach_REG_Goatman");
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
