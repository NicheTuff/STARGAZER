using UnityEngine;

public abstract class StateMachine<StateType> : MonoBehaviour where StateType : State
{
    public abstract StateFactory<StateType> States { get; protected set; }
    public State CurrentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = States.DefaultState;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.UpdateState();
    }
}
