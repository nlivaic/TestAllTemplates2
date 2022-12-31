using System.Threading.Tasks;
using MassTransit;
using TestAllPipelines2.Core.Events;

namespace TestAllPipelines2.WorkerServices.FooService
{
    public class FooConsumer : IConsumer<IFooEvent>
    {
        public Task Consume(ConsumeContext<IFooEvent> context) =>
            Task.CompletedTask;
    }
}
