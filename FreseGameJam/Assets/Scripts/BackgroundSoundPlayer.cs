using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundPlayer : MonoBehaviour
{
    #region variables
    [Header("Insert all Background-Music AudioSources here:")]
    [SerializeField] AudioSource[] arrayOfBackgroundMusic; // put all backgroundtracks in here.
    [Header("Optionall Settings(!):")]
    [SerializeField] AudioSource playThisSongFirst; // if a song is put in here, that will be the first song played on load
    [SerializeField] bool playOnlyTheSameSongTheWholeTime = false; // kind of redundant, why would we want/need this?

    private AudioSource _randomTrack;
    private AudioSource _nextRandomTrack;
    private AudioSource _activeTracK; // this is for "FocusPlayerViewOnObject"-Script to pause the background music on demand;
#endregion

    private void Start()
    {
        Invoke("PlayTrack", .1f); // for some reason, if the "PlayTrack()" Logic was here, all would be called, except the .Play(); part = no sound played!
    }
    void PlayTrack()
    {
        if (playThisSongFirst == null) // play random background track:
        {
            _randomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
            float _lengthOfTrack = _randomTrack.clip.length;
            //Debug.Log("this song is " + _lengthOfTrack + " seconds long!");
            Invoke("PlayNextTrack", _lengthOfTrack);
            //Debug.Log("this songs name is " + _randomTrack.clip.name);
            _randomTrack.Play();
            _activeTracK = _randomTrack;
        }else // play a specific track first:
        {
            //Debug.Log("this track: \"" + playThisSongFirst + "\" was explicetly chosen to be the first track");
            playThisSongFirst.Play();
            _activeTracK = playThisSongFirst;
            if (playOnlyTheSameSongTheWholeTime)
            {
                //Debug.Log("this track: " + playThisSongFirst + " is on going to loop the whole time");
                Invoke("PlayTrack", playThisSongFirst.clip.length); // loop this song indefinitely:
            }else
            {
                Invoke("PlayNextTrack", playThisSongFirst.clip.length); // play next, random song after this:

            }
        }
    }
    public void PlayNextTrack()
    {
        Debug.Log("playing next background track");
        _nextRandomTrack = arrayOfBackgroundMusic[Random.Range(0, arrayOfBackgroundMusic.Length)];
        if (_nextRandomTrack == _randomTrack)
        {
            // this gets executed if the new track is the same as the last one:
            PlayNextTrack();
            Debug.Log("The next song would be the same as the one that just played - searching new song now!");
            return;
        }else
        {
            // if new track was found, replace the old one with the new one:
            _randomTrack = _nextRandomTrack;
            Debug.Log("this songs name is \"" + _randomTrack.clip.name + "\"");
        }

        _randomTrack.Play();
        _activeTracK = _randomTrack;
        float _lengthOfTrack = _randomTrack.clip.length;
        Debug.Log("this song is " + _lengthOfTrack + " seconds long!");
        Invoke("PlayNextTrack", _lengthOfTrack);
    }
    public void PauseMusic()
    {
        _activeTracK.Pause();
    }
    public void UnpauseMusic()
    {
        _activeTracK.UnPause();
    }
}