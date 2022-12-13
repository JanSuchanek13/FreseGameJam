using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaShuffleTile : MonoBehaviour
{
    [SerializeField] GameObject[] firesOfPlattform;
    [SerializeField] float timeBeforeSubmerge = 1f;
    [SerializeField] float timeBeforeResurface = 1f;
    [SerializeField] Shuffle _thisTilesShuffleScript;
    bool isStanding = false;
    bool inCoroutine = false;
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
    /*private void OnTriggerEnter(Collider other)
    {
        if (!inCoroutine && other.tag == "Player")
        {
            inCoroutine = true;

                _player = other.gameObject;
                _player.transform.parent = transform;
                _fire1.SetActive(true);
                isStanding = true;
                StartCoroutine(CountDownToSubmerge());
                inCoroutine = true;
        }
    }
        //Debug.Log("player jumped on me!");
        
        if (other.tag == "Player")
        {
            _player = other.gameObject;
            Debug.Log("player jumped on me!");
            _fire1.SetActive(true);
            isStanding = true;
            StartCoroutine(CountDownToSubmerge());
            Debug.Log("1");
            //test
            _player.transform.parent = transform;
            Debug.Log("player should be part of magmatile!");
        }
    }*/
    private void OnTriggerStay(Collider other) //allows player to stand on moving platform
    {
        if (other.tag == "Player")
        {
            Debug.Log("platofrm was entered");
            _player = other.gameObject;
            _player.transform.parent = transform;
            StartCoroutine(ContactChecker());
            if (!inCoroutine)
            {
                //_player = other.gameObject;
                //_player.transform.parent = transform;
                _fire1.SetActive(true);
                isStanding = true;
                //if (!inCoroutine)
                //{
                StartCoroutine(CountDownToSubmerge());
                //}
                inCoroutine = true;
            }
        }/*else
        {
            Debug.Log("platofrm was left");
            _player.transform.parent = null;
            _player = null;
        }*/
    }
    private IEnumerator ContactChecker()
    {
        if (_player.transform.parent != null)
        {
            yield return new WaitForSeconds(.01f);
            _player.transform.parent = null;
        }
    }
    /*private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _player.transform.parent = null;
        }
    }*/
    /*private void OnTriggerExit(Collider other)
    {
        Debug.Log("player jumped OFF plattform!");
        isStanding = false;
        if (other.tag == "Player")
        {
            _player.transform.parent = null;
            //other.transform.parent = null;
            foreach (GameObject i in firesOfPlattform)
            {
                i.SetActive(false);
            }
        }
    }*/
    private IEnumerator CountDownToSubmerge()
    {
        Debug.Log("coroutine started!");
        if (isStanding)
        {
            yield return new WaitForSeconds(timeBeforeSubmerge / 4);
            _fire2.SetActive(true);
            Debug.Log("fire 2");

            yield return new WaitForSeconds(timeBeforeSubmerge / 4);
            _fire3.SetActive(true);
            Debug.Log("fire 3");

            yield return new WaitForSeconds(timeBeforeSubmerge / 4);
            _fire4.SetActive(true);
            Debug.Log("fire 4");

            yield return new WaitForSeconds(timeBeforeSubmerge / 4); // now it should sink:
            Debug.Log("sink");

            _player.transform.parent = null; // unlock player
            _thisTilesShuffleScript.enabled = false; // stop tile movement
            transform.position += new Vector3(0, -1f, 0); // submerge tile
            foreach (GameObject i in firesOfPlattform) // turn off all fires
            {
                i.SetActive(false);
            }
            yield return new WaitForSeconds(timeBeforeResurface);
            transform.position += new Vector3(0, +1f, 0);
            _thisTilesShuffleScript.enabled = true;
            inCoroutine = false;
            yield break;
        }else
        {
            foreach (GameObject i in firesOfPlattform)
            {
                i.SetActive(false);
            }
            inCoroutine = false;
            yield break;
        }
    }
}
