using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Aplib.Core.Desire;

namespace Aplib.Logging;

public static class MetadataExtensionMethods
{
    public static IMetadata GetMetadata<TBeliefSet>(this IDesireSet<TBeliefSet> desireSet) where TBeliefSet : IBeliefSet
        => (desireSet as IDocumented)?.Metadata ?? new Metadata("No metadata available");

    public static IMetadata GetMetadata<TBeliefSet>(this IGoalStructure<TBeliefSet> goalStructure) where TBeliefSet : IBeliefSet
        => (goalStructure as IDocumented)?.Metadata ?? new Metadata("No metadata available");

    public static IMetadata GetMetadata<TBeliefSet>(this IGoal<TBeliefSet> goal) where TBeliefSet : IBeliefSet 
        => (goal as IDocumented)?.Metadata ?? new Metadata("No metadata available");

    public static IMetadata GetMetadata<TBeliefSet>(this ITactic<TBeliefSet> tactic) where TBeliefSet : IBeliefSet
        => (tactic as IDocumented)?.Metadata ?? new Metadata("No metadata available");

    /// <summary>
    /// Gets the metadata of the action.
    /// </summary>
    /// <param name="action">The (nullable) action.</param>
    /// <param name="message">The message to display if the action is null.</param>
    public static IMetadata GetMetadata<TBeliefSet>(this IAction<TBeliefSet>? action, string message = "no action enabled") where TBeliefSet : IBeliefSet  
    {
        if (action == null) return new Metadata(message);
        return (action as IDocumented)?.Metadata ?? new Metadata("No metadata available");
    }

    public static TBeliefSet GetBeliefSet<TBeliefSet>(this IBeliefSet beliefSet) where TBeliefSet : IBeliefSet
        => (TBeliefSet)beliefSet;
}