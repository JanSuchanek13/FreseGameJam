using UnityEngine;

public class TurnOffWhenPlaying : MonoBehaviour
{
    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
