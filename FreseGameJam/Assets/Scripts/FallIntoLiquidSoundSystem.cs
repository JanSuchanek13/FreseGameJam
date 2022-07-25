using UnityEngine;

public class FallIntoLiquidSoundSystem : MonoBehaviour
{
    // this script only exists because "SplashSound"-Script on the water and Lava-prefabs does not work in the game world. I cant figure out why.
    #region variables:
    [SerializeField] AudioSource[] arrayOfWaterSplashSounds;
    [SerializeField] AudioSource[] arrayOfLavaSplashSounds;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            if (other.name == "WATER_prefab")
            {
                AudioSource _randomTrack = arrayOfWaterSplashSounds[Random.Range(0, arrayOfWaterSplashSounds.Length)];
                _randomTrack.Play();
            }
            else if (other.name == "Lava")
            {
                AudioSource _randomTrack = arrayOfLavaSplashSounds[Random.Range(0, arrayOfLavaSplashSounds.Length)];
                _randomTrack.Play();
            }
        }
    }
}
