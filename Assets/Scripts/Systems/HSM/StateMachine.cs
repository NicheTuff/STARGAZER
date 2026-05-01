using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.HSM
{
    // State Machine implementation heavily inspired by this bloke on YouTube: https://youtu.be/c-XoTg6Fba4?si=9mmc9SnlWlmyl2bG

    /// <summary>
    /// Hierarchical state machine system; each state can have an arbitrary number of parents and children.
    /// </summary>
    public abstract class StateMachine : MonoBehaviour
    {
        //TODO:
        // Standardized method of implementation for state keys 
        // Like an enum or smth
        // psuedocode:
        // enum statekeys: default = 0
        // InitializeStates(): RootStates.Add(statekeys.default, koistate.idle)
        // and defaultstate should always default to rootstates[statekeys.default] or rootstates[0] which is why some standardized method is required
        // this comment does nothing and im just testing out the discord webhook
        protected Dictionary<int, State> RootStates;
        protected abstract void InitializeStates();
        public abstract State DefaultState { get; protected set; }
        public abstract State ActiveState { get; protected set; }

        void Start()
        {
            InitializeStates();
            ActiveState = DefaultState;
            ActiveState.HierarchyEnter();
        }

        private void Update() => ActiveState?.HierarchyUpdate();
        private void FixedUpdate() => ActiveState?.HierarchyFixedUpdate();
        private void OnCollisionEnter2D(Collision2D collision) => ActiveState?.HierarchyCollisionEnter2D(collision);
        private void OnCollisionExit2D(Collision2D collision) => ActiveState?.HierarchyCollisionExit2D(collision);

        public void SwitchState(State current, State next)
        {
            if (current == null || next == null || !current.Active || current == next) return;

            // If the target state is on an entirely new tree, fully exit out of the current tree before swapping ActiveTree to the new tree and entering down to the target state.
            if (current.Root != next.Root)
            {
                ActiveState.Root.HierarchyExit(false);
                ActiveState = next;
                next.HierarchyEnter();
                return;
            }

            // Otherwise, determine and exit up to the common root before entering down to the target state
            var state = CommonAncestor(current, next);
            state.HierarchyExit(true);
            next.HierarchyEnter();
        }

        /// <summary>
        /// Returns the root state (earliest shared parent state) of two states.
        /// <para>Useful for dynamically switching states no matter how deep they are in a hierarchy.</para>
        /// <para>If null, state1 and state2 are on different trees entirely.</para>
        /// </summary>
        public static State CommonAncestor(State state1, State state2)
        {
            var check = new HashSet<State>();
            for (var i = state1; i != null; i = i.Parent)
            {
                check.Add(i);
            }
            for (var i = state2; i != null; i = i.Parent)
            {
                if (check.Contains(i)) return i;
            }
            return null;
        }
    }
}