using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    // this script sits on the trigger that is supposed to play a given Sound from a specific "DelaySound"-carrying GO.
    // this script is deactivated by "DelaySound" after being triggered to avoid triggering again.
    #region variables
    public bool gotActivated = false;
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("something collided wuitg water");
        if (other.CompareTag("Player"))
        {
            gotActivated = true; // this tells "DelaySound" to play its sound.
            //Debug.Log("it was player");
        }
    }
}
