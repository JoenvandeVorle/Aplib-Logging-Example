
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Collections;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Microsoft.Extensions.Logging;

namespace Aplib.Logging;

/// <summary>
/// An Aplib runner that allows for logging of the agent's actions.
/// </summary>
public abstract class LoggingAplibRunner<TBeliefSet> where TBeliefSet : IBeliefSet
{
    /// <summary>
    /// The agent that the test runner is testing.
    /// </summary>
    protected readonly LoggableBdiAgent<TBeliefSet> _agent;

    /// <summary>
    /// The logger used to log the agent's actions.
    /// </summary>
    protected ILogger _logger;

    /// <summary>
    /// The stack of goal structures that the agent is working with.
    /// Keeps track of how long the agent has been working on each goal structure.
    /// </summary>
    protected Dictionary<IGoalStructure<TBeliefSet>, int> _goalTree = new();

    protected LoggingAplibRunner(LoggableBdiAgent<TBeliefSet> agent, ILogger logger)
    {
        _agent = agent;
        _logger = logger;
    }

    protected virtual void LogCurrentAction(TBeliefSet beliefSet)
    {
        IDesireSet<TBeliefSet> desireSet = _agent.DesireSet;
        IGoal<TBeliefSet> goal = desireSet.GetCurrentGoal(beliefSet);
        ITactic<TBeliefSet> tactic = goal.Tactic;
        IAction<TBeliefSet>? action = tactic.GetAction(beliefSet);

        _logger.LogInformation("Current goal: {GoalName} -- Current Tactic: {TacticName} -- Current Action: {ActionName}",
            goal.GetMetadata().Name, tactic.GetMetadata().Name, action.GetMetadata().Name);
    }

    /// <summary>
    /// Logs the GoalStack of the DesireSet as a tree.
    /// </summary>
    protected virtual void LogGoalTree(TBeliefSet beliefSet)
    {
        LoggableDesireSet<TBeliefSet> desireSet = _agent.DesireSet;
        IGoal<TBeliefSet> goal = desireSet.GetCurrentGoal(beliefSet);
        ITactic<TBeliefSet> tactic = goal.Tactic;
        IAction<TBeliefSet>? action = tactic.GetAction(beliefSet);

        _logger.LogInformation("Agent status is {AgentStatus}. Current goal: {GoalName} -- Current Tactic: {TacticName} -- Current Action: {ActionName}",
            _agent.Status, goal.GetMetadata().Name, tactic.GetMetadata().Name, action.GetMetadata().Name);
    }
}
