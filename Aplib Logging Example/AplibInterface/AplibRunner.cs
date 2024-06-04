using Aplib.Core;
using Aplib_Logging_Example.GameExample;

namespace Aplib_Logging_Example.AplibInterface
{
    public class AplibRunner
    {
        /// <summary>
        /// The agent that the test runner is testing.
        /// </summary>
        private readonly IAgent _agent;

        public AplibRunner(IAgent agent)
        {
            _agent = agent;
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
            }

            return _agent.Status == CompletionStatus.Success;
        }
    }
}