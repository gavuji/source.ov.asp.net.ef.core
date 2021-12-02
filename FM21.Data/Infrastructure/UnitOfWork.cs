using System.Threading.Tasks;

namespace FM21.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Variable
        private readonly IDatabaseFactory databaseFactory;

        private AppEntities dataContext;
        #endregion

        #region Constructor
        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }
        #endregion

        #region Property
        protected AppEntities DataContext => dataContext ?? (dataContext = databaseFactory.Get());
        #endregion

        #region Methods
        public void Commit()
        {
            DataContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await DataContext.SaveChangesAsync();
        }
        #endregion
    }
}