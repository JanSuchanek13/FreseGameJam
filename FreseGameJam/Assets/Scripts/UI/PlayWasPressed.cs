using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayWasPressed : MonoBehaviour
{
    [Header("Juice Elements when hitting Play:")]
    [SerializeField] GameObject friendInBoat;
    [SerializeField] GameObject firesInBoat_1;
    [SerializeField] GameObject firesInBoat_2;
    [SerializeField] GameObject leftBoat;
    [Space(10)]

    [SerializeField] GameObject rightBoat;
    [Space(10)]

    [SerializeField] GameObject fadeToBlackBlende;
    [Space(10)]

    [SerializeField] AudioSource panickedScream;
    [SerializeField] AudioSource helpScream;
    [SerializeField] float delayBeforeStarting = 5;

    [Header("Lightning Strike:")]
    [SerializeField] GameObject _lightningGO;
    [SerializeField] GameObject _lightningLight;
    [Space(5)]
    [SerializeField] float _lightFlashDuration = 0.1f;
    [Space(5)]
    [SerializeField] float _timeWhenLowestPointReached = 0.5f;
    [Space(5)]
    [SerializeField] AudioSource _lightningSound;
    [Space(5)]
    [SerializeField] ParticleSystem _lightningParticles;


    // private variables:
    private Image _fadeToBlackBlende_Image;
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
        //_increaceColorAlphaIncrement = 255 / (((delayBeforeStarting - 1.0f) * .6f) * 50f); // -1 should help completly blacken the screen a second before delay runs up
        //Debug.Log(_increaceColorAlphaIncrement); // TST:

        _fadeToBlackBlende_Image = fadeToBlackBlende.GetComponent<Image>();
        _color_R_component = _fadeToBlackBlende_Image.color.r;
        _color_G_component = _fadeToBlackBlende_Image.color.g;
        _color_B_component = _fadeToBlackBlende_Image.color.b;
        _color_A_component = _fadeToBlackBlende_Image.color.a;
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
            if (_color_A_component < 250) // change color of overlay to fully opaque
            {
                _color_A_component += _increaceColorAlphaIncrement;
                var _increasedOpaqueness = new Color32((byte)_color_R_component, (byte)_color_G_component, (byte)_color_B_component, (byte)_color_A_component);
                _fadeToBlackBlende_Image.color = _increasedOpaqueness;
            }else
            {
                _color_A_component = 255; // set to max
                var _increasedOpaqueness = new Color32((byte)_color_R_component, (byte)_color_G_component, (byte)_color_B_component, (byte)_color_A_component);
                _fadeToBlackBlende_Image.color = _increasedOpaqueness;
                
                // done with fading:
                _startFading = false;
            }
        }
    }
    public void ImmersePlayer(int ContinueOrNewRound)
    {
        // "0" means new round
        // "1" means continue
        // "2" means hardcore round
        if (ContinueOrNewRound == 1)
        {
            Invoke("ContinueOldRound", delayBeforeStarting);
        }else if(ContinueOrNewRound == 0)
        {
            Invoke("StartNewRound", delayBeforeStarting);
        }else if(ContinueOrNewRound == 2)
        {
            Invoke("StartHardcoreRound", delayBeforeStarting);
        }

        // call lightning:
        StartCoroutine(LightningStrike());

        // make boats move towards island:
        leftBoat.GetComponent<Shuffle>().enabled = false;
        rightBoat.GetComponent<Shuffle>().enabled = false;
        leftBoat.GetComponent<MoveTowardPoint>().MoveIt();
        rightBoat.GetComponent<MoveTowardPoint>().MoveIt();

        StartDelayedFade();
    }
    void StartNewRound()
    {
        PlayerPrefs.SetInt("HardcoreMode", 0);
        GetComponent<Level_Manager>().LoadLevel(1);
    }
    void ContinueOldRound()
    {
        PlayerPrefs.SetInt("HardcoreMode", 0);
        GetComponent<Level_Manager>().ContinueLevel();
    }

    /// <summary>
    /// This is public as this function is used in the HardcoreFastRestart-script to restart hardcore-mode from ingame (HardcoreMode).
    /// </summary>
    public void StartHardcoreRound()
    {
        PlayerPrefs.SetInt("HardcoreMode", 1);
        GetComponent<Level_Manager>().LoadLevel(1);
    }
    void StartDelayedFade()
    {
        _startFading = true;
    }

    IEnumerator LightningStrike()
    {
        // little bit of a juicy delay before the lighting
        yield return new WaitForSeconds(2.0f); // Adjust the duration to your liking

        float _animationSpeedAdjustment = 0.5f; // tweak the speed here!

        // lightning will animate automatically once awake:
        _lightningGO.SetActive(true);

        Animation _anim = _lightningGO.GetComponent<Animation>();
        _anim["LightningStrike"].speed = _animationSpeedAdjustment;

        // we need duration of the lightning animation to know when to turn it off:
        float _durationOfLightningStrike = _lightningGO.GetComponent<Animation>().clip.length / _animationSpeedAdjustment; // pretend the animation is longer if its played slower!
        _lightningSound.Play();

        // turn on the light for a short period of time to create a lightning-flash-effect:
        _lightningLight.SetActive(true);
        yield return new WaitForSeconds(_lightFlashDuration); // Adjust the duration to your liking
        _lightningLight.SetActive(false);

        // wait until the lowest point of the animation is reached (impact achieved):
        yield return new WaitForSeconds((_timeWhenLowestPointReached / _animationSpeedAdjustment) - _lightFlashDuration); 

        _lightningParticles.Play();

        // get remainder of animation time and speed up the rest of animation:
        _durationOfLightningStrike -= Time.deltaTime;
        _animationSpeedAdjustment = 2.0f;
        _anim["LightningStrike"].speed = _animationSpeedAdjustment;
        _durationOfLightningStrike /= _animationSpeedAdjustment;

        // Wait for lightning animation to finish:
        yield return new WaitForSeconds(_durationOfLightningStrike - 0.4f); // there is an odd delay where the GO just hangs there but is still in animation

        // Disable the lightning effect
        _lightningGO.SetActive(false);
        firesInBoat_1.SetActive(true);
        
        // play relevant sounds of people in boats:
        float _randomPitch = Random.Range(1.2f, 1.5f);
        // currently not used:
        //panickedScream.pitch = _randomPitch;
        //panickedScream.Play();

        helpScream.pitch = _randomPitch;
        helpScream.Play();

        yield return new WaitForSeconds(1.5f);
        firesInBoat_2.SetActive(true);
    }
}
