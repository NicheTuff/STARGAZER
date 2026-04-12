using System.Collections.Generic;
using UnityEngine;

// State Machine implementation heavily inspired by this bloke on YouTube: https://youtu.be/c-XoTg6Fba4?si=9mmc9SnlWlmyl2bG

/// <summary>
/// Hierarchical state machine system; each state can have an arbitrary number of parents and children.
/// </summary>
public abstract class StateMachine : MonoBehaviour
{
    /// <summary>
    /// This is where the tree states, or the highest-level states acting on a given state machine, are stored.
    /// </summary>
    public abstract Dictionary<int, State> TreeStates { get; set; }
    public abstract State DefaultTree { get; set; }
    public State ActiveTree { get; set; }

    void Start()
    {
        ActiveTree = DefaultTree;
        ActiveTree.HierarchyEnter();
    }

    public void Update() => ActiveTree.HierarchyUpdate();
    private void FixedUpdate() => ActiveTree.HierarchyFixedUpdate();
    private void OnCollisionEnter2D(Collision2D collision) => ActiveTree.HierarchyCollisionEnter2D(collision);
    private void OnCollisionExit2D(Collision2D collision) => ActiveTree.HierarchyCollisionExit2D(collision);

    public void SwitchState(State current, State next)
    {
        if (current == null || next == null || !current.Active || current == next) return;
        bool isNewTree = TreeStates.ContainsValue(next);

        // If the target state is on an entirely new tree, fully exit out of the current tree before swapping ActiveTree to the new tree and entering down to the target state.
        if (isNewTree)
        {
            ActiveTree.HierarchyExit(true);                                     
            ActiveTree = next.Tree;
            next.HierarchyEnter();
            return;
        }

        // Otherwise, determine and exit up to the common root before entering down to the target state
        var state = CommonRootState(current, next);
        state.HierarchyExit();
        next.HierarchyEnter();
    }

    /// <summary>
    /// Returns the root state (earliest shared parent state) of two states.
    /// <para>Useful for dynamically switching states no matter how deep they are in a hierarchy.</para>
    /// <para>If null, state1 and state2 are on different trees entirely.</para>
    /// </summary>
    public static State CommonRootState(State state1, State state2)
    {
        var check = new HashSet<State>();
        for (var i = state1; i != null; i = i.Parent) check.Add(i);
        for (var i = state2; i != null; i = i.Parent)
        {
            if (check.Contains(i)) return i;
        }
        return null;                                                                  
    }
}