using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Aplib.Logging;
using Aplib_Logging_Example.GameExample;
using Serilog.Core;

namespace Aplib_Logging_Example.AplibInterface
{
    public class AplibRunner<TBeliefSet> : LoggingAplibRunner<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        public AplibRunner(LoggableBdiAgent<TBeliefSet> agent, Logger logger) : base(agent, logger)
        {
        }

        /// <summary>
        /// Runs the test for the agent. The test continues until the agent's status is no longer Unfinished.
        /// </summary>
        /// <returns>True if the agent succeeded, false otherwise.</returns>
        public bool Test(SimpleGame game)
        {
            TBeliefSet beliefSet = _agent.BeliefSet;
            DesireSet<TBeliefSet> desireSet = _agent.DesireSet;
            _logger.Information($"Test is starting with DesireSet {desireSet.Metadata.Name} and BeliefSet {beliefSet.GetType().Name}");

            while (_agent.Status == CompletionStatus.Unfinished)
            {
                IGoal<TBeliefSet> goal = desireSet.GetCurrentGoal(beliefSet);
                ITactic<TBeliefSet> tactic = goal.Tactic;
                IAction<TBeliefSet>? action = tactic.GetAction(beliefSet);

                _logger.Information($"Agent status is {_agent.Status}. Current goal: {goal} \n"
                    + $"Current Tactic:{tactic} -- Current Action:{action}"); // TODO: add metadata to IGoal, ITactic, and IAction

                game.Update();
                _agent.Update();
            }

            return _agent.Status == CompletionStatus.Success;
        }
    }
}