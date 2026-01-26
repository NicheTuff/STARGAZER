using System.Collections.Generic;
using UnityEngine;

// State Machine implementation heavily inspired by this bloke on YouTube: https://youtu.be/c-XoTg6Fba4?si=9mmc9SnlWlmyl2bG

/// <summary>
/// Hierarchical state machine system; each state can have an arbitrary number of parents and children.
/// </summary>
public abstract class StateMachine : MonoBehaviour
{
    public readonly StateSequencer Sequencer;
    public StateMachine()
    {
        Sequencer = new StateSequencer(this);
    }

    /// <summary>
    /// This is where the tree states, or the highest-level states acting on a given state machine, are stored.
    /// </summary>
    public abstract Dictionary<int, State> TreeStates { get; set; }

    public abstract State DefaultTree { get; set; }
    public State ActiveTree { get; set; }

    void Start()
    {
        foreach (var state in TreeStates) state.Value.HierarchyStart();
        ActiveTree = DefaultTree;
        ActiveTree.HierarchyEnter();
    }

    private void Update() => Sequencer.SequenceUpdate();

    public void StateUpdate() => ActiveTree.HierarchyUpdate(Time.deltaTime);
    private void FixedUpdate() => ActiveTree.HierarchyFixedUpdate();
    private void OnCollisionEnter2D(Collision2D collision) => ActiveTree.HierarchyCollisionEnter2D(collision);
    private void OnCollisionExit2D(Collision2D collision) => ActiveTree.HierarchyCollisionExit2D(collision);


    public void SwitchState(State source,  State next)
    {
        if (source == null || next == null || !source.Active || source == next) return; 
        bool isNewTree = TreeStates.ContainsValue(next);

        // If the target state is on an entirely new tree, fully exit out of the current tree before swapping ActiveTree to the new tree and entering down to the target state.
        if (isNewTree)
        {
            ActiveTree.HierarchyExit(true);                                     
            ActiveTree = GetTree(next);
            next.HierarchyEnter();
            return;
        }

        // Otherwise, determine and exit up to the common root before entering down to the target state
        var state = CommonRootState(source, next);                                                      
        state.HierarchyExit();                                                                          
        next.HierarchyEnter();                                                                          
    }

    /// <summary>
    /// Returns the root state (earliest shared parent state) of two states.
    /// <para>Useful for dynamically switching states no matter how deep they are in a hierarchy.</para>
    /// <para>If null, state a and state b are on different trees entirely.</para>
    /// </summary>
    public static State CommonRootState(State a, State b)
    {
        var hierarchy = new Stack<State>();
        for (var i = a; i != null; i = i.Parent) hierarchy.Push(i);                    
        for (var i = b; i != null; i = i.Parent) if (hierarchy.Contains(i) || i == hierarchy.Peek()) return i;
        return null;                                                                  
    }

    public static State GetTree(State state)
    {
        for (var i = state; i != null; i = i.Parent) state = i;
        return state;
    }
}
