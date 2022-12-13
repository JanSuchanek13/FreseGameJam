using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpray : MonoBehaviour
{
    private IEnumerator coroutine;
    public float minTime = 2;
    public float maxTime = 5;
    public float lifeTime = 1;

    private void Start()
    {
        StartCoroutine(SpawnSpray());
    }

    private IEnumerator SpawnSpray()
    {
        
        //gameObject.GetComponent<MeshRenderer>().enabled = true;
        while (true)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
            yield return new WaitForSeconds(lifeTime);
        }
        


    }
}
