using Aplib.Core.Belief.BeliefSets;
using Aplib.Logging;
using Aplib_Logging_Example.GameExample;
using Microsoft.Extensions.Logging;

namespace Aplib_Logging_Example.AplibInterface
{
    public class SimpleGameAplibRunner<TBeliefSet> : LoggingAplibRunner<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        SimpleGame _game;

        public SimpleGameAplibRunner(LoggableBdiAgent<TBeliefSet> agent, ILogger logger, SimpleGame game) : base(agent, logger)
        {
            _game = game;
        }

        protected override void DoWhileRunning()
        {
            _game.Update();
        }
    }
}