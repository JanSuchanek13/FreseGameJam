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

    /*void Update()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        // stats = tracking data, like distance run, which can be tracked as progression
        //int _dataExample = 1;
        //SteamUserStats.SetStat("exampleStatKey", _dataExample);

        // achievement = actually reachable target which does not need tracked progression
        SteamUserStats.SetAchievement("exampleAchievementKey"); // use keycode for achievement here!

        // store all stats AND achievements to steam
        SteamUserStats.StoreStats();
    }*/

    /// <summary>
    /// Call this function when unlocking an achievement at any place in the game by populating the correct ID
    /// 1 = Find crown
    /// </summary>
    /// <param name="_achievementID"></param>
    public void UnlockAchievement(int _achievementID)
    {
        switch (_achievementID)
        {
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

            case 11: // achievement: Finish a regular run in under 10 mins
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
