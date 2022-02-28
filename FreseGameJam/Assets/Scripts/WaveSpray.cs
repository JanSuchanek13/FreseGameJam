using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpray : MonoBehaviour
{
    private IEnumerator coroutine;
    public float startTime;
    public float repetitionTime;

    private void Start()
    {
        StartCoroutine(SpawnSpray());
    }

    private IEnumerator SpawnSpray()
    {
        yield return new WaitForSeconds(startTime);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        while (true)
        {
            yield return new WaitForSeconds(repetitionTime);
            gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
        }
        


    }
}
