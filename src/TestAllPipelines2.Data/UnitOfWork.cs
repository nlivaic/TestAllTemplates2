using System.Threading.Tasks;
using TestAllPipelines2.Common.Interfaces;

namespace TestAllPipelines2.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestAllPipelines2DbContext _dbContext;

        public UnitOfWork(TestAllPipelines2DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveAsync()
        {
            if (_dbContext.ChangeTracker.HasChanges())
            {
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}