using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControlsSign : MonoBehaviour
{
    [Header("REFERENCE")]
    public GameObject keyboardControls;
    public GameObject ps4Controls;
    public GameObject xboxControls;

    [Header("HARDCOR REFERENCE")]
    public GameObject keyboardHCControls;
    public GameObject ps4HCControls;
    public GameObject xboxHCControls;

    [Header("REFERENCE")]
    [Tooltip("0=Keyboard, 1=Ps4, 2=XBox")]
    public int controlSettings = 0;

    void Awake()
    {
        ChangeSign();
    }

    void Update()
    {
        if (controlSettings != PlayerPrefs.GetInt("controlsSettings"))
        {
            ChangeSign();
        }
    }


    private void ChangeSign()
    {
        keyboardControls.SetActive(false);
        ps4Controls.SetActive(false);
        xboxControls.SetActive(false);
        controlSettings = PlayerPrefs.GetInt("controlsSettings");

        if (PlayerPrefs.GetInt("HardcoreMode") != 0)
        {
            //Debug.Log("test");
            switch (controlSettings)
            {
                case 0:
                    keyboardHCControls.SetActive(true);
                    break;

                case 1:
                    ps4HCControls.SetActive(true);
                    break;

                case 2:
                    xboxHCControls.SetActive(true);
                    break;

                default:

                    Debug.Log(controlSettings);
                    break;
            }
        }
        else
        {
            switch (controlSettings)
            {
                case 0:
                    keyboardControls.SetActive(true);
                    break;

                case 1:
                    ps4Controls.SetActive(true);
                    break;

                case 2:
                    xboxControls.SetActive(true);
                    break;

                default:
                    break;
            }
        }
        
    }
}
