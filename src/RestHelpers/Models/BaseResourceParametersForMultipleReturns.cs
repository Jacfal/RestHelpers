using System;
using System.Collections.Generic;
using System.Text;

namespace RestHelpers.Models
{
    /// <summary>
    ///     Base resource parameters for communication with Res.Tapecenter REST interface. Paged collection on return is
    ///     supposed.
    /// </summary>
    public abstract class BaseResourceParametersFormultipleReturns
    {
        /// <summary>
        ///     Page number.
        ///     
        ///     Example: page=5
        /// </summary>
        public virtual int Page { get; set; } = 1;
        /// <summary>
        ///     A number of records presented on the actual page.  
        ///     Default value is set to 500 records.
        ///     
        ///     Example: pageSize=20
        /// </summary>
        public virtual int PageSize { get; set; } = 500;
        /// <summary>
        ///     Sort the result set in ascending or descending order. To sort the records in descending order,
        ///     use the "desc" keyword. Multiple sorting is supported when each field is separated by the proper delimiter.
        ///     Default delimiter is set to ','.
        ///     
        ///     Example: orderBy=field1 desc, field2 (field1 column will be ordered descending, field2 column will be ordered ascending).
        /// </summary>
        public virtual string OrderBy { get; set; }
        /// <summary>
        ///     Select which fields will be returned in the data transfer object model. All possible fields are returned
        ///     in the data transfer object model by default. Multiple fields selection is supported with using a proper
        ///     delimiter. The default delimiter is set to ','.
        ///     
        ///     Example: select=field1, field2, field3
        /// </summary>
        public virtual string Select { get; set; }
    }
}
