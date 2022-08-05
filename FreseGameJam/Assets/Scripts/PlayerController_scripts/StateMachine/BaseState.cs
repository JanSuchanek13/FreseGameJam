using UnityEngine;

public abstract class BaseState
{
    /// <summary>
    /// holds current state
    /// defines Methodes for States
    /// </summary>

    public abstract void EnterState(StateManager player);
    public abstract void UpdateState(StateManager player);
    public abstract void OnCollisionState(StateManager player);

}
