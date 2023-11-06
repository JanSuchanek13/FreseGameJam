using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BackgroundSoundPlayer : MonoBehaviour
{
    #region variables:
    [Header("Insert all Background-Music AudioSources here:")]
    [SerializeField] AudioSource[] arrayOfBackgroundMusic; // put all backgroundtracks in here.
    [Space(10)]

    [Header("Background Sound Settings:")]
    [SerializeField] bool _playMusicOnLoad = true; // this is set to false in start menu to allow music to start after warning
    
    [Space(10)]
    [SerializeField] bool _turnMusicOffDuringCutscenes = true;
    [SerializeField] bool _lowerMusicVolumeDuringCutscenes = false;
    [SerializeField] AudioSource _playThisSongFirst; // if a song is put in here, that will be the first song played on load
    [SerializeField] AudioSource[] _arrayOfHardcoreMusic; // if a song is put in here, that will be the first song played on load
    [SerializeField] AudioSource[] _interruptingMusicSounds;

    [Space(10)]
    [SerializeField] bool _bandSignedContract = false;
    [SerializeField] GameObject _songTitleUI;
    [SerializeField] TextMeshProUGUI _songTitleTextWindow;
    [SerializeField] float _showSongTitleThisLong = 5.0f;

    private AudioSource _randomTrack;
    private AudioSource _nextRandomTrack;
    private AudioSource _activeTrack; // this is for "FocusPlayerViewOnObject"-Script to pause the background music on demand;

    [Space(10)]
    [SerializeField] float _lowerVolumeTo = 0.5f;
    float _timePausedAmt = 0.0f;
    bool _timePaused;
    bool _thisIsHardcore = false;

    float _musicVolumeAtStart;
    #endregion

    private void Start()
    {
        if (_playMusicOnLoad)
        {
            Invoke("PlayTrack", .1f);
            Debug.Log("play music was called");
        }
    }

    /// <summary>
    /// This is public so the sound can be started after displaying the epilepsy warning in the start menu.
    /// </summary>
    public void PlayTrack()
    {
        if (_playThisSongFirst == null) // play random background track:
        {
            _randomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            _activeTrack = _randomTrack;
            _activeTrack.Play();

            StartCoroutine(PlayNextTrack());
        }
        else // play a specific track first:
        {
            _playThisSongFirst.Play();
            _activeTrack = _playThisSongFirst;

            if (!_playThisSongFirst.loop)
            {
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
            int trackID = Random.Range(0, _arrayOfHardcoreMusic.Length);
            _randomTrack = _arrayOfHardcoreMusic[trackID];
            //_randomTrack = _arrayOfHardcoreMusic[Random.Range(0, _arrayOfHardcoreMusic.Length)];

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

                    // only call this logic if the band signed the contract!
                    if(_bandSignedContract && (trackID == 0 || trackID == 1 || trackID == 2))
                    {
                        StartCoroutine(DisplaySongTitle(trackID));
                    }

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

    // Susi: Adjust the titles & Band names here as needed. Make sure the band signed the contract before placing their music in here!
    IEnumerator DisplaySongTitle(int trackID)
    {
        _songTitleUI.SetActive(true);

        switch (trackID)
        {
            case 0: _songTitleTextWindow.text = "Brainwashed (Instrumental) - Absinth";
                break;

            case 1:
                _songTitleTextWindow.text = "It's Not You (Instrumental) - Absinth";
                break;

            case 2:
                _songTitleTextWindow.text = "Shoot Em Dead (Instrumental) - Absinth";
                break;
        }
        Debug.Log("Hardcore AudioSource nr: " + trackID + " is being played"); // this will print in the console if the correct ID is being played
                                                                               // 
        yield return new WaitForSeconds(_showSongTitleThisLong);
        
        _songTitleUI.SetActive(false);
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