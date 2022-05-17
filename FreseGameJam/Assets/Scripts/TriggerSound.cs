using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public bool gotActivated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gotActivated = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //tst
    }
}
