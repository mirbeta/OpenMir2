using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// This interface was created to make possible to see that the object was initialized.
    /// </summary>
    public interface IInitializeIndication
    {
        /// <summary>
        /// Shows that the object was initialized or not.
        /// I do not recommend to touch object if it wasn't initialized.
        /// </summary>
        bool IsInitialized { get; }
    }
}
