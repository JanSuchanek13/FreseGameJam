using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;


public class SteamStatsManager : MonoBehaviour
{
    // reference: nominator / demoninator, ex: 4567 seconds reg runtime, 5 runs completed = avg runtime of (4567/60)/5 = 76,117min/5 = 15.223min per run on AVG

    private Dictionary<string, float> _unsentUpdates = new Dictionary<string, float>();
    private const string UNSAVED_UPDATES_KEY = "UnsentStatUpdates";

    public enum TrackableStatTypes
    {
        STAT_INT = 0,
        STAT_FLOAT = 1,
        STAT_AVGRATE = 2,
    }

    [System.Serializable]
    public struct Stat_t
    {
        [Tooltip("This is the name of the stat as it is recognized by Steamworks. This string would be used when communicating with the Steamworks API to set, get, or update the stat.")]
        public string _steamStatKey; // use string instead of const char* for Unity compatibility.
        public TrackableStatTypes _statType;
        public int _intValueToTrack;
        public float _floatValueToTrack;
        public float _avgNumerator;
        public float _avgDenominator;
    }

    [SerializeField] private Stat_t[] _statsToTrack; // array of stats to track. Populate this in the inspector.

    private void Start()
    {
        LoadCachedUpdates();  // load any previously cached data first

        if (!SteamManager.Initialized)
        {
            Debug.LogError("SteamManager is not initialized. Ensure you've initialized Steamworks.NET.");
            return;
        }

        // fetch the current stats from Steam at the start.
        FetchStats();
    }

    /// <summary>
    /// When called this function updates our tracked stats from anywhere in the game.
    /// Added logic to cache data if steam is not initialized.
    /// List of locations this is called:
    /// --> OnTriggerEnter(Collider other) in the CrownCounter-script (reg lifetime crowns) (called whenever a crown is collected in runtime)
    /// --> OnTriggerEnter(Collider other) in the CrownCounter-script (hc lifetime crowns) (called whenever a crown is collected in runtime)
    /// --> SaveCraneTime() in the SteamAchievementsManager-script (lifetime glide time) (called whenever achievement-milestone is reached in runtime or application is closed)
    /// --> OnTriggerEnter(Collider other) in the LevelScript-script (lieftime runs completed) (called whenever any run has reached the endzone)
    /// --> OnTriggerEnter(Collider other) in the HiddenLocationExplorerAchievementTrigger-script (secret locations found) (called whenever a secret location is found in runtime)
    /// </summary>
    /// <param name="statName"></param>
    /// <param name="value"></param>
    public void UpdateStat(string statName, float value)
    {
        // check if Steam is initialized
        if (!SteamManager.Initialized)
        {
            // cache the unsent update
            if (_unsentUpdates.ContainsKey(statName))
            {
                _unsentUpdates[statName] += value;
            }else
            {
                _unsentUpdates.Add(statName, value);
            }

            Debug.LogWarning($"Steam not initialized. Caching stat update for {statName}. Total cached updates: {_unsentUpdates.Count}");
            return;
        }

        // if there are any cached updates, apply them
        if (_unsentUpdates.Count > 0)
        {
            foreach (var cachedUpdate in _unsentUpdates)
            {
                ApplyStatUpdate(cachedUpdate.Key, cachedUpdate.Value);
            }
            _unsentUpdates.Clear();
            Debug.Log("Applied cached stat updates.");
        }

        // apply the current update
        ApplyStatUpdate(statName, value);
    }

    private void SaveCachedUpdates()
    {
        string jsonData = JsonUtility.ToJson(new SerializableDictionary<string, float>(_unsentUpdates));
        PlayerPrefs.SetString(UNSAVED_UPDATES_KEY, jsonData);
        PlayerPrefs.Save();
    }

    private void LoadCachedUpdates()
    {
        if (PlayerPrefs.HasKey(UNSAVED_UPDATES_KEY))
        {
            string jsonData = PlayerPrefs.GetString(UNSAVED_UPDATES_KEY);
            SerializableDictionary<string, float> loadedData = JsonUtility.FromJson<SerializableDictionary<string, float>>(jsonData);
            _unsentUpdates = loadedData.ToDictionary();
        }
    }

