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

    [Header("LightFlash_PP")]
    public GameObject lightFlashPP;
    private float lightFlashTransitionSpeed = 0.3f;
    private bool inLightFlash;

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "InsideVulcano_PP")
        {

            timeInVulcano += Time.deltaTime;
        }
    }

    private void Update()
    {
        lastTimeInVulcano = timeInVulcano;
        //Invoke("CheckForNarrowSpace", 1f);
        StartCoroutine("UpdateVulcano", lastTimeInVulcano);
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
                if (!inLightFlash)
                {
                    StartCoroutine("LightFlash");
                }
                
                yield return new WaitForSeconds(1f);
                if (! (_lastTimeInVulcano < timeInVulcano))
                {
                    vulcanoPP.GetComponent<Volume>().enabled = false;
                }
            }
            isInVulcano = false;
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
        lightFlashPP.GetComponent<Volume>().weight = 1;
        inLightFlash = false;
    }
        
}
