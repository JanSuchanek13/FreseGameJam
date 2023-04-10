using UnityEngine;

public class WaitForPlayer : MonoBehaviour
{
    [SerializeField] bool stopSomethingWhenItGetsHere = false;
    [SerializeField] GameObject objectThatShouldWait;
    private bool _perpetuallyEnableBoat = false;
    //[SerializeField] GameObject ObjectThatShouldWait;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("i was impacted");
        if (stopSomethingWhenItGetsHere && other.gameObject == objectThatShouldWait)//.CompareTag("Friend"))
        {
            //Debug.Log("i was impacted by the boat");
            this.gameObject.GetComponent<Collider>().enabled = false;
            objectThatShouldWait.GetComponent<Patrolling>().WaitForPlayer();
            //other.GetComponentInChildren<Patrolling>().WaitForPlayer();
        }
        else if (!stopSomethingWhenItGetsHere && other.CompareTag("Player"))
        {
            //Debug.Log("i was impacted by the player");
            _perpetuallyEnableBoat = true;
            //objectThatShouldWait.GetComponent<Patrolling>().StopWaiting();
            //other.GetComponent<Patrolling>().StopWaiting();
        }
    }
    private void Update()
    {
        if (_perpetuallyEnableBoat)
        {
            objectThatShouldWait.GetComponent<Patrolling>().StopWaiting(); // this lets the boat sail past stoppoint when continuing a round and having already passed the unlock-spot!
        }
    }
}
