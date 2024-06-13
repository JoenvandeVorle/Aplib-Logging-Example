using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Microsoft.Extensions.Logging;
using System.Text;

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

    protected DesireSet<TBeliefSet> DesireSet => _agent.DesireSet;

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
        _logger.LogInformation("Test is starting with DesireSet: {DesireSetName} -{DesireSetDescription}- and BeliefSet: {BeliefSetName}", 
            DesireSet.GetMetadata().Name, DesireSet.GetMetadata().Description, beliefSet.GetType().Name);

        LogGoalTree(beliefSet);

        while (_agent.Status == CompletionStatus.Unfinished)
        {
            IGoal<TBeliefSet> goal = DesireSet.GetCurrentGoal(beliefSet);

            if (goal is ILoggable loggable)
            {
                LogCurrentGoal(beliefSet, loggable);
            }
            else 
                _logger.LogError("Current goal is not loggable!");
                

            if (_goalTree.TryGetValue(goal, out int goalCount))
                _goalTree[goal] = goalCount + 1;
            else
                _goalTree[goal] = 1;

            DoWhileRunning();
            _agent.Update();
        }

        _logger.LogInformation("Test is finished with status {AgentStatus}", _agent.Status);

        return _agent.Status == CompletionStatus.Success;
    }

    protected virtual void LogCurrentGoal(TBeliefSet beliefSet, ILoggable loggable)
    {
        IAction<TBeliefSet>? action = DesireSet.GetCurrentGoal(beliefSet).Tactic.GetAction(beliefSet);

        LogNode tree = loggable.GetLogTree(0);
        _logger.LogInformation("Executing cycle with\nCurrent Action: {ActionName}\nTree:{Tree} ",
            action.GetMetadata().Name, LogNodeToString(tree));
    }

    /// <summary>
    /// Logs the entire goal tree of the agent.
    /// </summary>
    protected virtual void LogGoalTree(TBeliefSet beliefSet)
    {
        if (DesireSet is ILoggable loggable)
        {
            _logger.LogInformation("Full tree:\n{DesireSet}", LogNodeToString(loggable.GetLogTree(0)));
        }
        else 
            _logger.LogError("Current goal is not loggable!");
    }

    protected string LogNodeToString(LogNode node)
    {
        int depth = node.Depth;
        string indent = "---- ";

        StringBuilder stringBuilder = new();
        stringBuilder.Append(string.Concat(Enumerable.Repeat(indent, depth)));
        stringBuilder.Append(node.Loggable.Metadata.Name);
        stringBuilder.Append('\n');

        foreach (LogNode child in node.Children)
        {
            stringBuilder.Append(LogNodeToString(child));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// The method that is called while the agent is running.
    /// This method can be overridden to add custom behavior to the test runner.
    /// </summary>
    protected virtual void DoWhileRunning()
    {
    }
}