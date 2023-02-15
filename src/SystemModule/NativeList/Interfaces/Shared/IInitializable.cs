using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Opposite to Disposable objects - objects that require an initialization to work.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Main method after constructor to invoke.
        /// Make possible using of this object.
        /// </summary>
        void Initialize();
    }

    /// <summary>
    /// Opposite to Disposable objects - objects that require an initialization to work.
    /// </summary>
    /// <remarks>
    /// Has an argumented initializer.
    /// </remarks>
    public interface IInitializable<TArgument>
    {
        /// <summary>
        /// Main method after constructor to invoke.
        /// Make possible using of this object.
        /// </summary>
        void Initialize(TArgument argument);
    }
}
