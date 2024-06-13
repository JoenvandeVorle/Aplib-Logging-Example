using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplib.Logging.AplibChanges;

/// <summary>
/// An interface that allows for logging with a logtree
/// </summary>
public interface ILoggable
{
    /// <summary>
    /// Prints a log tree of the object
    /// </summary>
    /// <param name="depth">The depth of the tree, impacts the indentation level</param>
    /// <returns>A string representation of the log tree</returns>
    public string GetLogTree(int depth = 0);
}
