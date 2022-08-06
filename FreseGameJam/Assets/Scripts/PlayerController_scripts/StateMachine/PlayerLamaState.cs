using UnityEngine;

public class PlayerLamaState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("changed to LamaState");
    }
    public override void UpdateState(StateManager player)
    {

    }
    public override void OnCollisionState(StateManager player)
    {

    }

}
