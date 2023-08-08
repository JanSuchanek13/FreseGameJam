using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObj : MonoBehaviour
{
    private GameObject _player;

    [Header("Breaking Objects Settings:")] // warum muss irgednwas hiervon public sein?
    //public GameObject origamiFriend;
    public bool levelEnd;
    public bool onlyCapricorn;
    public bool breakingChildren;
    public float timeTillBreak;
    public bool reset;
    public GameObject origamiFriend;

    // local variables:
    [SerializeField] bool _isBox = false;
    [SerializeField] bool _turnOffCollidersAfterBreak = false; // making this optional allows to have breaking objects which become part of the traversable environment
    [SerializeField] float _timeTillReset = 5.0f;
    [Space(10)]

    [Tooltip("This is used to tell the transcluscency-script which type of material to apply. 0 is the default for no changing materials." +
        " 1 is for breaking boxes, 2 is for breaking wood, 3+ is currently unassigned.")]
    [SerializeField] int _typeOfObject = 0;
    [Space(10)]

    [Tooltip("Add AudioSources here to be played upon collapsing the structure. Do not put anything in otherwise!")]
    [SerializeField] AudioSource[] _arrayOfCollapsingSounds;

    Vector3 _resetPos;
    Vector3 _resetRot;

    List<Vector3> _resetPosListOfChildren = new List<Vector3>();
    List<Vector3> _resetRotListOfChildren = new List<Vector3>();

    private void Start()
    {
        if (reset)
        {
            // prevent resettable objects from being destructed when falling through ground, water, lava etc:
            transform.gameObject.tag = "Indestructable";

            if (breakingChildren)
            {
                foreach (Transform child in transform)
                {
                    _resetPosListOfChildren.Add(child.gameObject.GetComponent<Transform>().position);
                    _resetRotListOfChildren.Add(child.gameObject.GetComponent<Transform>().rotation.eulerAngles);
                }
            }else
            {
                _resetPos = transform.position;
                _resetRot = transform.rotation.eulerAngles;
            }
        }
    }

    bool _calledBreak = false;

    void OnTriggerEnter(Collider collision) // was on triggerstay // not sure why we switched away from this?!
    {
        if (collision.gameObject.tag == "Player")
        {
            _player = collision.gameObject;
            StateController stateController = _player.GetComponent<StateController>();
            if (onlyCapricorn)
            {

                if (_player.GetComponent<StateController>().capricorn && _player.GetComponent<ThirdPersonMovement>().breakableDash)
                {
                    if (!_calledBreak)
                    {
                        Invoke("Break", timeTillBreak);
                        _calledBreak = true;
                    }
                }
            }
            else
            {
                if (!_calledBreak)
                {
                    Invoke("Break", timeTillBreak);
                    _calledBreak = true;
                }
                //Invoke("Break", timeTillBreak);

                if (levelEnd)
                {
                    Invoke("SetForcedFalling", 0.5f);
                    //_player.GetComponent<InputHandler>().enabled = false; if this is not active, the player can walk till the end
                    stateController.enabled = false;
                    if (!stateController.human)
                    {
                        StartCoroutine(stateController.changeModell(1));
                        stateController.ball = false;
                        stateController.human = true;
                        stateController.frog = false;
                        stateController.crane = false;
                        stateController.capricorn = false;
                        stateController.lama = false;
                    }
                    //_player.GetComponent<Animator>().SetBool("Falling", true);
                    origamiFriend.GetComponent<Rigidbody>().isKinematic = false;

                    // stopping the clock when end is reached, not need to fall to the trigger at the bottom!
                    FindObjectOfType<LevelScript>().StopTheClock();
                }
            }
        }
    }

    /*void OnTriggerStay(Collider collision) // was on triggerenter
    {
        if (collision.gameObject.tag == "Player")
        {
            _player = collision.gameObject;
            StateController stateController = _player.GetComponent<StateController>();
            if (onlyCapricorn)
            {
                
                if (_player.GetComponent<StateController>().capricorn && _player.GetComponent<ThirdPersonMovement>().breakableDash)
                {
                    if (!_calledBreak)
                    {
                        Invoke("Break", timeTillBreak);
                        _calledBreak = true;
                    }
                }
            }else
            {
                if (!_calledBreak)
                {
                    Invoke("Break", timeTillBreak);
                    _calledBreak = true;
                }
                //Invoke("Break", timeTillBreak);

                if (levelEnd)
                {
                    Invoke("SetForcedFalling", 0.5f);
                    //_player.GetComponent<InputHandler>().enabled = false; if this is not active, the player can walk till the end
                    stateController.enabled = false;
                    if (!stateController.human)
                    {
                        StartCoroutine(stateController.changeModell(1));
                        stateController.ball = false;
                        stateController.human = true;
                        stateController.frog = false;
                        stateController.crane = false;
                        stateController.capricorn = false;
                        stateController.lama = false;
                    }
                    //_player.GetComponent<Animator>().SetBool("Falling", true);
                    origamiFriend.GetComponent<Rigidbody>().isKinematic = false;

                    // stopping the clock when end is reached, not need to fall to the trigger at the bottom!
                    FindObjectOfType<LevelScript>().StopTheClock();
                }
            }
        }
    }*/

    void SetForcedFalling()
    {
        Debug.Log("forcedFalling");
        _player.GetComponent<ThirdPersonMovement>().forcedFalling = true;
    }

    void Break()
    {
        if (_isBox)
        {
            // change the layer of destroyed boxes, so the camera no longer tries to snap in front of these empty husks:
            gameObject.layer = LayerMask.NameToLayer("NonInteractiveSurfaceThatCanTurnTransluscent");
            //Debug.Log("this was called");
        }

        if (_arrayOfCollapsingSounds.Length != 0)
        {
            AudioSource _randomCollapsingSound = _arrayOfCollapsingSounds[Random.Range(0, _arrayOfCollapsingSounds.Length)];
            float _randomPitch = Random.Range(0.9f, 1.1f);
            _randomCollapsingSound.pitch = _randomPitch;

            _randomCollapsingSound.Play();
        }

        if (breakingChildren)
        {
            foreach(Transform child in transform)
            {
                if (child.GetComponent<Rigidbody>()) // this allows to search through children who may NOT have a Rigidbody component
                {
                    child.GetComponent<Rigidbody>().isKinematic = false;

                    if (_turnOffCollidersAfterBreak)
                    {
                        child.gameObject.AddComponent<MakeTraversable>(); // this will take care of making fragments traversable
                        child.gameObject.GetComponent<MakeTraversable>().typeOfObject = _typeOfObject; // this is used to allovate the correct transluscent material
                    }
                }

                // this will also loop through all children of children: Needed to break walls of new composite-breakable walls
                foreach(Transform _grandChild in child)
                {
                    if (_grandChild.GetComponent<Rigidbody>())
                    {
                        _grandChild.GetComponent<Rigidbody>().isKinematic = false;
                        
                        if (_turnOffCollidersAfterBreak)
                        {
                            _grandChild.gameObject.AddComponent<MakeTraversable>(); // this will take care of making fragments traversable
                            _grandChild.gameObject.GetComponent<MakeTraversable>().typeOfObject = _typeOfObject; // this is used to allovate the correct transluscent material
                        }
                    }
                }
            }
        }else if(GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }

        if (reset)
        {
            Invoke("Reset", _timeTillReset);
        }else
        {
            GetComponent<Collider>().enabled = false; // turn off trigger so it no longer triggers sound or particles
        }
    }

    private void Reset()
    {
        if (breakingChildren)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Rigidbody>()) // this allows to search through children who may NOT have a Rigidbody component
                {
                    child.GetComponent<Rigidbody>().isKinematic = true;
                    int i = 0;
                    child.transform.position = _resetPosListOfChildren[i];
                    child.transform.rotation = Quaternion.Euler(_resetPosListOfChildren[i]);
                    i++;

                    _calledBreak = false;
                }
            }
        }else 
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            transform.position = _resetPos;
            transform.rotation = Quaternion.Euler(_resetRot);

            _calledBreak = false;
        }
    }
}
