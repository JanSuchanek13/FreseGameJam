using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControlsSign : MonoBehaviour
{
    [Header("REFERENCE")]
    public GameObject keyboardControls;
    public GameObject ps4Controls;
    public GameObject xboxControls;

    [Header("REFERENCE")]
    [Tooltip("0=Keyboard, 1=Ps4, 2=XBox")]
    public int controlSettings = 0;
    void Awake()
    {
        controlSettings = PlayerPrefs.GetInt("controlsSettings");

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
