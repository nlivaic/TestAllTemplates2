using System;
using System.Reflection;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SparkRoseDigital.Infrastructure.Caching;
using SparkRoseDigital.Infrastructure.Logging;
using TestAllPipelines2.Common.MessageBroker;
using TestAllPipelines2.Common.MessageBroker.Middlewares.ErrorLogging;
using TestAllPipelines2.Common.MessageBroker.Middlewares.Tracing;
using TestAllPipelines2.Core;
using TestAllPipelines2.Core.Events;
using TestAllPipelines2.Data;
using TestAllPipelines2.WorkerServices.FaultService;
using TestAllPipelines2.WorkerServices.FooService;

namespace TestAllPipelines2.WorkerServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SparkRoseDigital.Infrastructure.Logging.LoggerExtensions.ConfigureSerilogLogger("DOTNET_ENVIRONMENT");

            try
            {
                Log.Information("Starting up TestAllPipelines2 Worker Services.");
                CreateHostBuilder(args)
                    .Build()
                    .AddW3CTraceContextActivityLogging()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "TestAllPipelines2 Worker Services failed at startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var hostEnvironment = hostContext.HostingEnvironment;
                    services.AddDbContext<TestAllPipelines2DbContext>(options =>
                    {
                        var connString = new SqlConnectionStringBuilder(configuration.GetConnectionString("TestAllPipelines2DbConnection"))
                        {
                            UserID = configuration["DB_USER"],
                            Password = configuration["DB_PASSWORD"]
                        };
                        options.UseSqlServer(connString.ConnectionString);
                        if (hostEnvironment.IsDevelopment())
                        {
                            options.EnableSensitiveDataLogging(true);
                        }
                    });
                    services.AddGenericRepository();
                    services.AddSpecificRepositories();
                    services.AddCoreServices();
                    services.AddSingleton<ICache, Cache>();
                    services.AddMemoryCache();
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(Program).Assembly);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<FooConsumer>();
                        x.AddConsumer<FooFaultConsumer>();
                        x.AddConsumer<FaultConsumer>();
                        x.AddConsumer<FooCommandConsumer>(typeof(FooCommandConsumer.FooCommandConsumerDefinition));
                        x.UsingAzureServiceBus((ctx, cfg) =>
                        {
                            cfg.Host(
                                new MessageBrokerConnectionStringBuilder(
                                    configuration.GetConnectionString("MessageBroker"),
                                    configuration["MessageBroker:Reader:SharedAccessKeyName"],
                                    configuration["MessageBroker:Reader:SharedAccessKey"]).ConnectionString);

                            // Use the below line if you are not going with
                            // SetKebabCaseEndpointNameFormatter() in the publishing project (see API project),
                            // but have rather given the topic a custom name.
                            // cfg.Message<VoteCast>(configTopology => configTopology.SetEntityName("foo-topic"));
                            cfg.SubscriptionEndpoint<IFooEvent>("foo-event-subscription-1", e =>
                            {
                                e.ConfigureConsumer<FooConsumer>(ctx);
                            });

                            // This is here only for show. I have not thought through a proper
                            // error handling strategy.
                            cfg.SubscriptionEndpoint<Fault<IFooEvent>>("foo-event-fault-consumer", e =>
                            {
                                e.ConfigureConsumer<FooFaultConsumer>(ctx);
                            });

                            // This is here only for show. I have not thought through a proper
                            // error handling strategy.
                            cfg.SubscriptionEndpoint<Fault>("fault-consumer", e =>
                            {
                                e.ConfigureConsumer<FaultConsumer>(ctx);
                            });
                            cfg.ConfigureEndpoints(ctx);

                            cfg.UseMessageBrokerTracing();
                            cfg.UseExceptionLogger(services);
                        });
                        x.AddEntityFrameworkOutbox<TestAllPipelines2DbContext>(o =>
                        {
                            // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                            o.UseSqlServer();

                            // enable the bus outbox
                            o.UseBusOutbox();
                        });
                    });
                });
    }
}
