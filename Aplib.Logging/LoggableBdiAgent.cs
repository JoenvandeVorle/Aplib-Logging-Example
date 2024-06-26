using Aplib.Core.Agents;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;

namespace Aplib.Logging;

/// <summary>
/// Represents an agent that performs actions based on goals and beliefs.
/// Implements the <see cref="ILoggableAgent{TBeliefSet}" /> interface to allow logging of the agent's actions.
/// </summary>
public class LoggableBdiAgent<TBeliefSet> : BdiAgent<TBeliefSet>
    where TBeliefSet : IBeliefSet
{
    public DesireSet<TBeliefSet> DesireSet { get; }
    public TBeliefSet BeliefSet { get; }

    public LoggableBdiAgent(TBeliefSet beliefSet, DesireSet<TBeliefSet> desireSet)
        : base(beliefSet, desireSet)
    {
        BeliefSet = beliefSet;
        DesireSet = desireSet;
    }
}
