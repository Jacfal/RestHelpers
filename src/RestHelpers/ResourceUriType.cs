using System;
using System.Collections.Generic;
using System.Text;

namespace RestHelpers
{
    /// <summary>
    ///     Valid page resource uri types.
    /// </summary>
    public enum ResourceUriType
    {
        /// <summary>
        ///     Previous page.
        /// </summary>
        PreviousPage,
        /// <summary>
        ///     Next page.
        /// </summary>
        NextPage,
        /// <summary>
        ///     Current.
        /// </summary>
        Current
    }
}