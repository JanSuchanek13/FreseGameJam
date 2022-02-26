using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public bool availableFrog;
    public bool availableCrane;

    public bool ball;
    public bool human = true;
    public bool frog;
    public bool crane;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };


    private IEnumerator coroutine;
    public GameObject ballVisuell;
    [SerializeField] GameObject humanVisuell;
    [SerializeField] GameObject frogVisuell;
    [SerializeField] GameObject craneVisuell;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.gameObject.CompareTag("Crane"))
        {
            Debug.Log("hit2");
            availableCrane = true;
        }
    }

    private void Start()
    {
        human = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Debug.Log(i * 3);

                switch (i)//change Movement state
                {
                    case 1:
                        ball = false;
                        human = true;
                        frog = false;
                        crane = false;
                        StartCoroutine(changeModell(i));
                        break;

                    case 2:
                        if (availableFrog)
                        {
                            ball = false;
                            human = false;
                            frog = true;
                            crane = false;
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    case 3:
                        if (availableCrane)
                        {
                            ball = false;
                            human = false;
                            frog = false;
                            crane = true;
                            StartCoroutine(changeModell(i));
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private IEnumerator changeModell(int state)
    {
        ballVisuell.SetActive(true);
        humanVisuell.SetActive(false);
        frogVisuell.SetActive(false);
        craneVisuell.SetActive(false);

        yield return new WaitForSeconds(.5f);
        switch (state)
        {
            case 1:
                humanVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 2:
                frogVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            case 3:
                craneVisuell.SetActive(true);
                ballVisuell.SetActive(false);
                break;

            default:
                break;
        }
    }
}
