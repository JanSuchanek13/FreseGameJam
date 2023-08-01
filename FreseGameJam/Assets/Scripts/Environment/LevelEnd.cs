using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public void StopTheClock()
    {
        Debug.Log("hardcore finished");

        FindObjectOfType<HardcoreMode>().runFinished = true;
    }
}
