using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedRunLeadManager : MonoBehaviour
{
    private SteamLeaderboard_t s_currentLeaderboard;
    private bool s_initialized = false;
    private CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>();
    private CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>();
    private CallResult<LeaderboardScoresDownloaded_t> m_downloadResult = new CallResult<LeaderboardScoresDownloaded_t>();

    [SerializeField]
    List<TMP_Text> LeaderboardNames = new List<TMP_Text>();
    [SerializeField]
    List<TMP_Text> LeaderboardTimes = new List<TMP_Text>();
    [SerializeField]
    TMP_Text YourName;
    [SerializeField]
    TMP_Text YourTime;
    [SerializeField]
    TMP_Text YourRank;

    public void Debug_SetScore(int _score)
    {
        //UpdateScore(_score);

        GetLeaderBoardData();
    }

    public struct LeaderboardData
    {
        public string username;
        public int rank;
        public int score;
    }
    List<LeaderboardData> LeaderboardDataset;

    public void UpdateScore(int score)
    {
        if (!s_initialized)
        {
            Debug.LogError("Leaderboard not initialized");
        }
        else
        {
            //Change upload method to 
            SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
            m_uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
        }
    }

    private void Awake()
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard("Ori Speedrun Best Time1");
        m_findResult.Set(hSteamAPICall, OnLeaderboardFindResult);

        CSteamID[] Users = { SteamUser.GetSteamID() }; // Local user steam id
        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntriesForUsers(s_currentLeaderboard, Users, Users.Length);
        //Debug.Log(Users[0]);
    }

    private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Find: Did it fail? {failure}, Found: {pCallback.m_bLeaderboardFound}, leaderboardID: {pCallback.m_hSteamLeaderboard.m_SteamLeaderboard}, Name: {pCallback.m_hSteamLeaderboard.m_SteamLeaderboard.ToString()}");
        s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
        s_initialized = true;
    }

    private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Upload: Did it fail? {failure}, Score: {pCallback.m_nScore}, HasChanged: {pCallback.m_bScoreChanged}");
    }

    //change ELeaderboardDataRequest to get a different set (focused around player or global)
    public void GetLeaderBoardData(ELeaderboardDataRequest _type = ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, int entries = 14)
    {
        SteamAPICall_t hSteamAPICall;
        switch (_type)
        {
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal:
                hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboard, _type, 1, entries);
                m_downloadResult.Set(hSteamAPICall, OnLeaderboardDownloadResult);
                break;
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser:
                hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboard, _type, -(entries / 2), (entries / 2));
                m_downloadResult.Set(hSteamAPICall, OnLeaderboardDownloadResult);
                break;
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends:
                hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboard, _type, 1, entries);
                m_downloadResult.Set(hSteamAPICall, OnLeaderboardDownloadResult);
                break;
        }
        //Note that the LeaderboardDataset will not be updated immediatly (see callback below)
    }

    private void OnLeaderboardDownloadResult(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Download: Did it fail? {failure}, Result - {pCallback.m_hSteamLeaderboardEntries}");
        LeaderboardDataset = new List<LeaderboardData>();
        //Iterates through each entry gathered in leaderboard
        for (int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            LeaderboardEntry_t leaderboardEntry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out leaderboardEntry, null, 0);
            //Example of how leaderboardEntry might be held/used
            LeaderboardData lD;
            lD.username = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);
            lD.rank = leaderboardEntry.m_nGlobalRank;
            lD.score = leaderboardEntry.m_nScore;
            LeaderboardDataset.Add(lD);
            Debug.Log($"User: {lD.username} - Score: {lD.score} - Rank: {lD.rank}");
            LeaderboardNames[i].text = lD.username;
            int timer = lD.score;
            int minutes = (int)timer / 60000;
            int seconds = (int)timer / 1000 % 60;
            int milliseconds = (int)timer % 1000;
            LeaderboardTimes[i].text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            // Überprüfen, ob die Steam-ID der Eintrag des lokalen Benutzers entspricht
            if (leaderboardEntry.m_steamIDUser == SteamUser.GetSteamID())
            {
                // Die Position des Spielers ist gegeben durch leaderboardEntry.m_nGlobalRank
                int playerRank = leaderboardEntry.m_nGlobalRank;
                int playerTime = leaderboardEntry.m_nScore / 1000;
                int playertimer = playerTime;
                int playerminutes = (int)timer / 60000;
                int playerseconds = (int)timer / 1000 % 60;
                int playermilliseconds = (int)timer % 1000;
                Debug.Log("Spieler ist auf Position " + playerRank + " im Leaderboard mit" + playerTime);
                YourRank.text = playerRank.ToString();
                YourTime.text = string.Format("{0:00}:{1:00}:{2:000}", playerminutes, playerseconds, playermilliseconds);
                YourName.text = SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID());
                if(playerRank == 1)
                {
                    PlayerPrefs.SetInt("WasFastestHardcoreRunner" + 0, 1);
                }
            }
        }
        //This is the callback for my own project - function is asynchronous so it must return from here rather than from GetLeaderBoardData
        //FindObjectOfType<HighscoreUIMan>().FillLeaderboard(LeaderboardDataset);
    }
}