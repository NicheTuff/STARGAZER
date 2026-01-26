using System;

public class StateSequencer
{
    StateMachine Machine;

    IStateSequence Sequence;
    Action NextPhase;
    (State source, State next)? QueuedTransition;
    State lastFrom, lastTo; // Records the states last switched out of and into

    public StateSequencer(StateMachine machine) => Machine = machine;

    public void RequestTransition(State source, State next)
    {
        if (next == null || next == source || !source.Active) return;
        if (Sequence != null) QueuedTransition = (source, next);
        BeginTransition(source, next);
    }

    public void BeginTransition(State source, State next)
    {
        Sequence = source.ExitSequence();
        Sequence.Start();

        NextPhase = () => //TODO: Figure out wtf this lambda is doing
        {
            Machine.SwitchState(source, next); 
            Sequence = next.EnterSequence(); 
            Sequence.Start();
        };

    }

    public void EndTransition(State source, State next)
    {
        Sequence = null;
        if (QueuedTransition.HasValue)
        {
            var q = QueuedTransition.Value;
            QueuedTransition = null;
            BeginTransition(q.source, q.next);
        }
    }

    public void SequenceUpdate() //TODO: Finish this; intended behavior is that StateUpdate() doesn't run until the states have fully transitioned and the sequence has run both its exit and enter behavior
    {
        if (Sequence != null && Sequence.Update() == true)
        {
            Sequence.Update();
        }
        Machine.StateUpdate();
    }
}

//TODO: Make IStateSequence templates, e.g. a noop template
public interface IStateSequence
{
    bool IsComplete { get; protected set; }
    void Start();
    bool Update();
}