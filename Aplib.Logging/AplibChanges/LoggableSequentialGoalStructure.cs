using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplib.Logging.AplibChanges
{
    public class LoggableSequentialGoalStructure<TBeliefSet> : LoggableGoalStructure<TBeliefSet>, IDisposable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the enumerator for the children of the goal structure.
        /// </summary>
        private readonly IEnumerator<LoggableGoalStructure<TBeliefSet>> _childrenEnumerator;

        public LoggableSequentialGoalStructure(IMetadata metadata, params LoggableGoalStructure<TBeliefSet>[] children) : base(metadata, children)
        {
            if (children.Length <= 0)
                throw new ArgumentException("Collection of children is empty", nameof(children));

            _childrenEnumerator = _children.GetEnumerator();
            _childrenEnumerator.MoveNext();
            _currentGoalStructure = _childrenEnumerator.Current;
        }

        public LoggableSequentialGoalStructure(params LoggableGoalStructure<TBeliefSet>[] children) : this(new Metadata(), children)
        {
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override string GetLogTree(int depth)
        {
            StringBuilder tree = new StringBuilder();
            tree.Append($"Sequential GoalStructure: {Metadata.Name}");
            int count = 1;
            foreach (var child in _children)
            {
                tree.Append($"\n:{new string('-', depth * 4) + " "}Sub {count++}: {child.GetLogTree(depth + 1)}");
            }
            return tree.ToString();
        }

        /// <summary>
        /// Disposes the enumerator.
        /// </summary>
        /// <param name="disposing">Whether the object is being disposed.</param>
        protected virtual void Dispose(bool disposing) => _childrenEnumerator.Dispose();

        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet) 
            => _currentGoalStructure!.GetCurrentGoal(beliefSet);

        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            // Loop through all the children until one of them is unfinished or successful.
            // This loop is here to prevent tail recursion.
            while (true)
            {
                if (Status == CompletionStatus.Success) return;

                _currentGoalStructure!.UpdateStatus(beliefSet);

                switch (_currentGoalStructure.Status)
                {
                    case CompletionStatus.Unfinished:
                        return;
                    case CompletionStatus.Failure:
                        Status = CompletionStatus.Failure;
                        return;
                    case CompletionStatus.Success:
                    default:
                        break;
                }

                if (_childrenEnumerator.MoveNext())
                {
                    _currentGoalStructure = _childrenEnumerator.Current;
                    Status = CompletionStatus.Unfinished;

                    // Update the state of the new goal structure
                    continue;
                }

                Status = CompletionStatus.Success;
                return;
            }
        }
    }
}