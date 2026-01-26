using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public readonly StateMachine Machine;
    public readonly State Parent;
    public bool Active = false;
    public State(StateMachine machine, State parent)
    {
        Machine = machine;
        Parent = parent;
    }

    /// <summary>
    /// Use to create child states that are inactive whenever this state is too. Each should take 'Machine' and 'this' as first and second arguments.
    /// </summary>
    public virtual List<State> ChildStates { get; set; }
    public State ActiveChild;

    protected virtual State GetDefaultChild() => null;
    /// <summary>
    /// Place logic for choosing when to switch states.
    /// </summary>
    /// <returns>null if this state should remain active; otherwise, returns the state to switch into.</returns>
    protected virtual State GetTransition() => null;

    public abstract IStateSequence EnterSequence();
    public abstract IStateSequence ExitSequence();

    protected virtual void OnStart() { }
    public void HierarchyStart()
    {
        OnStart();
        ActiveChild = GetDefaultChild();
        foreach (var child in ChildStates) child.HierarchyStart();
    }

    protected virtual void OnEnter() { }
    /// <summary>
    /// Entering a state will activate each of its parent states, then itself, then each child state from top to bottom.<br/>
    /// A "leaf" cannot be activated without activating the entire branch.
    /// </summary>
    public void HierarchyEnter()
    {
        Parent?.HierarchyEnter();
        if (Parent != null) Parent.ActiveChild = this;
        Active = true;
        OnEnter();
        ActiveChild?.HierarchyEnter();
    }

    protected virtual void OnExit() { }
    public void HierarchyExit(bool rootRemainsActive = false)
    {
        ActiveChild?.HierarchyExit();
        ActiveChild = null;
        if (rootRemainsActive) return;
        Active = false;
        OnExit();
    }

    protected virtual void OnUpdate(float deltaTime) { }
    public void HierarchyUpdate(float deltaTime)
    {
        State transition = GetTransition();
        if (transition != null) Machine.SwitchState(this, transition);
        OnUpdate(deltaTime);
        ActiveChild?.HierarchyUpdate(deltaTime);
    }
    
    protected virtual void OnFixedUpdate() { }
    public void HierarchyFixedUpdate()
    {
        OnFixedUpdate();
        ActiveChild?.HierarchyFixedUpdate();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    public void HierarchyCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
        ActiveChild?.HierarchyCollisionEnter2D(collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision) { }
    public void HierarchyCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit2D(collision);
        ActiveChild?.OnCollisionExit2D(collision);
    }
}
