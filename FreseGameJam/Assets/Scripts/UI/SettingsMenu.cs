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
    public Slider MusicSlider;
    public TMPro.TMP_Dropdown controlsDropdown;
    public Toggle glidingToggle;
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("mouseSensitivitySettings"))
        {
            
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivitySettings");
            //Debug.Log("Loaded a Sensitivity of:" + mouseSensitivitySlider.value);

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
            //Debug.Log("Loaded a Sound Value of:" + SoundSlider.value);
            audioMixer.SetFloat("volume", SoundSlider.value);
        }
        if (PlayerPrefs.HasKey("musicSettings"))
        {
            MusicSlider.value = PlayerPrefs.GetFloat("musicSettings");
            //Debug.Log("Loaded a Sound Value of:" + SoundSlider.value);
            musicMixer.SetFloat("volume", MusicSlider.value);
        }
        if (PlayerPrefs.HasKey("controlsSettings") && controlsDropdown!=null)
        {
            controlsDropdown.value = PlayerPrefs.GetInt("controlsSettings");
        }
        if (PlayerPrefs.HasKey("glidingSettings") && glidingToggle!=null)
        {
            if (PlayerPrefs.GetInt("glidingSettings") > 0)
            {
                glidingToggle.isOn = true;
            }
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

    public void SetMusic(float volume)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("musicSettings", volume);
        Debug.Log("Set music to:" + volume);


        musicMixer.SetFloat("volume", volume);
    }

    public void SetControls (Int32 controler)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        Debug.Log(controler);
        int controlerInt = (int)controler;
        PlayerPrefs.SetInt("controlsSettings", controlerInt);
        
    }

    public void SetGliding (bool holdGliding)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        int holdGlidingInt = holdGliding ? 1 : 0;
        PlayerPrefs.SetInt("glidingSettings", holdGlidingInt);
    }
}
