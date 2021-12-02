using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FM21.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddAll(IEnumerable<T> entityList);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, dynamic>>[] includeExpressions);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(bool asNoTracking);
        IEnumerable<T> GetAll(bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where, bool asNoTracking);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where, bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions);
        int Count();
        int Count(Expression<Func<T, bool>> where);
        bool Any(Expression<Func<T, bool>> where);
        bool Any();
        IQueryable<T> Query();
        IQueryable<T> Query(bool asNoTracking);
        IQueryable<T> Query(bool asNoTracking, params Expression<Func<T, dynamic>>[] includeExpressions);
        void AddAsync(T entity);
        Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entityList);
        void UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<int> DeleteAsync(Expression<Func<T, bool>> where);
        Task<T> GetByIdAsync(long id);
        Task<T> GetByIdAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        Task<T> GetByIdAsync(string id);
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> GetAllAsync(bool asNoTracking);
        Task<ICollection<T>> GetManyAsync(Expression<Func<T, bool>> where);
        Task<ICollection<T>> GetManyAsync(Expression<Func<T, bool>> where, bool asNoTracking);
        Task<int> CountAsync(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>> where);
        DataTable GetDataFromQuery(string strQuery);
        DataTable GetFromStoredProcedure(string procedureName, params (string, object)[] nameValueParams);
        Task<DataTable> GetFromStoredProcedureAsync(string procedureName, params (string, object)[] nameValueParams);
    }
}