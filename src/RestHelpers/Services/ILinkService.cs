using RestHelpers.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RestHelpers
{
    /// <summary>
    ///     Interface for HATEOAS links management service.
    /// </summary>
    public interface ILinkService<TResourceParameters>
        where TResourceParameters : BaseResourceParametersFormultipleReturns
    {
        /// <summary>
        ///     Create HATEOAS friendly link.
        /// </summary>
        /// <param name="routeName">Route to controller which returns single object result.</param>
        /// <param name="rel">Rel attribute.</param>
        /// <param name="method">Method attribute.</param>
        /// <returns>HATEOAS friendly link.</returns>
        LinkDto CreateLink(string routeName, string rel, HttpMethod method);
        /// <summary>
        ///     Create HATEOAS friendly links.
        /// </summary>
        /// <param name="routeName">Route to controller which returns single object result.</param>
        /// <param name="values">Link query string values.</param>
        /// <param name="rel">Rel attribute.</param>
        /// <param name="method">Method attribute.</param>
        /// <returns>HATEOAS friendly link.</returns>
        LinkDto CreateLink(string routeName, object values, string rel, HttpMethod method);
        /// <summary>
        ///     This method creates HATEOAS friendly links for paged collection.
        /// </summary>
        /// <param name="baseResourceParameters">Base resource parameters for paged collection.</param>
        /// <param name="routeName">Route to controller which returns specific paged data collection.</param>
        /// <returns></returns>
        IEnumerable<LinkDto> CreatePagedLinks(TResourceParameters baseResourceParameters,
            string routeName);
        /// <summary>
        ///     Create resource uri for specific resource uri type.
        /// </summary>
        /// <param name="baseResourceParameters">Base resource parameters for paged results.</param>
        /// <param name="resourceUriType">Resource uri type.</param>
        /// <param name="routeName">Route name to method which returns multiple paged results.</param>
        /// <returns>Valid URI.</returns>
        string CreateResourceUri(TResourceParameters baseResourceParameters,
            ResourceUriType resourceUriType, string routeName);
    }

    /// <summary>
    ///     Interface with default parameters for HATEOAS links management service.
    /// </summary>
    public interface ILinkService : ILinkService<BaseResourceParametersFormultipleReturns>
    {
    }
}
