using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableScript : MonoBehaviour
{


    void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            AttachPlatform myScript = other.gameObject.GetComponent<AttachPlatform>();
            myScript.enabled = true;
        }
    }

    void OnTriggerExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            AttachPlatform myScript = other.gameObject.GetComponent<AttachPlatform>();
            myScript.enabled = false;
        }
    }

}
       

    