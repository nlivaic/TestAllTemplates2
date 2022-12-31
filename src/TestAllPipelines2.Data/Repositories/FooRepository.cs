using TestAllPipelines2.Core.Entities;
using TestAllPipelines2.Core.Interfaces;

namespace TestAllPipelines2.Data.Repositories
{
    public class FooRepository : Repository<Foo>, IFooRepository
    {
        public FooRepository(TestAllPipelines2DbContext context)
            : base(context)
        {
        }
    }
}
