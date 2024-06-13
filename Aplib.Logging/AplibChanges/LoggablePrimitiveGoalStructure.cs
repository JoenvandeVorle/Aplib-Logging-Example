using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using System.Text;

namespace Aplib.Logging.AplibChanges
{
    public class LoggablePrimitiveGoalStructure<TBeliefSet> : LoggableGoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        private readonly LoggableGoal<TBeliefSet> _goal;

        public LoggablePrimitiveGoalStructure(IMetadata metadata, LoggableGoal<TBeliefSet> goal)
            : base(metadata, Array.Empty<LoggableGoalStructure<TBeliefSet>>()) => _goal = goal;

        public LoggablePrimitiveGoalStructure(LoggableGoal<TBeliefSet> goal) : this(new Metadata(), goal) { }

        public override LoggableGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet) => _goal;

        public override string GetLogTree(int depth)
        {
            StringBuilder tree = new StringBuilder();
            tree.Append($"Primitive GoalStructure --{Metadata.Name}-- \n {_goal.GetLogTree(depth + 1)}");

            return tree.ToString();
        }

        public override void UpdateStatus(TBeliefSet beliefSet) => 
            Status = _goal.GetStatus(beliefSet);
    }
}