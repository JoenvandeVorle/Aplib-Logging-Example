using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplib.Logging.AplibChanges
{
    public class LoggableGoal<TBeliefSet> : Goal<TBeliefSet>, ILoggable
        where TBeliefSet : IBeliefSet
    {
        public LoggableGoal(ITactic<TBeliefSet> tactic, HeuristicFunction heuristicFunction, double epsilon = 0.005) : base(tactic, heuristicFunction, epsilon)
        {
        }

        public LoggableGoal(ITactic<TBeliefSet> tactic, Func<TBeliefSet, bool> predicate, double epsilon = 0.005) : base(tactic, predicate, epsilon)
        {
        }

        public LoggableGoal(IMetadata metadata, ITactic<TBeliefSet> tactic, HeuristicFunction heuristicFunction, double epsilon = 0.005) : base(metadata, tactic, heuristicFunction, epsilon)
        {
        }

        public LoggableGoal(IMetadata metadata, ITactic<TBeliefSet> tactic, Func<TBeliefSet, bool> predicate, double epsilon = 0.005) : base(metadata, tactic, predicate, epsilon)
        {
        }

        /// <summary>
        /// Generates a log tree of the goal.
        /// </summary>
        /// <param name="depth">The depth of the tree, impacts the indentation level</param>
        public string GetLogTree(int depth)
        {
            ITactic<TBeliefSet> tactic = Tactic;
            // IAction<TBeliefSet>? action = tactic.GetAction(BeliefSet); TODO:: something about actions

            StringBuilder tree = new StringBuilder();
            tree.Append($"Goal: {Metadata.Name} \n {new string('-', (depth+1) * 4) + " "}Tactic: {Tactic.GetMetadata().Name}");
            tree.Insert(0, new string('-', depth * 4) + " ");

            return tree.ToString();
        }
    }
}