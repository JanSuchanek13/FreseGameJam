using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundPlayer : MonoBehaviour
{
    #region variables:
    [Header("Insert all Background-Music AudioSources here:")]
    [SerializeField] AudioSource[] arrayOfBackgroundMusic; // put all backgroundtracks in here.
    [Space(10)]

    [Header("Background Sound Settings:")]
    [SerializeField] bool _turnMusicOffDuringCutscenes = true;
    [SerializeField] bool _lowerMusicVolumeDuringCutscenes = false;
    [SerializeField] AudioSource _playThisSongFirst; // if a song is put in here, that will be the first song played on load

    private AudioSource _randomTrack;
    private AudioSource _nextRandomTrack;
    private AudioSource _activeTrack; // this is for "FocusPlayerViewOnObject"-Script to pause the background music on demand;

    [SerializeField] float _lowerVolumeTo = 0.5f;
    float _timePausedAmt = 0.0f;
    bool _timePaused;

    float _musicVolumeAtStart;
    #endregion

    private void Start()
    {
        Invoke("PlayTrack", .1f);

        // save volume at game start (this allows for player preferences to be relevant)
        //_musicVolumeAtStart = _activeTrack.volume;
    }

    void PlayTrack()
    {
        if (_playThisSongFirst == null) // play random background track:
        {
            _randomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            float _lengthOfTrack = _randomTrack.clip.length;
            _randomTrack.Play();
            _activeTrack = _randomTrack;

            StartCoroutine(PlayNextTrack(_lengthOfTrack));
        }else // play a specific track first:
        {
            _playThisSongFirst.Play();
            _activeTrack = _playThisSongFirst;
            float _lengthOfTrack = _activeTrack.clip.length;

            StartCoroutine(PlayNextTrack(_lengthOfTrack));
        }

        // save volume at game start (this allows for player preferences to be relevant)
        _musicVolumeAtStart = _activeTrack.volume;
    }

    IEnumerator PlayNextTrack(float _lengthOfCurrentTrack)
    {
        yield return new WaitForSeconds(_lengthOfCurrentTrack);

        if (_timePausedAmt == 0.0)
        {
            _nextRandomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            if (_nextRandomTrack == _randomTrack) // prevent the same song playing twice in a row:
            {
                StartCoroutine(PlayNextTrack(0.0f));
                yield break;
            }else
            {
                _randomTrack = _nextRandomTrack;
            }

            _randomTrack.Play();
            _activeTrack = _randomTrack;
            float _lengthOfTrack = _randomTrack.clip.length;
            StartCoroutine(PlayNextTrack(_lengthOfTrack));
        }else
        {
            Debug.Log("2");

            Debug.Log("game was paused, now adding: " + _timePausedAmt + " to current track to allow to catch up!");
            StartCoroutine(PlayNextTrack(_timePausedAmt));

            _timePausedAmt = 0.0f; // has to be reset here, otherwise after pausing, all future tracks will be delayed by this amount:
            
            yield break;
        }
    }

    public void LowerVolume()
    {
        if(_activeTrack.volume == _musicVolumeAtStart) // prevent multiple volume reductions in chained cutscenes:
        {
            _activeTrack.volume *= _lowerVolumeTo;
        }
    }
    public void IncreaseVolume() // always reset to max sound of the players preference:
    {
        _activeTrack.volume = _musicVolumeAtStart;
    }

    public void PauseMusic()
    {
        if (_turnMusicOffDuringCutscenes)
        {
            _activeTrack.Pause();
            StartPauseTimer();
        }
        if (_lowerMusicVolumeDuringCutscenes)
        {
            LowerVolume();
        }
    }
    public void TurnOffMusic()
    {
        _activeTrack.Stop();
    }

    public void UnpauseMusic()
    {
        if (_turnMusicOffDuringCutscenes)
        {
            _activeTrack.UnPause();
            EndPauseTimer();
        }
        if (_lowerMusicVolumeDuringCutscenes)
        {
            IncreaseVolume();
        }
    }

    void StartPauseTimer()
    {
        _timePaused = true;
    }

    void EndPauseTimer()
    {
        _timePaused = false;
    }

    private void Update()
    {
        if (_timePaused)
        {
            _timePausedAmt += Time.deltaTime;
        }
    }
}