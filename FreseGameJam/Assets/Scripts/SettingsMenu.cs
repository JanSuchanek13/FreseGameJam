using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using Cinemachine;
using UnityEngine.Serialization;

public class SettingsMenu : MonoBehaviour
{
    public bool initialized = false;
    public CinemachineFreeLook cinemachineFreeLook;
    public CinemachineFreeLook CloseUpCam;
    public Slider mouseSensitivitySlider;
    public Slider SoundSlider;
    public AudioMixer audioMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("mouseSensitivitySettings"))
        {
            
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivitySettings");
            Debug.Log("Loaded a Sensitivity of:" + mouseSensitivitySlider.value);

            if(cinemachineFreeLook != null)
            {
                cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = mouseSensitivitySlider.value;      //for Normal Cam
                cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = mouseSensitivitySlider.value / 100;
                CloseUpCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = mouseSensitivitySlider.value;               //for closeUp Cam
                CloseUpCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = mouseSensitivitySlider.value / 100;
            }
            
        }
        if (PlayerPrefs.HasKey("volumeSettings"))
        {
            SoundSlider.value = PlayerPrefs.GetFloat("volumeSettings");
            Debug.Log("Loaded a Sound Value of:" + SoundSlider.value);
            audioMixer.SetFloat("volume", SoundSlider.value);
        }
        initialized = true;
    }

    

    public void SetMouseSensitivity(float val)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("mouseSensitivitySettings", val);
        Debug.Log("Set Sensitivity to:" + val);

        if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = val;
            cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = val / 100;
            CloseUpCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = val;
            CloseUpCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = val / 100;
        }
        
    }

    public void SetVolume (float volume)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("volumeSettings", volume);
        Debug.Log("Set volume to:" + volume);


        audioMixer.SetFloat("volume", volume);
    }
}
