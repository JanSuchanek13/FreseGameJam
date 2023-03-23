using UnityEngine;

public class ActivateGravityOnTouch : MonoBehaviour
{
    /// <summary>
    /// If you want this gras to remain static, simply delete this script of the objects instance in your scene
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
