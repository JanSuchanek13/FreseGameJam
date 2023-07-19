using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrowSpaceDetector : MonoBehaviour
{
    [Header("RAYCAST")]
    private bool isInNarrowSpace;
    private float timeInNarrowSpace = 0;
    private float lastTimeInNarrowSpace = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NarrowSpace"))
        {
            
            timeInNarrowSpace += Time.deltaTime;
        }
    }

    private void Update()
    {
        lastTimeInNarrowSpace = timeInNarrowSpace;
        //Invoke("CheckForNarrowSpace", 1f);
        StartCoroutine("CheckForNarrowSpace", lastTimeInNarrowSpace);
    }

    IEnumerator CheckForNarrowSpace(float _lastTimeInNarrowSpace)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(_lastTimeInNarrowSpace + "    " + timeInNarrowSpace);
        if (_lastTimeInNarrowSpace < timeInNarrowSpace)
        {
            isInNarrowSpace = true;
        }
        else
        {
            isInNarrowSpace = false;
        }
    }
}
