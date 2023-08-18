using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaShuffleTile : MonoBehaviour
{
    [SerializeField] GameObject[] firesOfPlattform;
    [SerializeField] float timeBeforeSubmerge = 1f;
    [SerializeField] float timeBeforeResurface = 1f;
    [SerializeField] Shuffle _thisTilesShuffleScript;
    [Tooltip("One of these sounds will be played at random on every interval of the countdown of this tile. This will " +
        "coincide with the lighting of each flame.")]
    [SerializeField] AudioSource[] _arrayOfBurningSounds;
    [Tooltip("One of these sounds will be played at random when this platform gets submerged.")]
    [SerializeField] AudioSource[] _arrayOfSubmergeSounds;

    bool _isStanding = false;
    bool _inCoroutine = false;
    GameObject _fire1;
    GameObject _fire2;
    GameObject _fire3;
    GameObject _fire4;
    GameObject _player;


    private void Start()
    {
        if (firesOfPlattform != null) // redundant technically
        {
            _fire1 = firesOfPlattform[0];
            _fire2 = firesOfPlattform[1];
            _fire3 = firesOfPlattform[2];
            _fire4 = firesOfPlattform[3];
        }
    }

    private void OnTriggerStay(Collider other) //allows player to stand on moving platform
    {
        if (other.tag == "Player")
        {
            _player = other.gameObject;
            _player.transform.parent = transform;
            StartCoroutine(ContactChecker());

            if (!_inCoroutine)
            {
                _fire1.SetActive(true);
                PlayBurnSound();

                _isStanding = true;
                StartCoroutine(CountDownToSubmerge());
                _inCoroutine = true;
            }
        }
    }

    private IEnumerator ContactChecker()
    {
        if (_player.transform.parent != null)
        {
            yield return new WaitForSeconds(.01f);
            _player.transform.parent = null;
        }
    }

    private IEnumerator CountDownToSubmerge()
    {
        if (_isStanding)
        {
            yield return new WaitForSeconds(timeBeforeSubmerge / 4);
            _fire2.SetActive(true);
            PlayBurnSound();

            yield return new WaitForSeconds(timeBeforeSubmerge / 4);
            _fire3.SetActive(true);
            PlayBurnSound();

            yield return new WaitForSeconds(timeBeforeSubmerge / 4);
            _fire4.SetActive(true);
            PlayBurnSound();

            yield return new WaitForSeconds(timeBeforeSubmerge / 4); // now it should sink:

            // unlock player:
            _player.transform.parent = null;

            // disable tile movement:
            if (_thisTilesShuffleScript != null)
            {
                _thisTilesShuffleScript.enabled = false;
            }
            if (GetComponent<Patrol>())
            {
                GetComponent<Patrol>().patrolling = false; // stop patrolling:
                GetComponent<Rotator>().enabled = false;
            }

            // submerge tile:
            transform.position += new Vector3(0, -2f, 0);
            PlaySubmergeSound();

            // turn off all fires:
            foreach (GameObject i in firesOfPlattform)
            {
                i.SetActive(false);
            }
           
            yield return new WaitForSeconds(timeBeforeResurface);
            
            transform.position += new Vector3(0, +2f, 0);

            _inCoroutine = false;

            // enable tile movement:
            if (_thisTilesShuffleScript != null)
            {
                _thisTilesShuffleScript.enabled = true;
            }
            if (GetComponent<Patrol>())
            {
                GetComponent<Patrol>().ContinuePatrol(); // continue the patrol:

                GetComponent<Rotator>().enabled = true;
            }
            yield break;

        }else
        {
            foreach (GameObject i in firesOfPlattform)
            {
                i.SetActive(false);
            }

            _inCoroutine = false;
            yield break;
        }
    }
    void PlayBurnSound()
    {
        if (_arrayOfBurningSounds != null)
        {
            AudioSource _randomBurningSound = _arrayOfBurningSounds[Random.Range(0, _arrayOfBurningSounds.Length)];
            float _randomPitch = Random.Range(0.9f, 1.1f);
            _randomBurningSound.pitch = _randomPitch;

            _randomBurningSound.Play();
        }
    }

    void PlaySubmergeSound()
    {
        if (_arrayOfSubmergeSounds != null)
        {
            AudioSource _randomSubmergeSound = _arrayOfSubmergeSounds[Random.Range(0, _arrayOfSubmergeSounds.Length)];
            float _randomPitch = Random.Range(0.9f, 1.1f);
            _randomSubmergeSound.pitch = _randomPitch;

            _randomSubmergeSound.Play();
        }
    }
}
