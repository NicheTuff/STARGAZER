using UnityEngine;

public abstract class State
{
    public StateMachine<State> Machine { get; set; }
    public StateFactory<State> Factory { get; set; }
    public State(StateMachine<State> machine, StateFactory<State> factory)
    {
        Machine = machine;
        Factory = factory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void UpdateStateFixed();
    public abstract void OnCollision(Collision collision);
}
