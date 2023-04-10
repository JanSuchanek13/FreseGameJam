using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region variables:
    [Header("General Settings:")]
    // to follow

    [Header("Audio Settings:")]
    public float defaultVolumeSettings = 0.0f;

    [Header("Mouse Sensitivity Settings:")]
    public float defaultMouseSensitivitySettings = 500;

    [Header("Checkpoint Settings:")]
    public AudioSource choireHymnSound;
    public AudioSource fireSwooshSound;
    #endregion

    void Start()
    {
        //Debug.Log("GameManger Temp Msg was called"); // just until we have a real function
    }
}
