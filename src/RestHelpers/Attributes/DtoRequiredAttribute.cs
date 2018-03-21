using System;
using System.Collections.Generic;
using System.Text;

namespace RestHelpers.Attributes
{
    /// <summary>
    ///     Property marked with this attribute will be always part of returned DTO model. Even if
    ///     doesn't present in a filtered field.
    /// </summary>
    public class DtoRequiredAttribute : Attribute
    {
    }
}