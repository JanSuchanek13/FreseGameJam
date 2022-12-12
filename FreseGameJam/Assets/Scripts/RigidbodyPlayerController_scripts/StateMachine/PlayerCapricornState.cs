using UnityEngine;

public class PlayerCapricornState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("changed to CapricornState");
    }
    public override void UpdateState(StateManager player)
    {

    }
    public override void OnCollisionState(StateManager player)
    {

    }
}
