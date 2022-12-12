using UnityEngine;

public class PlayerHumanState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("changed to HumanState");
    }
    public override void UpdateState(StateManager player)
    {
        //player.SwitchState(player.CraneState);

    }
    public override void OnCollisionState(StateManager player)
    {

    }
}
