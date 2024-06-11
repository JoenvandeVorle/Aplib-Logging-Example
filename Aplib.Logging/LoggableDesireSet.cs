using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Collections;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.GoalStructures;
using System.Reflection;

namespace Aplib.Logging;

public class LoggableDesireSet<TBeliefSet> : DesireSet<TBeliefSet>
    where TBeliefSet : IBeliefSet
{
    public LoggableDesireSet(
        IMetadata metadata,
        IGoalStructure<TBeliefSet> mainGoal,
        params (IGoalStructure<TBeliefSet> goalStructure, Func<TBeliefSet, bool> guard)[] sideGoals
    ) : base(metadata, mainGoal, sideGoals)
    { }

    /// <summary>
    /// Try to get the GoalStructureStack of the DesireSet.
    /// </summary>
    public OptimizedActivationStack<(IGoalStructure<TBeliefSet> goalStructure, Func<TBeliefSet, bool> guard)>? GoalStack
    {
        get
        {
            // Use reflection to get the private field _goalStructureStack.
            var field = typeof(DesireSet<TBeliefSet>).GetField("_goalStructureStack", BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(this) as OptimizedActivationStack<(IGoalStructure<TBeliefSet> goalStructure, Func<TBeliefSet, bool> guard)>;
        }
    }
}
