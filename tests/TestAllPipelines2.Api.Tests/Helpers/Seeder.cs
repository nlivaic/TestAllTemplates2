using System.Collections.Generic;
using TestAllPipelines2.Core.Entities;
using TestAllPipelines2.Data;

namespace TestAllPipelines2.Api.Tests.Helpers
{
    public static class Seeder
    {
        public static void Seed(this TestAllPipelines2DbContext ctx)
        {
            ctx.Foos.AddRange(
                new List<Foo>
                {
                    new ("Text 1"),
                    new ("Text 2"),
                    new ("Text 3")
                });
            ctx.SaveChanges();
        }
    }
}
