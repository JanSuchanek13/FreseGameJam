using UnityEngine;

public class WaitForPlayer : MonoBehaviour
{
    [SerializeField] bool stopSomethingWhenItGetsHere = false;
    [SerializeField] GameObject objectThatShouldWait;
    //[SerializeField] GameObject ObjectThatShouldWait;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("i was impacted");
        if (stopSomethingWhenItGetsHere && other.gameObject == objectThatShouldWait)//.CompareTag("Friend"))
        {
            Debug.Log("i was impacted by the boat");
            this.gameObject.GetComponent<Collider>().enabled = false;
            objectThatShouldWait.GetComponent<Patrolling>().WaitForPlayer();
            //other.GetComponentInChildren<Patrolling>().WaitForPlayer();
        }
        else if (!stopSomethingWhenItGetsHere && other.CompareTag("Player"))
        {
            Debug.Log("i was impacted by the player");

            objectThatShouldWait.GetComponent<Patrolling>().StopWaiting();
            //other.GetComponent<Patrolling>().StopWaiting();
        }
    }
}
