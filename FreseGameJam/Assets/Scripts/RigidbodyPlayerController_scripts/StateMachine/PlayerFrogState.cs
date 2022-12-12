using UnityEngine;

public class PlayerFrogState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("changed to FrogState");
    }
    public override void UpdateState(StateManager player)
    {

    }
    public override void OnCollisionState(StateManager player)
    {

    }
}
