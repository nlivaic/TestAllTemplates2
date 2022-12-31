using Microsoft.Extensions.DependencyInjection;
using TestAllPipelines2.Common.Interfaces;
using TestAllPipelines2.Core.Interfaces;
using TestAllPipelines2.Data.Repositories;

namespace TestAllPipelines2.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSpecificRepositories(this IServiceCollection services) =>
            services.AddScoped<IFooRepository, FooRepository>();

        public static void AddGenericRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
