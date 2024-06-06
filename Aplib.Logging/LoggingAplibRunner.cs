
using Aplib.Core.Belief.BeliefSets;
using Serilog.Core;

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
    protected Logger _logger;

    protected LoggingAplibRunner(LoggableBdiAgent<TBeliefSet> agent, Logger logger)
    {
        _agent = agent;
        _logger = logger;
    }
}
