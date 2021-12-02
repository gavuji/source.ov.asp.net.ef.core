using System.Threading.Tasks;

namespace FM21.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        Task<int> CommitAsync();
    }
}