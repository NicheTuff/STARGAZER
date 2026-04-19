using System.Collections.Generic;
using UnityEngine;

namespace Assets.Systems.StateMachineSpace
{
    public abstract class State
    {
        public StateMachine Machine { get; private set; }
        public State Parent { get; private set; }
        public State(StateMachine machine, State parent)
        {
            Machine = machine;
            Parent = parent;
        }

        /// <summary>
        /// The "base-level" state at the surface of a given hierarchy
        /// </summary>
        public State Root => Parent == null ? this : Parent.Root;

        public bool Active { get; private set; }

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

        /// <summary>
        /// Runs once before update logic with the state is first entered
        /// </summary>
        protected virtual void OnEnter() { }
        /// <summary>
        /// Entering a state will activate each of its parent states, then itself, then each child state from top to bottom.<br/>
        /// A "leaf" cannot be activated without activating the entire branch.
        /// </summary>
        public void HierarchyEnter()
        {
            if (Active) return;
            Parent?.HierarchyEnter();
            if (Parent != null)
            {
                Parent.ActiveChild = this;
            }
            Active = true;
            OnEnter();
            if (GetDefaultChild() != null)
            {
                ActiveChild = GetDefaultChild();
                ActiveChild.OnEnter();
            }
        }

        protected virtual void OnExit() { }
        public void HierarchyExit(bool remainActive = false)
        {
            ActiveChild?.HierarchyExit();
            ActiveChild = null;
            if (remainActive == true) return;
            Active = false;
            OnExit();
        }

        protected virtual void OnUpdate() { }
        public void HierarchyUpdate()
        {
            State transition = GetTransition();
            if (transition != null)
            {
                Machine.SwitchState(this, transition);
                return;
            }
            OnUpdate();
            ActiveChild?.HierarchyUpdate();
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
            ActiveChild?.HierarchyCollisionExit2D(collision);
        }
    }
}