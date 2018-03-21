using Microsoft.Extensions.DependencyInjection;

namespace RestHelpers
{
    /// <summary>
    ///     Extensions for builder.
    /// </summary>
    public static class RestHelpersBuilderExtensions
    {
        /// <summary>
        ///     RestHelper default service definition for DI.
        /// </summary>
        /// <param name="services">Service collection for DI.</param>
        /// <returns>RestHelper default service collection for DI.</returns>
        public static IServiceCollection AddRestHelpersServices(this IServiceCollection services)
        {
            services.AddTransient<ILinkService, LinkService>();

            return services;
        }
    }
}