    // due to Unity's limitations with JsonUtility and dictionaries, we need a helper class:
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private List<TKey> keys;
        [SerializeField] private List<TValue> values;

        public SerializableDictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            keys = new List<TKey>(dictionary.Keys);
            values = new List<TValue>(dictionary.Values);
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i], values[i]);
            }
            return result;
        }
    }

    public void ApplyStatUpdate(string statName, float value)
    {
        for (int i = 0; i < _statsToTrack.Length; i++)
        {
            if (_statsToTrack[i]._steamStatKey == statName)
            {
                switch (_statsToTrack[i]._statType)
                {
                    case TrackableStatTypes.STAT_INT:
                        _statsToTrack[i]._intValueToTrack += (int)value;
                        SteamUserStats.SetStat(statName, _statsToTrack[i]._intValueToTrack);

                        Debug.Log($"Stat '{statName}' of type INT updated by {value}. New total: {_statsToTrack[i]._intValueToTrack}");
                        break;

                    case TrackableStatTypes.STAT_FLOAT:
                        _statsToTrack[i]._floatValueToTrack += value;
                        SteamUserStats.SetStat(statName, _statsToTrack[i]._floatValueToTrack);

                        Debug.Log($"Stat '{statName}' of type FLOAT updated by {value}. New total: {_statsToTrack[i]._floatValueToTrack}");
                        break;

                    case TrackableStatTypes.STAT_AVGRATE:
                        _statsToTrack[i]._avgNumerator += value;
                        _statsToTrack[i]._avgDenominator += 1; // this assumes were only ever adding a single count to the denominator (eg. 1 finished run), for more complex data, like tracking damage dealt to taken ratio etc
                                                               // we might want to have another method to update the denominator differently.
                        SteamUserStats.SetStat(statName + "_Numerator", _statsToTrack[i]._avgNumerator);
                        SteamUserStats.SetStat(statName + "_Denominator", _statsToTrack[i]._avgDenominator);

                        float newAverage = _statsToTrack[i]._avgNumerator / _statsToTrack[i]._avgDenominator;
                        Debug.Log($"Stat '{statName}' of type AVGRATE updated. Newest change: {value} against total runs: {_statsToTrack[i]._avgDenominator}. New average: {newAverage}");
                        break;
                }
                break;
            }
        }

        // store stats after updating.
        SteamUserStats.StoreStats();
    }

    private void FetchStats()
    {
        for (int i = 0; i < _statsToTrack.Length; i++)
        {
            switch (_statsToTrack[i]._statType)
            {
                case TrackableStatTypes.STAT_INT:
                    int intValue;
                    if (SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey, out intValue))
                    {
                        _statsToTrack[i]._intValueToTrack = intValue;
                    }
                    break;

                case TrackableStatTypes.STAT_FLOAT:
                    float floatValue;
                    if (SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey, out floatValue))
                    {
                        _statsToTrack[i]._floatValueToTrack = floatValue;
                    }
                    break;

                case TrackableStatTypes.STAT_AVGRATE:
                    float avgNumerator;
                    float avgDenominator;

                    if (SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey + "_Numerator", out avgNumerator)
                    && SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey + "_Denominator", out avgDenominator))
                    {
                        _statsToTrack[i]._avgNumerator = avgNumerator;
                        _statsToTrack[i]._avgDenominator = avgDenominator;
                    }
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        // check if Steam is initialized before storing stats
        if (SteamManager.Initialized)
        {
            // store stats when application quits.
            SteamUserStats.StoreStats();

            // if there are still unsent updates in the cache, save them to PlayerPrefs. Eg if the player launched steam halfway through the session.
            if (_unsentUpdates.Count > 0)
            {
                SaveCachedUpdates();
                Debug.LogWarning("Cached stats saved on application quit. They will be uploaded in the next session if Steam is initialized then.");
            }
        }else
        {
            // save any unsent updates to PlayerPrefs
            SaveCachedUpdates();
            Debug.LogWarning("Steam not initialized. Stats not stored to Steam on application quit. However, they have been cached.");
        }
    }

    // safety which does NOT cache data when steam is offline:
    /* 
    // reference: nominator / demoninator, ex: 4567 seconds reg runtime, 5 runs completed = avg runtime of (4567/60)/5 = 76,117min/5 = 15.223min per run on AVG
    public enum TrackableStatTypes
    {
        STAT_INT = 0,
        STAT_FLOAT = 1,
        STAT_AVGRATE = 2,
    }

    [System.Serializable]
    public struct Stat_t
    {
        [Tooltip("This is the name of the stat as it is recognized by Steamworks. This string would be used when communicating with the Steamworks API to set, get, or update the stat.")]
        public string _steamStatKey; // use string instead of const char* for Unity compatibility.
        public TrackableStatTypes _statType;
        public int _intValueToTrack;
        public float _floatValueToTrack;
        public float _avgNumerator;
        public float _avgDenominator;
    }

    [SerializeField]
    private Stat_t[] _statsToTrack; // array of stats to track. Populate this in the inspector.

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("SteamManager is not initialized. Ensure you've initialized Steamworks.NET.");
            return;
        }

        // fetch the current stats from Steam at the start.
        FetchStats();
    }
    /// <summary>
    /// When called this function updates our tracked stats from anywhere in the game.
    /// List of locations this is called:
    /// 
    /// </summary>
    /// <param name="statName"></param>
    /// <param name="value"></param>
    public void UpdateStat(string statName, float value)
    {
        for (int i = 0; i < _statsToTrack.Length; i++)
        {
            if (_statsToTrack[i]._steamStatKey == statName)
            {
                switch (_statsToTrack[i]._statType)
                {
                    case TrackableStatTypes.STAT_INT:
                        _statsToTrack[i]._intValueToTrack += (int)value;
                        SteamUserStats.SetStat(statName, _statsToTrack[i]._intValueToTrack);

                        Debug.Log($"Stat '{statName}' of type INT updated by {value}. New total: {_statsToTrack[i]._intValueToTrack}");
                        break;

                    case TrackableStatTypes.STAT_FLOAT:
                        _statsToTrack[i]._floatValueToTrack += value;
                        SteamUserStats.SetStat(statName, _statsToTrack[i]._floatValueToTrack);

                        Debug.Log($"Stat '{statName}' of type FLOAT updated by {value}. New total: {_statsToTrack[i]._floatValueToTrack}");
                        break;

                    case TrackableStatTypes.STAT_AVGRATE:
                        _statsToTrack[i]._avgNumerator += value;
                        _statsToTrack[i]._avgDenominator += 1; // For instance, we're assuming every call adds one event/instance.
                                                                  // You might want to have another method to update the denominator differently.
                        SteamUserStats.SetStat(statName + "_Numerator", _statsToTrack[i]._avgNumerator);
                        SteamUserStats.SetStat(statName + "_Denominator", _statsToTrack[i]._avgDenominator);

                        float newAverage = _statsToTrack[i]._avgNumerator / _statsToTrack[i]._avgDenominator;
                        Debug.Log($"Stat '{statName}' of type AVGRATE updated. Newest change: {value} against total runs: {_statsToTrack[i]._avgDenominator}. New average: {newAverage}");
                        break;
                }
                break;
            }
        }

        // Store stats after updating.
        SteamUserStats.StoreStats();
    }

    private void FetchStats()
    {
        for (int i = 0; i < _statsToTrack.Length; i++)
        {
            switch (_statsToTrack[i]._statType)
            {
                case TrackableStatTypes.STAT_INT:
                    int intValue;
                    if (SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey, out intValue))
                    {
                        _statsToTrack[i]._intValueToTrack = intValue;
                    }
                    break;

                case TrackableStatTypes.STAT_FLOAT:
                    float floatValue;
                    if (SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey, out floatValue))
                    {
                        _statsToTrack[i]._floatValueToTrack = floatValue;
                    }
                    break;

                case TrackableStatTypes.STAT_AVGRATE:
                    float avgNumerator;
                    float avgDenominator;

                    if (SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey + "_Numerator", out avgNumerator)
                    && SteamUserStats.GetStat(_statsToTrack[i]._steamStatKey + "_Denominator", out avgDenominator))
                    {
                        _statsToTrack[i]._avgNumerator = avgNumerator;
                        _statsToTrack[i]._avgDenominator = avgDenominator;
                    }
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Optional: Store stats when application quits.
        SteamUserStats.StoreStats();
    }*/
}