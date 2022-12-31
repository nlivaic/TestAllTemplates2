using System.Threading.Tasks;

namespace TestAllPipelines2.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}