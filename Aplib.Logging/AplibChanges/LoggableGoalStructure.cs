using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.GoalStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplib.Logging.AplibChanges
{
    public abstract class LoggableGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>, ILoggable
        where TBeliefSet : IBeliefSet
    {
        protected new IEnumerable<LoggableGoalStructure<TBeliefSet>> _children;

        protected new LoggableGoalStructure<TBeliefSet>? _currentGoalStructure;

        protected LoggableGoalStructure(IEnumerable<IGoalStructure<TBeliefSet>> children) : base(children)
        {
            _children = children.Cast<LoggableGoalStructure<TBeliefSet>>();
        }

        protected LoggableGoalStructure(IMetadata metadata, IEnumerable<IGoalStructure<TBeliefSet>> children) : base(metadata, children)
        {
            _children = children.Cast<LoggableGoalStructure<TBeliefSet>>();
        }

        /// <summary>
        /// Generates a log tree of the goal structure.
        /// </summary>
        /// <param name="depth">The depth of the tree, impacts the indentation level</param>
        public abstract string GetLogTree(int depth);
    }
}