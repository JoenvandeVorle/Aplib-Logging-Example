using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Logging;

/// <summary>
/// Represents an agent that performs actions based on goals and beliefs.
/// Implements the <see cref="ILoggableAgent{TBeliefSet}" /> interface to allow logging of the agent's actions.
/// </summary>
public class LoggableBdiAgent<TBeliefSet> : ILoggableAgent<TBeliefSet>
    where TBeliefSet : IBeliefSet
{
    /// <summary>
    /// Gets the beliefset of the agent.
    /// </summary>
    public TBeliefSet BeliefSet { get; private set; }

    /// <summary>
    /// Gets the desire of the agent.
    /// </summary>
    /// <remarks>
    /// The desire contains all goal structures and the current goal.
    /// </remarks>
    public DesireSet<TBeliefSet> DesireSet { get; private set; }

    /// <inheritdoc />
    public CompletionStatus Status => DesireSet.Status;

    // / <summary>
    // / Initializes a new instance of the <see cref="BdiAgent{TBeliefSet}" /> class.
    // / </summary>
    // / <param name="beliefSet">The beliefset of the agent.</param>
    // /// <param name="desireSet"></param>
    public LoggableBdiAgent(TBeliefSet beliefSet, DesireSet<TBeliefSet> desireSet)
    {
        BeliefSet = beliefSet;
        DesireSet = desireSet;
    }

    /// <summary>
    /// Performs a single BDI cycle, in which the agent updates its beliefs, selects a concrete goal,
    /// chooses a concrete action to achieve the selected goal, and executes the chosen action.
    /// </summary>
    /// <remarks>This method will get called every frame of the game.</remarks>
    public void Update()
    {
        // Belief
        BeliefSet.UpdateBeliefs();

        // Desire
        DesireSet.Update(BeliefSet);
        if (Status != CompletionStatus.Unfinished) return;
        IGoal<TBeliefSet> goal = DesireSet.GetCurrentGoal(BeliefSet);

        // Intent
        ITactic<TBeliefSet> tactic = goal.Tactic;
        IAction<TBeliefSet>? action = tactic.GetAction(BeliefSet);

        // Execute the action
        action?.Execute(BeliefSet);
    }
}
