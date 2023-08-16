using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHiddenObj : MonoBehaviour
{
    public GameObject hiddenObj;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.H))
        {
            Debug.Log("Hidden Obj now shown");
            hiddenObj.SetActive(true);
        }
    }
}
