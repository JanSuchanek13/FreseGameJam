using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using Cinemachine;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    public bool initialized = false;
    public CinemachineFreeLook cinemachineFreeLook;
    public CinemachineFreeLook CloseUpCam;
    public CinemachineFreeLook CraneCam;
    public Slider mouseSensitivitySlider;
    public Slider SoundSlider;
    public Slider MusicSlider;
    public TMPro.TMP_Dropdown controlsDropdown;
    public Toggle glidingToggle;
    public Toggle vibrationToggle;
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;
    public Toggle lightningToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("mouseSensitivitySettings"))
        {
            
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivitySettings");
            //Debug.Log("Loaded a Sensitivity of:" + mouseSensitivitySlider.value);

            if(cinemachineFreeLook != null)
            {
                cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = mouseSensitivitySlider.value / 100;      //for Normal Cam
                cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = mouseSensitivitySlider.value / 10000;
                CloseUpCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = mouseSensitivitySlider.value / 100;               //for closeUp Cam
                CloseUpCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = mouseSensitivitySlider.value / 10000;
                CraneCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = mouseSensitivitySlider.value / 100;               //for Crane Cam
                CraneCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = mouseSensitivitySlider.value / 10000;
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
            glidingToggle.isOn = PlayerPrefs.GetInt("glidingSettings") == 1 ? true : false;
        }
        if (PlayerPrefs.HasKey("vibrationSettings") && vibrationToggle != null)
        {
            vibrationToggle.isOn = PlayerPrefs.GetInt("vibrationSettings") == 1 ? true : false;
        }

        initialized = true;
    }


    public void SetToDefault()
    {
        mouseSensitivitySlider.value = mouseSensitivitySlider.maxValue / 2;     //Sensitivity half of Max
        SoundSlider.value = SoundSlider.minValue / 2;                           //Sound half of Min --> negative Values
        MusicSlider.value = MusicSlider.minValue / 2;                           //Music half of Min
        controlsDropdown.value = 0;                                             //Controls to Keyboard
        glidingToggle.isOn = true;                                              //Hold for gliding
        vibrationToggle.isOn = true;                                            //Use Vibration
        lightningToggle.isOn = true;                                            //Enable Lightning Flash
    }
    

    public void SetMouseSensitivity(float val)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("mouseSensitivitySettings", val);
        Debug.Log("Set Sensitivity to:" + val);

        if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = val / 100;
            cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = val / 10000;
            CloseUpCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = val / 100;
            CloseUpCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = val / 10000;
            CraneCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = val / 100;
            CraneCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = val / 10000;
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
        
        if (controlerInt > 0)
        {
            EventSystem.current.SetSelectedGameObject(controlsDropdown.gameObject);
        }
    }

    public void SetGliding (bool holdGliding)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;
        
        int holdGlidingInt = holdGliding ? 1 : 0;
        PlayerPrefs.SetInt("glidingSettings", holdGlidingInt);
    }

    public void SetVibration(bool useVibration)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;
        
        int useVibrationInt = useVibration ? 1 : 0;
        PlayerPrefs.SetInt("vibrationSettings", useVibrationInt);
    }

    public void SetLightning(bool enableLightning)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        int enableLightningInt = enableLightning ? 1 : 0;
        PlayerPrefs.SetInt("lightningSettings", enableLightningInt);
    }
}
