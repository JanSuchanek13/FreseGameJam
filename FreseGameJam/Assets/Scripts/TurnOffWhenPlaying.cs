using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffWhenPlaying : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
