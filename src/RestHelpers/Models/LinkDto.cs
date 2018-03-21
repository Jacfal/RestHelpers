using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RestHelpers.Models
{
    /// <summary>
    ///     HATEOAS link.
    /// </summary>
    public class LinkDto
    {
        /// <summary>
        ///     HATEOAS link.
        /// </summary>
        /// <param name="href">Href attribute.</param>
        /// <param name="rel">Rel attribute.</param>
        /// <param name="method">HTTP method attribute.</param>
        public LinkDto(string href, string rel, HttpMethod method)
        {
            Href = href;
            Rel = rel;
            Method = method.ToString();
        }

        /// <summary>
        ///     HATEOAS link.
        /// </summary>
        /// <param name="href">Href attribute.</param>
        /// <param name="rel">Rel attribute.</param>
        /// <param name="method">HTTP method attribute.</param>
        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method.ToString();
        }

        /// <summary>
        ///     Href attribute.
        /// </summary>
        public string Href { get; private set; }
        /// <summary>
        ///     Rel attribute.
        /// </summary>
        public string Rel { get; private set; }
        /// <summary>
        ///     Method attribute.
        /// </summary>
        public string Method { get; private set; }
    }
}
