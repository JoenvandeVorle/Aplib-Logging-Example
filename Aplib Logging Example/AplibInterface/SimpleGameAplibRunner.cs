using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Aplib.Logging;
using Aplib_Logging_Example.GameExample;
using Microsoft.Extensions.Logging;

namespace Aplib_Logging_Example.AplibInterface
{
    public class SimpleGameAplibRunner<TBeliefSet> : LoggingAplibRunner<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        public SimpleGameAplibRunner(LoggableBdiAgent<TBeliefSet> agent, ILogger logger) : base(agent, logger)
        {
        }

        /// <summary>
        /// Runs the test for the agent. The test continues until the agent's status is no longer Unfinished.
        /// </summary>
        /// <returns>True if the agent succeeded, false otherwise.</returns>
        public bool Test(SimpleGame game)
        {
            TBeliefSet beliefSet = _agent.BeliefSet;
            IDesireSet<TBeliefSet> desireSet = _agent.DesireSet;
            _logger.LogInformation("Test is starting with DesireSet {DesireSetName} and BeliefSet {BeliefSetName}", desireSet.GetMetadata().Name, beliefSet.GetType().Name);

            while (_agent.Status == CompletionStatus.Unfinished)
            {
                LogCurrentAction(beliefSet);

                game.Update();
                _agent.Update();
            }

            _logger.LogInformation("Test is finished with status {AgentStatus}", _agent.Status);

            return _agent.Status == CompletionStatus.Success;
        }
    }
}