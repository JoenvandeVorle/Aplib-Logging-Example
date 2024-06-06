
using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;

namespace Aplib.Logging;

/// <summary>
/// Defines an agent that can play a game.
/// Exposes the agent's desire set for logging purposes.
/// </summary>
public interface ILoggableAgent<TBeliefset> : ICompletable
    where TBeliefset : IBeliefSet
{
    /// <summary>
    /// Updates the agent's state and goals.
    /// </summary>
    /// <remarks>This method will get called every frame of the game.</remarks>
    public void Update();

    public DesireSet<TBeliefset> DesireSet { get; }
}
