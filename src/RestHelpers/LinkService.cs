using RestHelpers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Dynamic;

namespace RestHelpers
{
    /// <summary>
    ///     HATEOAS links management service.
    /// </summary>
    public class LinkService<TResourceParameters> : ILinkService<TResourceParameters>
        where TResourceParameters : BaseResourceParametersFormultipleReturns
    {
        /// <summary>
        ///     HATEOAS links management service.
        /// </summary>
        /// <param name="urlHelper">URL helper.</param>
        public LinkService(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Uri helper interface.
        /// </summary>
        private IUrlHelper _urlHelper;

        /// <summary>
        ///     Create HATEOAS friendly link.
        /// </summary>
        /// <param name="routeName">Route to controller which returns single object result.</param>
        /// <param name="rel">Rel attribute.</param>
        /// <param name="method">Method attribute.</param>
        /// <returns>HATEOAS friendly link.</returns>
        public LinkDto CreateLink(string routeName, string rel, HttpMethod method)
        {
            return CreateLink(routeName, null, rel, method);
        }

        /// <summary>
        ///     Create HATEOAS friendly link.
        /// </summary>
        /// <param name="routeName">Route to controller which returns single object result.</param>
        /// <param name="values">Link query string values.</param>
        /// <param name="rel">Rel attribute.</param>
        /// <param name="method">Method attribute.</param>
        /// <returns>HATEOAS friendly link.</returns>
        public LinkDto CreateLink(string routeName, object values, string rel, HttpMethod method)
        {
                return new LinkDto(_urlHelper.Link(routeName, values),
                            rel,
                            method);
        }

        /// <summary>
        ///     List of link DTO models.
        /// </summary>
        private List<LinkDto> _links = new List<LinkDto>();

        /// <summary>
        ///     This method creates HATEOAS friendly links for paged collection.
        /// </summary>
        /// <param name="baseResourceParameters">Base resource parameters for paged collection.</param>
        /// <param name="routeName">Route to controller which returns specific paged data collection.</param>
        /// <returns></returns>
        public IEnumerable<LinkDto> CreatePagedLinks(TResourceParameters baseResourceParameters,
            string routeName)
        {
            // current page
            _links.Add(new LinkDto(CreateResourceUri(baseResourceParameters, ResourceUriType.Current, routeName),
                "self",
                HttpMethod.Get));

            // has next page
            if (baseResourceParameters.Page < baseResourceParameters.PageSize)
            {
                _links.Add(new LinkDto(CreateResourceUri(baseResourceParameters, ResourceUriType.NextPage, routeName),
                    "nextPage",
                    HttpMethod.Get));
            }

            // has previous page
            if (baseResourceParameters.Page > 1)
            {
                _links.Add(new LinkDto(CreateResourceUri(baseResourceParameters, ResourceUriType.PreviousPage, routeName),
                    "previousPage",
                    HttpMethod.Get));
            }

            return _links;
        }

        /// <summary>
        ///     Create resource uri for specific resource uri type.
        /// </summary>
        /// <param name="baseResourceParameters">Base resource parameters for paged results.</param>
        /// <param name="resourceUriType">Resource uri type.</param>
        /// <param name="routeName">Route name to method which returns multiple paged results.</param>
        /// <returns>Valid URI.</returns>
        public string CreateResourceUri(TResourceParameters baseResourceParameters, 
            ResourceUriType resourceUriType, string routeName)
        {
            var queryStringValues = baseResourceParameters.ShapeData(resourceUriType, baseResourceParameters.Page);

            return _urlHelper.Link(routeName, queryStringValues);
        }
    }

    /// <summary>
    ///     HATEOAS links management service with default parameters.
    /// </summary>
    public class LinkService : LinkService<BaseResourceParametersFormultipleReturns>, ILinkService
    {
        public LinkService(IUrlHelper urlHelper) : base(urlHelper)
        {
        }
    }
}
