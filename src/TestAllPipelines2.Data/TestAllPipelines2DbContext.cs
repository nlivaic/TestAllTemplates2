using MassTransit;
using Microsoft.EntityFrameworkCore;
using TestAllPipelines2.Core.Entities;

namespace TestAllPipelines2.Data
{
    public class TestAllPipelines2DbContext : DbContext
    {
        public TestAllPipelines2DbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Foo> Foos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
