using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    // this script sits on the trigger that is supposed to play a given Sound from a specific "DelaySound"-carrying GO.
    // this script is deactivated by "DelaySound" after being triggered to avoid triggering again.
    #region variables
    public bool gotActivated = false;
    public DelaySound delaySound;
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gotActivated = true; // this tells "DelaySound" to play its sound.
            delaySound.PlaySound();

            // turn off this script to avoid calling again:
            this.enabled = false;
        }
    }
}
