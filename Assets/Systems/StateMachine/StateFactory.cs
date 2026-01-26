using System.Collections.Generic;

public abstract class StateFactory<StateType> where StateType : State
{
    public StateMachine<StateType> Machine;
    public List<State> States;
    public StateFactory(StateMachine<StateType> machine)
    {
        Machine = machine;
    }
    public abstract State DefaultState { get; protected set; }
}