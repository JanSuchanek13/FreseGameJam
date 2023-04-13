using UnityEngine;

public class FallIntoLiquidSoundSystem : MonoBehaviour
{
    #region variables:
    [SerializeField] AudioSource[] arrayOfWaterSplashSounds;
    [SerializeField] ParticleSystem waterSplashEffect;
    [SerializeField] AudioSource[] arrayOfLavaSplashSounds;
    [SerializeField] ParticleSystem lavalSplashEffect;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            if (other.name == "Water_deathZone")
            {
                AudioSource _randomTrack = arrayOfWaterSplashSounds[Random.Range(0, arrayOfWaterSplashSounds.Length)];
                _randomTrack.Play();
                waterSplashEffect.Play();
            }
            if (other.name == "Lava_deathZone")
            {
                AudioSource _randomTrack = arrayOfLavaSplashSounds[Random.Range(0, arrayOfLavaSplashSounds.Length)];
                _randomTrack.Play();
                lavalSplashEffect.Play();
            }
        }
    }
}
