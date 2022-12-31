using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestAllPipelines2.Application.Pipelines;

namespace TestAllPipelines2.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTestAllPipelines2ApplicationHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
            services.AddPipelines();

            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        }
    }
}
