using System;
using System.Collections.Generic;
using System.Text;

namespace RestHelpers.Attributes
{
    /// <summary>
    ///     Property marked with this attribute can be used in request query string "order by" clause.
    /// </summary>
    public class OrderByPropertyAttribute : Attribute
    {
    }
}
