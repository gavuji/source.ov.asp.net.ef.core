using Microsoft.EntityFrameworkCore;

namespace FM21.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private AppEntities dataContext;

        public AppEntities Get()
        {
            return dataContext ?? (dataContext = new AppEntities(new DbContextOptions<AppEntities>()));
        }

        protected override void DisposeCore()
        {
            dataContext?.Dispose();
        }
    }
}