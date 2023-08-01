using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHiddenObj : MonoBehaviour
{
    public GameObject hiddenObj;

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.H))
        {
            Debug.Log("Hidden Obj now shown");
            hiddenObj.SetActive(true);
        }
    }
}
