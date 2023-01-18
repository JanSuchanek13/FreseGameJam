using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControlsSign : MonoBehaviour
{
    [Header("REFERENCE")]
    public GameObject keyboardControls;
    public GameObject ps4Controls;

    [Header("REFERENCE")]
    [Tooltip("0=Keyboard, 1=Ps4")]
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

            default:
                break;
        }
    }

}
