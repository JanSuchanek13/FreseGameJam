using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeadMan : MonoBehaviour
{
    private SteamLeaderboard_t s_currentLeaderboard;
    private SteamLeaderboard_t s_currentLeaderboardTime;
    private bool s_initialized = false;
    private CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>();
    private CallResult<LeaderboardFindResult_t> m_findResultTime = new CallResult<LeaderboardFindResult_t>();
    private CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>();
    private CallResult<LeaderboardScoreUploaded_t> m_uploadResultTime = new CallResult<LeaderboardScoreUploaded_t>();
    private CallResult<LeaderboardScoresDownloaded_t> m_downloadResult = new CallResult<LeaderboardScoresDownloaded_t>();
    private CallResult<LeaderboardScoresDownloaded_t> m_downloadResultTime = new CallResult<LeaderboardScoresDownloaded_t>();

    [SerializeField]
    List<TMP_Text> LeaderboardNames = new List<TMP_Text>();
    [SerializeField]
    List<TMP_Text> LeaderboardCrowns = new List<TMP_Text>();
    [SerializeField]
    List<TMP_Text> LeaderboardTimes = new List<TMP_Text>();
    [SerializeField]
    TMP_Text YourName;
    [SerializeField]
    TMP_Text YourCrowns;
    [SerializeField]
    TMP_Text YourCrownTime;
    [SerializeField]
    TMP_Text YourRank;

    public struct LeaderboardData
    {
        public string username;
        public int rank;
        public int score;
    }
    List<LeaderboardData> LeaderboardDataset;

    public struct LeaderboardDataTime
    {
        public string usernameTime;
        public int rankTime;
        public int scoreTime;
    }
    List<LeaderboardDataTime> LeaderboardDatasetTime;

    public struct PlayerLeaderboardData
    {
        public string username;
        public int crowns;
        public int time;
    }

    private int currentCrownTime;

    public void Debug_SetScore(int _score)
    {
        //UpdateScore(_score);

        //GetLeaderBoardData();// Daten aus "Crown Leaderboard" abrufen
        GetLeaderBoardData(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 14);

        // Warte auf die Callback-Funktion, um die Daten zu erhalten (das kann asynchron sein)
        // In der Callback-Funktion OnLeaderboardDownloadResult wird die Liste LeaderboardDataset gefüllt.

        // Daten aus "Crown Time Leaderboard" abrufen
        GetLeaderBoardDataTime(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 14);
        Invoke("SortLeaderboard", 1f);
    }

    private void SortLeaderboard()
    {
        SortCombinedLeaderboard(GetCombinedLeaderboardData());
    }

    public void UpdateScore(int score, int scoreTime)
    {
        currentCrownTime = scoreTime;
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

    public void UpdateScoreTime()
    {
        if (!s_initialized)
        {
            Debug.LogError("Leaderboard not initialized");
        }
        else
        {
            //Change upload method to 
            SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboardTime, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, currentCrownTime, null, 0);
            m_uploadResultTime.Set(hSteamAPICall, OnLeaderboardUploadResultTime);
        }
    }

    private void Awake()
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard("Crowns1");
        m_findResult.Set(hSteamAPICall, OnLeaderboardFindResult);

        SteamAPICall_t hSteamAPICallTime = SteamUserStats.FindLeaderboard("Crowns Time1");
        m_findResultTime.Set(hSteamAPICallTime, OnLeaderboardFindResultTime);

        CSteamID[] Users = { SteamUser.GetSteamID() }; // Local user steam id
        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntriesForUsers(s_currentLeaderboard, Users, Users.Length);
        //Debug.Log(Users[0]);
    }

    private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Find: Did it fail? {failure}, Found: {pCallback.m_bLeaderboardFound}, leaderboardID: {pCallback.m_hSteamLeaderboard.m_SteamLeaderboard}");
        s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
        s_initialized = true;
    }

    private void OnLeaderboardFindResultTime(LeaderboardFindResult_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Find: Did it fail? {failure}, Found: {pCallback.m_bLeaderboardFound}, leaderboardID: {pCallback.m_hSteamLeaderboard.m_SteamLeaderboard}");
        s_currentLeaderboardTime = pCallback.m_hSteamLeaderboard;
        s_initialized = true;
    }

    private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Upload: Did it fail? {failure}, Score: {pCallback.m_nScore}, HasChanged: {pCallback.m_bScoreChanged}");
        if (pCallback.m_bScoreChanged == 1)
        {
            UpdateScoreTime();
        }
        else if(PlayerPrefs.GetInt("HardcoreCrowns" + 0) == pCallback.m_nScore)
        {
            UpdateScoreTime();
        }
    }

    private void OnLeaderboardUploadResultTime(LeaderboardScoreUploaded_t pCallback, bool failure)
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

    public void GetLeaderBoardDataTime(ELeaderboardDataRequest _type = ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, int entries = 14)
    {
        SteamAPICall_t hSteamAPICall;
        switch (_type)
        {
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal:
                hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboardTime, _type, 1, entries);
                m_downloadResultTime.Set(hSteamAPICall, OnLeaderboardDownloadResultTime);
                break;
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser:
                hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboardTime, _type, -(entries / 2), (entries / 2));
                m_downloadResultTime.Set(hSteamAPICall, OnLeaderboardDownloadResultTime);
                break;
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends:
                hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboardTime, _type, 1, entries);
                m_downloadResultTime.Set(hSteamAPICall, OnLeaderboardDownloadResultTime);
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
            //LeaderboardNames[i].text = lD.username;
            //LeaderboardCrowns[i].text = lD.score.ToString();

            /*
            // Überprüfen, ob die Steam-ID der Eintrag des lokalen Benutzers entspricht
            if (leaderboardEntry.m_steamIDUser == SteamUser.GetSteamID())
            {
                // Die Position des Spielers ist gegeben durch leaderboardEntry.m_nGlobalRank
                int playerRank = leaderboardEntry.m_nGlobalRank;
                int playerCrowns = leaderboardEntry.m_nScore;
                Debug.Log("Spieler ist auf Position " + playerRank + " im Leaderboard.");
                YourRank.text = playerRank.ToString();
                YourCrowns.text = playerCrowns.ToString();
                YourName.text = SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID());
                if (playerRank == 1)
                {
                    PlayerPrefs.SetInt("WasFastestHardcoreCrownRunner" + 0, 1);
                }
            }
            */
        }
        //This is the callback for my own project - function is asynchronous so it must return from here rather than from GetLeaderBoardData
        //FindObjectOfType<HighscoreUIMan>().FillLeaderboard(LeaderboardDataset);
    }

    private void OnLeaderboardDownloadResultTime(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        Debug.Log($"Steam Leaderboard Download Time: Did it fail? {failure}, Result - {pCallback.m_hSteamLeaderboardEntries}");
        LeaderboardDatasetTime = new List<LeaderboardDataTime>();
        // Iteriere durch jeden Eintrag im Leaderboard "Crown Time"
        for (int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            LeaderboardEntry_t leaderboardEntry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out leaderboardEntry, null, 0);
            // Beispiel, wie leaderboardEntry genutzt werden kann
            LeaderboardDataTime lDTime;
            lDTime.usernameTime = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);
            lDTime.rankTime = leaderboardEntry.m_nGlobalRank;
            lDTime.scoreTime = leaderboardEntry.m_nScore;
            LeaderboardDatasetTime.Add(lDTime);
            Debug.Log($"User: {lDTime.usernameTime} - Score: {lDTime.scoreTime} - Rank: {lDTime.rankTime}");
            // Hier kannst du die gefundenen Daten verarbeiten, wie es für dein Projekt erforderlich ist.
        }
        // Falls du die Daten in deinem Projekt verwenden möchtest, könntest du eine entsprechende Funktion aufrufen, um die Daten anzuzeigen oder zu verarbeiten.
    }

    private List<PlayerLeaderboardData> GetCombinedLeaderboardData()
    {
        List<PlayerLeaderboardData> combinedLeaderboardData = new List<PlayerLeaderboardData>();

        

        
        // Warte auf die Callback-Funktion, um die Daten zu erhalten (das kann asynchron sein)
        // In der Callback-Funktion OnLeaderboardDownloadResultTime wird die Liste LeaderboardDatasetTime gefüllt.

        // Kombiniere die Daten aus beiden Leaderboards
        foreach (var crownData in LeaderboardDataset)
        {
            // Finde die entsprechenden Daten aus dem "Crown Time Leaderboard" für den gleichen Benutzer
            var matchingTimeData = LeaderboardDatasetTime.Find(data => data.usernameTime == crownData.username);

            PlayerLeaderboardData playerData = new PlayerLeaderboardData
            {
                username = crownData.username,
                crowns = crownData.score,
                time = matchingTimeData.scoreTime  // Falls keine Übereinstimmung gefunden wurde, verwende 0 für die Zeit.
            };

            combinedLeaderboardData.Add(playerData);
        }

        return combinedLeaderboardData;
    }


    private void SortCombinedLeaderboard(List<PlayerLeaderboardData> combinedLeaderboardData)
    {
        combinedLeaderboardData.Sort((player1, player2) =>
        {
            // Sortiere zuerst nach Kronen (absteigend)
            int crownComparison = player2.crowns.CompareTo(player1.crowns);

            // Wenn die Kronen gleich sind, sortiere nach Zeit (aufsteigend)
            if (crownComparison == 0)
            {
                return player1.time.CompareTo(player2.time);
            }

            return crownComparison;
        });

        int i = 0;
        foreach (var playerData in combinedLeaderboardData)
        {
            

            Debug.Log("Username: " + playerData.username + ", Crowns: " + playerData.crowns + ", Time: " + playerData.time + i);
            LeaderboardNames[i].text = playerData.username;
            LeaderboardCrowns[i].text = playerData.crowns.ToString();
            int timer = playerData.time;
            int minutes = (int)timer / 60000;
            int seconds = (int)timer / 1000 % 60;
            int milliseconds = (int)timer % 1000;
            LeaderboardTimes[i].text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            CSteamID steamID = SteamUser.GetSteamID();
            string userName = SteamFriends.GetFriendPersonaName(steamID);
            // Überprüfen, ob die Steam-ID der Eintrag des lokalen Benutzers entspricht
            if (playerData.username == userName)
            {
                int playerRank = i+1;
                int playerCrowns = playerData.crowns;
                Debug.Log("Spieler ist auf Position " + playerRank + " im Leaderboard.");
                YourRank.text = playerRank.ToString();
                YourCrowns.text = playerCrowns.ToString();
                YourCrownTime.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
                YourName.text = SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID());
                if (playerRank == 1)
                {
                    PlayerPrefs.SetInt("WasFastestHardcoreCrownRunner" + 0, 1);
                }
            }


            i++;
        }
    }

}