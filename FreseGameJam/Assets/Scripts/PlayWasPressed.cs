using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayWasPressed : MonoBehaviour
{
    [SerializeField] GameObject friendInBoat;
    [SerializeField] GameObject firesInBoat_1;
    [SerializeField] GameObject firesInBoat_2;
    [SerializeField] GameObject leftBoat;
    [SerializeField] GameObject rightBoat;
    [SerializeField] GameObject fadeToBlackBlende;
    private Image fadeToBlackBlende_Image;
    [SerializeField] AudioSource panickedScream;
    [SerializeField] AudioSource helpScream;
    [SerializeField] float delayBeforeStarting = 5;
    private bool _moveTowardIsland = false;
    private float _shuffleIncreaseIncrement;
    private float _speedIncreaseIncrement;
    private bool _startFading;
    private float _increaceColorAlphaIncrement;
    private float _color_R_component;
    private float _color_G_component;
    private float _color_B_component;
    private float _color_A_component;

    void Start()
    {
        //Debug.Log("start was called");
        _shuffleIncreaseIncrement = 5f / ((delayBeforeStarting+1) * 50f);
        //Debug.Log(_shuffleIncreaseIncrement); // TST:
        _speedIncreaseIncrement = 2.5f / ((delayBeforeStarting+1) * 50f);
        //Debug.Log(_speedIncreaseIncrement); // TST:
        _increaceColorAlphaIncrement = 255 / ((delayBeforeStarting * .6f) * 50f);
        //Debug.Log(_increaceColorAlphaIncrement); // TST:

        fadeToBlackBlende_Image = fadeToBlackBlende.GetComponent<Image>();
        _color_R_component = fadeToBlackBlende_Image.color.r;
        _color_G_component = fadeToBlackBlende_Image.color.g;
        _color_B_component = fadeToBlackBlende_Image.color.b;
        _color_A_component = fadeToBlackBlende_Image.color.a;
        //image = GetComponent<Image>();


        //colorOfMixture = new Color32((byte)color_R_component, (byte)color_G_component, (byte)color_B_component, (byte)color_A_component);
    }
    void FixedUpdate()
    {
        if (_moveTowardIsland)
        {
            if (leftBoat.GetComponent<Shuffle>().shuffleDistance < 10f)
            {
                leftBoat.GetComponent<Shuffle>().shuffleDistance += _shuffleIncreaseIncrement;
                leftBoat.GetComponent<Shuffle>().speed += _speedIncreaseIncrement;
                rightBoat.GetComponent<Shuffle>().shuffleDistance += _shuffleIncreaseIncrement;
                rightBoat.GetComponent<Shuffle>().speed += _speedIncreaseIncrement;
            }
        }
        if (_startFading)
        {
            if (_color_A_component != 255) // change color of splash to fully opaque
            {
                //opaqueSpill = false;
                //color_A_component = 255;
                _color_A_component += _increaceColorAlphaIncrement;
                var _increasedOpaqueness = new Color32((byte)_color_R_component, (byte)_color_G_component, (byte)_color_B_component, (byte)_color_A_component);
                fadeToBlackBlende_Image.color = _increasedOpaqueness;
            }

            //var tempColor = fadeToBlackBlende_Image.color;
           // tempColor.a += _increaceColorAlphaIncrement;
           // fadeToBlackBlende_Image.color = tempColor;
            //fadeToBlackBlende.GetComponent<Image>().GetComponent<Color32>().a += _increaceColorAlphaIncrement;
        }
    }
    public void ImmersePlayer(int ContinueOrNewRound)
    {
        // "0" means new round
        // "1" means continue
        if (ContinueOrNewRound == 1)
        {
            Invoke("ContinueOldRound", delayBeforeStarting);
        }else
        {
            Invoke("StartNewRound", delayBeforeStarting);
        }

        firesInBoat_1.SetActive(true);
        friendInBoat.GetComponent<LookAtTarget>().enabled = false; // stop looking at friend.
        friendInBoat.transform.LookAt(firesInBoat_1.transform);
        float _randomPitch = Random.Range(1f, 3f);
        panickedScream.pitch = _randomPitch;
        panickedScream.Play();
        helpScream.PlayDelayed(delayBeforeStarting/2f);

        //_moveTowardIsland = true; // testing new "MoveTowardPoint" script:
        leftBoat.GetComponent<Shuffle>().enabled = false;
        rightBoat.GetComponent<Shuffle>().enabled = false;
        leftBoat.GetComponent<MoveTowardPoint>().MoveIt();
        rightBoat.GetComponent<MoveTowardPoint>().MoveIt();

        Invoke("StartDelayedFade", delayBeforeStarting * .4f);
    }
    void StartNewRound()
    {
        GetComponent<Level_Manager>().LoadLevel(1);
    }
    void ContinueOldRound()
    {
        GetComponent<Level_Manager>().ContinueLevel();
    }

    void StartDelayedFade()
    {
        _startFading = true;
        firesInBoat_2.SetActive(true);
    }
}
