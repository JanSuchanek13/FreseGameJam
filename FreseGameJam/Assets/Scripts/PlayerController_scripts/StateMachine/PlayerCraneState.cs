using UnityEngine;

public class PlayerCraneState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("changed to CraneState");
    }
    public override void UpdateState(StateManager player)
    {

    }
    public override void OnCollisionState(StateManager player)
    {

    }
}
