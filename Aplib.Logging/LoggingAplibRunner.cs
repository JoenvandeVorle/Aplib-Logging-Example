using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
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
    protected Dictionary<IGoal<TBeliefSet>, int> _goalTree = [];

    protected LoggableDesireSet<TBeliefSet> DesireSet => _agent.DesireSet;

    protected LoggingAplibRunner(LoggableBdiAgent<TBeliefSet> agent, ILogger logger)
    {
        _agent = agent;
        _logger = logger;
    }


    /// <summary>
    /// Runs the test for the agent. The test continues until the agent's status is no longer Unfinished.
    /// </summary>
    /// <returns>True if the agent succeeded, false otherwise.</returns>
    public bool Test()
    {
        TBeliefSet beliefSet = _agent.BeliefSet;
        _logger.LogInformation("Test is starting with DesireSet -{DesireSetName}- ({DesireSetDescription}) and BeliefSet -{BeliefSetName}-", 
            DesireSet.GetMetadata().Name, DesireSet.GetMetadata().Description, beliefSet.GetType().Name);

        while (_agent.Status == CompletionStatus.Unfinished)
        {
            LogCurrentAction(beliefSet);
            IGoal<TBeliefSet> goal = DesireSet.GetCurrentGoal(beliefSet);

            if (_goalTree.TryGetValue(goal, out int goalCount))
                _goalTree[goal] = goalCount + 1;
            else
                _goalTree[goal] = 1;

            DoWhileRunning();
            _agent.Update();
        }

        _logger.LogInformation("Test is finished with status {AgentStatus}", _agent.Status);
        LogGoalTree(beliefSet);

        return _agent.Status == CompletionStatus.Success;
    }

    protected virtual void LogCurrentAction(TBeliefSet beliefSet)
    {
        IGoal<TBeliefSet> goal = DesireSet.GetCurrentGoal(beliefSet);
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
        _logger.LogInformation("Goal tree:");
        foreach (var (goal, count) in _goalTree)
        {
            _logger.LogInformation("{GoalName} - {Count}", goal.GetMetadata().Name, count);
        }
    }

    /// <summary>
    /// The method that is called while the agent is running.
    /// This method can be overridden to add custom behavior to the test runner.
    /// </summary>
    protected virtual void DoWhileRunning()
    {
    }
}