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
    public GameObject VulcanoPP;

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
        StartCoroutine("CheckForNarrowSpace", lastTimeInVulcano);
    }

    IEnumerator CheckForNarrowSpace(float _lastTimeInVulcano)
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(_lastTimeInVulcano + "    " + timeInVulcano);
        if (_lastTimeInVulcano < timeInVulcano)
        {
            if (!isInVulcano)
            {
                VulcanoPP.GetComponent<Volume>().enabled = true;
            }
            if (VulcanoPP.GetComponent<Volume>().weight <= 0.5f)
            {
                VulcanoPP.GetComponent<Volume>().weight += Time.deltaTime * vulcanoTransitionSpeed;
            }
            isInVulcano = true;
        }
        else
        {
            
            if (VulcanoPP.GetComponent<Volume>().weight >= 0f)
            {
                VulcanoPP.GetComponent<Volume>().weight -= Time.deltaTime * vulcanoTransitionSpeed;
            }
            if (isInVulcano)
            {
                yield return new WaitForSeconds(1f);
                if (! (_lastTimeInVulcano < timeInVulcano))
                {
                    VulcanoPP.GetComponent<Volume>().enabled = false;
                }
            }
            isInVulcano = false;
        }
    }
}
