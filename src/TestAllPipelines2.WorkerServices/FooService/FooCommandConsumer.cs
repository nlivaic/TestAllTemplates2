﻿using System;
using System.Threading.Tasks;
using MassTransit;
using TestAllPipelines2.Core.Events;
using TestAllPipelines2.Data;

namespace TestAllPipelines2.WorkerServices.FooService
{
    public class FooCommandConsumer : IConsumer<IFooCommand>
    {
        public Task Consume(ConsumeContext<IFooCommand> context) =>
            Task.CompletedTask;

        public class FooCommandConsumerDefinition : ConsumerDefinition<FooCommandConsumer>
        {
            private readonly IServiceProvider _provider;

            public FooCommandConsumerDefinition(IServiceProvider provider)
            {
                EndpointName = $"{WorkerAssemblyInfo.ServiceName.ToLower()}-foo-command-queue";
                _provider = provider;
            }

            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FooCommandConsumer> consumerConfigurator)
            {
                // Configure message retry with millisecond intervals.
                // endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
                // Creates only a queue, without topic and subscription.
                // endpointConfigurator.ConfigureConsumeTopology = false;

                // Use the outbox to prevent duplicate events from being published.
                endpointConfigurator.UseEntityFrameworkOutbox<TestAllPipelines2DbContext>(_provider);
            }
        }
    }
}
