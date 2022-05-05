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
    public Slider mouseSensitivitySlider;
    public Slider SoundSlider;
    public AudioMixer audioMixer;

    int pingPongAlpha =1;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
            Debug.Log("Loaded a Sensitivity of:" + mouseSensitivitySlider.value);

            cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed * mouseSensitivitySlider.value;
            cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed * mouseSensitivitySlider.value;
        }
        if (PlayerPrefs.HasKey("Sound"))
        {
            SoundSlider.value = PlayerPrefs.GetFloat("Sound");
            Debug.Log("Loaded a Sensitivity of:" + SoundSlider.value);
            audioMixer.SetFloat("volume", SoundSlider.value);
        }
        initialized = true;
    }

    private void Update()
    {
        
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("pressed ESC");
            GetComponent<CanvasGroup>().alpha = pingPongAlpha;
            pingPongAlpha = pingPongAlpha*-1;
        }
    }

    public void SetMouseSensitivity(float val)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("Sensitivity", val);
        Debug.Log("Set Sensitivity to:" + val);

        cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed * val;
        cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = cinemachineFreeLook.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed * val;
    }

    public void SetVolume (float volume)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("Sound", volume);
        Debug.Log("Set Sensitivity to:" + volume);


        audioMixer.SetFloat("volume", volume);
    }
}
