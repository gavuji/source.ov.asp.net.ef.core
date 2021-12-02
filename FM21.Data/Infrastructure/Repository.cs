using FM21.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FM21.Data.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Variable / Properties  
        private AppEntities dataContext;
        private readonly DbSet<T> dbSet;
        protected AppEntities DataContext => dataContext ?? (dataContext = DatabaseFactory.Get());

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }
        #endregion

        #region Sync Methods
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<T> entityList)
        {
            DataContext.Set<T>().AddRange(entityList);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                dbSet.Remove(obj);
            }
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return Get(where, null);
        }

        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, dynamic>>[] includeExpressions)
        {
            if (includeExpressions != null)
            {
                IQueryable<T> set = dbSet;

                foreach (var includeExpression in includeExpressions)
                {
                    set = set.Include(includeExpression);
                }
                return set.Where(where).FirstOrDefault<T>();
            }
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public IEnumerable<T> GetAll(bool asNoTracking)
        {
            return GetAll(asNoTracking, null);
        }

        public IEnumerable<T> GetAll(bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions)
        {
            IQueryable<T> set = dbSet;
            if (includeExpressions != null)
            {
                foreach (var includeExpression in includeExpressions)
                {
                    set = set.Include(includeExpression);
                }
            }
            return asNoTracking ? set.AsNoTracking().ToList() : set.ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return GetAll(false, null);
        }

        public T GetById(long id)
        {
            return dbSet.Find(id);
        }

        public T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where, bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions)
        {
            IQueryable<T> set = dbSet;
            if (includeExpressions != null)
            {
                foreach (var includeExpression in includeExpressions)
                {
                    set = set.Include(includeExpression);
                }
            }
            return asNoTracking ? set.Where(where).AsNoTracking().ToList() : set.Where(where).ToList();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where, bool asNoTracking)
        {
            return GetMany(where, asNoTracking, null);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GetMany(where, false);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public int Count()
        {
            return dbSet.AsNoTracking().Count();
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).AsNoTracking().Count();
        }

        public bool Any(Expression<Func<T, bool>> @where)
        {
            return dbSet.Where(where).AsNoTracking().Any();
        }

        public bool Any()
        {
            return dbSet.AsNoTracking().Any();
        }

        public IQueryable<T> Query()
        {
            return Query(false);
        }

        public IQueryable<T> Query(bool asNoTracking)
        {
            return Query(asNoTracking, null);
        }

        public IQueryable<T> Query(bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions)
        {
            //dataContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            IQueryable<T> set = dbSet;
            if (includeExpressions != null)
            {
                foreach (var includeExpression in includeExpressions)
                {
                    set = set.Include(includeExpression);
                }
            }
            return asNoTracking ? set.AsNoTracking().AsQueryable() : set.AsQueryable();
        }
        #endregion       

        #region Async Methods
        public void AddAsync(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<int> DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            return await dataContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                dbSet.Remove(obj);
            }
            return await dataContext.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<ICollection<T>> GetAllAsync(bool asNoTracking)
        {
            return await GetAllAsync(asNoTracking, null);
        }

        public async Task<ICollection<T>> GetAllAsync(bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions)
        {
            IQueryable<T> set = dbSet;
            if (includeExpressions != null)
            {
                foreach (var includeExpression in includeExpressions)
                {
                    set = set.Include(includeExpression);
                }
            }
            var result = asNoTracking ? set.AsNoTracking().ToListAsync() : set.ToListAsync();
            return await result;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await DataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await DataContext.Set<T>().FindAsync(id);
        }

        public async Task<ICollection<T>> GetManyAsync(Expression<Func<T, bool>> where, bool asNoTracking)
        {
            if (asNoTracking)
            {
                return await dbSet.AsNoTracking().Where(where).ToListAsync();
            }
            return await dbSet.Where(where).ToListAsync();
        }

        public async Task<ICollection<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await GetManyAsync(where, false);
        }

        public void UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public Task<int> CountAsync(T entity)
        {
            return dbSet.AsNoTracking().CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return dbSet.AsNoTracking().Where(where).CountAsync();
        }

        public async Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entityList)
        {
            DataContext.Set<T>().AddRange(entityList);
            await dataContext.SaveChangesAsync();
            return entityList;
        }
        #endregion

        #region Store Procedures
        public DataTable GetDataFromQuery(string strQuery)
        {
            var cmd = DataContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = strQuery;
            return GetDataUsingDBCommand(cmd);
        }

        /// <param name="name">Pass SQL stored procedure name</param>
        /// <param name="nameValueParams">If SP do not have parameter then pass null value</param>
        public DataTable GetFromStoredProcedure(string procedureName, params (string, object)[] nameValueParams)
        {
            var cmd = GetDBCommand(procedureName, nameValueParams);
            return GetDataUsingDBCommand(cmd);
        }

        /// <param name="name">Pass SQL stored procedure name</param>
        /// <param name="nameValueParams">If SP do not have parameter then pass null value</param>
        public async Task<DataTable> GetFromStoredProcedureAsync(string procedureName, params (string, object)[] nameValueParams)
        {
            var cmd = GetDBCommand(procedureName, nameValueParams);
            return await GetDataUsingDBCommandAsync(cmd);
        }

        private DbCommand GetDBCommand(string procedureName, params (string, object)[] nameValueParams)
        {
            var cmd = DataContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = ApplicationConstants.SQLServerTimeOut;

            if (nameValueParams != null)
            {
                foreach (var pair in nameValueParams)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = pair.Item1;
                    param.Value = pair.Item2 ?? DBNull.Value;
                    cmd.Parameters.Add(param);
                }
            }
            return cmd;
        }

        private DataTable GetDataUsingDBCommand(DbCommand cmd)
        {
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        private async Task<DataTable> GetDataUsingDBCommandAsync(DbCommand cmd)
        {
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                try
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }
        #endregion
    }
}