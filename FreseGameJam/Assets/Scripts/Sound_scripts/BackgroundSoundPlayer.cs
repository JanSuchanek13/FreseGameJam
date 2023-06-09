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
    [SerializeField] AudioSource _hardcoreMusic; // if a song is put in here, that will be the first song played on load
    [SerializeField] AudioSource[] _interruptingMusicSounds;

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
    }

    void PlayTrack()
    {
        if (_playThisSongFirst == null) // play random background track:
        {
            _randomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            //float _lengthOfTrack = _randomTrack.clip.length;
            //_randomTrack.Play();
            _activeTrack = _randomTrack;
            float _lengthOfTrack = _activeTrack.clip.length;
            _activeTrack.Play();

            Debug.Log("1");
            StartCoroutine(PlayNextTrack(_lengthOfTrack));
        }
        else // play a specific track first:
        {
            _playThisSongFirst.Play();
            _activeTrack = _playThisSongFirst;
            float _lengthOfTrack = _activeTrack.clip.length;

            if (!_playThisSongFirst.loop)
            {
                StartCoroutine(PlayNextTrack(_lengthOfTrack));
            }
        }

        // save volume at game start (this allows for player preferences to be relevant)
        _musicVolumeAtStart = _activeTrack.volume;
    }

    IEnumerator PlayNextTrack(float _lengthOfCurrentTrack)
    {
        Debug.Log("2, current tracks length: " + _activeTrack.clip.length);

        yield return new WaitForSeconds(_lengthOfCurrentTrack);
        
        Debug.Log("3, this should be clips lenght later");


        if (_playThisSongFirst == null) // This is to stop the next random track from playing in hardcore, as this is changed after being called!
        {
            if (_timePausedAmt == 0.0)
            {
                _nextRandomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];

                if (_nextRandomTrack == _randomTrack) // prevent the same song playing twice in a row:
                {
                    StartCoroutine(PlayNextTrack(0.0f));
                    Debug.Log("--> the same song was picked, retrying!");

                    yield break;
                }else
                {
                    Debug.Log("--> new track should start playing");

                    _randomTrack = _nextRandomTrack;
                }

                //_randomTrack.Play();
                //_activeTrack = _randomTrack;
                //float _lengthOfTrack = _randomTrack.clip.length;
                _activeTrack = _randomTrack;
                float _lengthOfTrack = _activeTrack.clip.length;
                _activeTrack.Play();
                Debug.Log("new track started playing!");

                StartCoroutine(PlayNextTrack(_lengthOfTrack));
            }
            else if(_turnMusicOffDuringCutscenes == true && _timePausedAmt != 0.0)
            {
                //Debug.Log("game was paused, now adding: " + _timePausedAmt + " to current track to allow to catch up!");
                float _savedPauseTime = _timePausedAmt;
                _timePausedAmt = 0.0f; // has to be reset here, otherwise after pausing, all future tracks will be delayed by this amount:
                StartCoroutine(PlayNextTrack(_savedPauseTime));
                yield break;
            }
            // the fix should be to count the time paused and then subtract that from the waittime,
            // as the pausing will stop passing of time. a 100sec track would play after 100 sec, if 20 sec
            // paused it will only commence counting after the 20 secs, so it would wait 120 sec...


            /*if (_timePausedAmt == 0.0)
            {
                _nextRandomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
                if (_nextRandomTrack == _randomTrack) // prevent the same song playing twice in a row:
                {
                    StartCoroutine(PlayNextTrack(0.0f));
                    yield break;
                }
                else
                {
                    _randomTrack = _nextRandomTrack;
                }

                _randomTrack.Play();
                _activeTrack = _randomTrack;
                float _lengthOfTrack = _randomTrack.clip.length;
                StartCoroutine(PlayNextTrack(_lengthOfTrack));
            }
            else
            {
                Debug.Log("game was paused, now adding: " + _timePausedAmt + " to current track to allow to catch up!");
                StartCoroutine(PlayNextTrack(_timePausedAmt));

                _timePausedAmt = 0.0f; // has to be reset here, otherwise after pausing, all future tracks will be delayed by this amount:

                yield break;
            }*/
        }
        else // ignore this function if a looping track was selected after this was calles (Hardcore mode does this).
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
            Debug.Log("this number should be zero: " + _timePausedAmt);
            _timePausedAmt += Time.deltaTime;
            Debug.Log("this much time was paused: " + _timePausedAmt);
        }
    }

    public void PlayHardcoreMusic()
    {
        StartCoroutine(WindDownMusic());
    }

    IEnumerator WindDownMusic()
    {
        AudioSource _randomScreetch = _interruptingMusicSounds[Random.Range(0, _interruptingMusicSounds.Length)];
        _randomTrack.Stop();
        _randomScreetch.Play();

        yield return new WaitForSeconds(_randomScreetch.clip.length);

        _playThisSongFirst = _hardcoreMusic;
        PlayTrack();
    }
}