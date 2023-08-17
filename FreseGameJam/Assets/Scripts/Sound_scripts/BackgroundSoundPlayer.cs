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
    [SerializeField] AudioSource[] _arrayOfHardcoreMusic; // if a song is put in here, that will be the first song played on load
    [SerializeField] AudioSource[] _interruptingMusicSounds;

    private AudioSource _randomTrack;
    private AudioSource _nextRandomTrack;
    private AudioSource _activeTrack; // this is for "FocusPlayerViewOnObject"-Script to pause the background music on demand;

    [SerializeField] float _lowerVolumeTo = 0.5f;
    float _timePausedAmt = 0.0f;
    bool _timePaused;
    bool _thisIsHardcore = false;

    float _musicVolumeAtStart;
    #endregion

    private void Start()
    {
        Invoke("PlayTrack", .1f);
    }

    void PlayTrack()
    {
        if (_playThisSongFirst == null) // play random background track:
        {
            _randomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            _activeTrack = _randomTrack;
            //float _lengthOfTrack = _activeTrack.clip.length;
            _activeTrack.Play();

            //Debug.Log("1");
            //StartCoroutine(PlayNextTrack(_lengthOfTrack));
            StartCoroutine(PlayNextTrack());
        }
        else // play a specific track first:
        {
            _playThisSongFirst.Play();
            _activeTrack = _playThisSongFirst;
            //float _lengthOfTrack = _activeTrack.clip.length;

            if (!_playThisSongFirst.loop)
            {
                //StartCoroutine(PlayNextTrack(_lengthOfTrack));
                StartCoroutine(PlayNextTrack());
            }
        }

        // save volume at game start (this allows for player preferences to be relevant)
        _musicVolumeAtStart = _activeTrack.volume;
    }

    /// <summary>
    /// The logic for pausing music is no longer part of this function, if we want that back, ill add it back in!
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayNextTrack()
    {
        yield return new WaitWhile(() => _activeTrack.isPlaying);

        //if (_playThisSongFirst == null && !_turnMusicOffDuringCutscenes) // This is to stop the next random track from playing in hardcore, as this is changed after being called!
        if (!_thisIsHardcore && !_turnMusicOffDuringCutscenes) 
        {
            _nextRandomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            if (_nextRandomTrack == _randomTrack) // prevent the same song playing twice in a row:
            {
                StartCoroutine(PlayNextTrack());
                yield break;
            }else
            {
                _randomTrack = _nextRandomTrack;
                _activeTrack = _randomTrack;
                _activeTrack.Play();
                StartCoroutine(PlayNextTrack());
            }
        }else if (_thisIsHardcore)
        {
            _randomTrack = _arrayOfHardcoreMusic[Random.Range(0, _arrayOfHardcoreMusic.Length)];

            // make sure there is more than one track in the array:
            // --> otherwise the next track would always be the same as the previous one!!!
            if (_arrayOfHardcoreMusic.Length > 0)
            {
                if (_nextRandomTrack == _randomTrack) // prevent the same song playing twice in a row:
                {
                    StartCoroutine(PlayNextTrack());
                    yield break;
                }else
                {
                    _nextRandomTrack = _randomTrack;
                    _activeTrack = _randomTrack;
                    _activeTrack.Play();
                    StartCoroutine(PlayNextTrack());
                }
            }else // in case there is only one track, just play that on repeat:
            {
                _activeTrack = _randomTrack;
                _activeTrack.Play();
                StartCoroutine(PlayNextTrack());
            }
        }
        else
        {
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
        // changed this to make end of run more juicy by playing screetch:
        AudioSource _randomScreetch = _interruptingMusicSounds[Random.Range(0, _interruptingMusicSounds.Length)];
        _activeTrack.Stop();
        _activeTrack = _randomScreetch;
        _activeTrack.Play();
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
            Debug.Log("this number should be zero: " + _timePausedAmt);
            _timePausedAmt += Time.deltaTime;
            Debug.Log("this much time was paused: " + _timePausedAmt);
        }
    }

    public void StartHardcoreMusic()
    {
        _thisIsHardcore = true;

        AudioSource _randomScreetch = _interruptingMusicSounds[Random.Range(0, _interruptingMusicSounds.Length)];

        _activeTrack.Stop();
        _activeTrack = _randomScreetch;
        _activeTrack.Play();

        StartCoroutine(PlayNextTrack());
        //WindDownMusic();
    }

    /*
    void WindDownMusic()
    {
        AudioSource _randomScreetch = _interruptingMusicSounds[Random.Range(0, _interruptingMusicSounds.Length)];

        _activeTrack.Stop();
        _activeTrack = _randomScreetch;
        _activeTrack.Play();

        StartCoroutine(PlayNextTrack());
    }*/
}