using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPCamTrigger : MonoBehaviour
{
    [Header("InsideVulcano_PP")]
    private bool isInVulcano;
    private float timeInVulcano = 0;
    private float lastTimeInVulcano = 0;
    private float vulcanoTransitionSpeed = 0.3f;
    public GameObject vulcanoPP;

    [Header("InsideTunnel_PP")]
    private bool isInTunnel;
    private float timeInTunnel = 0;
    private float lastTimeInTunnel = 0;
    private float tunnelTransitionSpeed = 0.3f;
    public GameObject tunnelPP;

    [Header("InsideMines_PP")]
    private bool isInMines;
    private float timeInMines = 0;
    private float lastTimeInMines = 0;
    private float minesTransitionSpeed = 0.3f;
    public GameObject minesPP;

    [Header("LightFlash_PP")]
    public GameObject lightFlashPP;
    private float lightFlashTransitionSpeed = 0.3f;
    private bool inLightFlash;

    private bool playerJustDied;

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "InsideVulcano_PP")
        {

            timeInVulcano += Time.deltaTime;
        }
        if (other.name == "InsideTunnel_PP")
        {

            timeInTunnel += Time.deltaTime;
        }
        if (other.name == "InsideMines_PP")
        {

            timeInMines += Time.deltaTime;
        }
    }

    private void Update()
    {
        lastTimeInVulcano = timeInVulcano;
        lastTimeInTunnel = timeInTunnel;
        lastTimeInMines = timeInMines;
        //Invoke("CheckForNarrowSpace", 1f);
        StartCoroutine("UpdateVulcano", lastTimeInVulcano);
        StartCoroutine("UpdateTunnel", lastTimeInTunnel);
        StartCoroutine("UpdateMines", lastTimeInMines);
        StartCoroutine("JustDied");
    }

    IEnumerator UpdateVulcano(float _lastTimeInVulcano)
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(_lastTimeInVulcano + "    " + timeInVulcano);
        if (_lastTimeInVulcano < timeInVulcano)
        {
            if (!isInVulcano)
            {
                vulcanoPP.GetComponent<Volume>().enabled = true;
            }
            if (vulcanoPP.GetComponent<Volume>().weight <= 0.5f)
            {
                vulcanoPP.GetComponent<Volume>().weight += Time.deltaTime * vulcanoTransitionSpeed;
            }
            isInVulcano = true;
        }
        else
        {
            
            if (vulcanoPP.GetComponent<Volume>().weight >= 0f)
            {
                vulcanoPP.GetComponent<Volume>().weight -= Time.deltaTime * vulcanoTransitionSpeed;
            }
            if (isInVulcano)
            {
                /*
                if (!inLightFlash && !playerJustDied)
                {
                    Debug.Log("Light");
                    StartCoroutine("LightFlash");
                }
                */
                
                yield return new WaitForSeconds(1f);
                if (! (_lastTimeInVulcano < timeInVulcano))
                {
                    vulcanoPP.GetComponent<Volume>().enabled = false;
                }
            }
            isInVulcano = false;
        }
    }

    IEnumerator UpdateTunnel(float _lastTimeInTunnel)
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(_lastTimeInTunnel + "    " + timeInTunnel);
        if (_lastTimeInTunnel < timeInTunnel)
        {
            if (!isInTunnel)
            {
                tunnelPP.GetComponent<Volume>().enabled = true;
            }
            if (tunnelPP.GetComponent<Volume>().weight <= 0.7f)
            {
                tunnelPP.GetComponent<Volume>().weight += Time.deltaTime * tunnelTransitionSpeed;
            }
            isInTunnel = true;
        }
        else
        {

            if (tunnelPP.GetComponent<Volume>().weight >= 0f)
            {
                tunnelPP.GetComponent<Volume>().weight -= Time.deltaTime * tunnelTransitionSpeed;
            }
            if (isInTunnel)
            {
                if (!inLightFlash && !playerJustDied)
                {
                    StartCoroutine("LightFlash");
                }

                yield return new WaitForSeconds(1f);
                if (!(_lastTimeInTunnel < timeInTunnel))
                {
                    tunnelPP.GetComponent<Volume>().enabled = false;
                }
            }
            isInTunnel = false;
        }
    }

    IEnumerator UpdateMines(float _lastTimeInMines)
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(_lastTimeInMines + "    " + timeInMines);
        if (_lastTimeInMines < timeInMines)
        {
            if (!isInMines)
            {
                minesPP.GetComponent<Volume>().enabled = true;
            }
            if (minesPP.GetComponent<Volume>().weight <= 0.5f)
            {
                minesPP.GetComponent<Volume>().weight += Time.deltaTime * minesTransitionSpeed;
            }
            isInMines = true;
        }
        else
        {
            if (minesPP.GetComponent<Volume>().weight >= 0f)
            {
                minesPP.GetComponent<Volume>().weight -= Time.deltaTime * minesTransitionSpeed;
            }
            if (isInMines)
            {
                if (!inLightFlash && !playerJustDied)
                {
                    StartCoroutine("LightFlash");
                }

                yield return new WaitForSeconds(1f);
                if (!(_lastTimeInMines < timeInMines))
                {
                    minesPP.GetComponent<Volume>().enabled = false;
                }
            }
            isInMines = false;
        }
    }

    IEnumerator LightFlash()
    {
        inLightFlash = true;
        lightFlashPP.GetComponent<Volume>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        
        while (lightFlashPP.GetComponent<Volume>().weight >= 0f)
        {
            yield return new WaitForSeconds(0.01f);
            lightFlashPP.GetComponent<Volume>().weight -= Time.deltaTime * lightFlashTransitionSpeed;
        }
        lightFlashPP.GetComponent<Volume>().enabled = false;
        lightFlashPP.GetComponent<Volume>().weight = 0.7f;
        inLightFlash = false;
    }

    IEnumerator JustDied()
    {
        if (GetComponent<HealthSystem>().inCoroutine)
        {
            playerJustDied = true;
            yield return new WaitForSeconds(3f);
            playerJustDied = false;
        }
    }
}
