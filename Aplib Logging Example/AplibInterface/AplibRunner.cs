using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
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
            while (_agent.Status == CompletionStatus.Unfinished)
            {
                _agent.Update();
                game.Update();

                DesireSet<TBeliefSet> desireSet = _agent.DesireSet;
                IMetadata desireData = desireSet.Metadata;

                _logger.Information("Desire: {Desire}", desireData.Name);
            }

            return _agent.Status == CompletionStatus.Success;
        }
    }
}