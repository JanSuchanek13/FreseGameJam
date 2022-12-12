using UnityEngine;

public class SplashSound : MonoBehaviour
{
    /*
    #region variables:
    [SerializeField] AudioSource[] arrayOfSplashSounds;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource _randomTrack = arrayOfSplashSounds[Random.Range(0, arrayOfSplashSounds.Length)];
            _randomTrack.Play();
            GetComponent<Collider>().enabled = false; // splash just once!
            Invoke("Reset", 3f);
            Debug.Log("water splash: \"" + _randomTrack.name + "\" should have played!");
        }
    }

    private void Reset()
    {
        GetComponent<Collider>().enabled = true;
    }*/
}
