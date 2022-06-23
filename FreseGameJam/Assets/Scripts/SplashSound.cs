using UnityEngine;

public class SplashSound : MonoBehaviour
{
    #region variables:
    [SerializeField] AudioSource splashSound;
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            splashSound.Play();
            GetComponent<Collider>().enabled = false; // splash just once!
            Invoke("Reset", 1f);
            Debug.Log("this was triggered");
        }
    }
    private void Reset()
    {
        GetComponent<Collider>().enabled = true;
    }
}
