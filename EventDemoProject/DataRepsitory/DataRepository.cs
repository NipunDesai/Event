using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventDemoProject.DataRepsitory
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        #region "Private Member(s)"

        private DbContext _context;
        private DbSet<T> _dbSet;
        private bool _isDisposed;
        private dynamic _transaction;  // create new transaction instance.

        #endregion

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="context"></param>
        public DataRepository(DbContext context)
        {
            try
            {
                this._context = context;
                this._dbSet = _context.Set<T>();
                this._isDisposed = false;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        #region "Public properties"

        /// <summary>
        /// Property fetches the total Count from the dbset.
        /// </summary>
        public int TotalCount
        {
            get { return _dbSet.Count(); }
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// Method add the entity into the context.
        /// </summary>
        /// <param name="entity"></param>
        public T Add(T entity)
        {
            try
            {
                // start new sql transaction for database operation.
                _transaction = _context.Database.BeginTransaction();
                var newEntity = _dbSet.Add(entity);
                return newEntity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method attaches the entity from the context
        /// </summary>
        /// <param name="entity"></param>
        public void Attach(T entity)
        {
            try
            {
                _dbSet.Attach(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method call when explicitly updating the enteries.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            try
            {
                // start new sql transaction for database operation.
                _transaction = _context.Database.BeginTransaction();
                var entry = _context.Entry(entity);
                _dbSet.Attach(entity);
                entry.State = EntityState.Modified;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the IQueryable based on the filter,orderby and properties to inculde.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IQueryable<T> Fetch(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (!String.IsNullOrWhiteSpace(includeProperties))
                {
                    query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(
                        query, (current, includeProperty) => current.Include(includeProperty));
                }

                return orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the IQueryable based on expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _dbSet.Where(predicate).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the IQueryable based on filter,size and index.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="total"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IQueryable<T> Fetch(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            total = 0;
            try
            {
                var skipCount = index * size;
                var resetSet = filter != null ? _dbSet.Where(filter).AsQueryable() : _dbSet.AsQueryable();
                resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
                total = resetSet.Count();
                return resetSet.AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the entity based on the keys supplied.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T Find(params object[] keys)
        {
            try
            {
                return _dbSet.Find(keys);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(object id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the first or default item from the datacontext based on the the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _dbSet.FirstOrDefault(predicate);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method fetches the first record based on the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T First(Func<T, bool> predicate)
        {
            try
            {
                return _dbSet.First<T>(predicate);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method Fetches the particular single record based on the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Single(Func<T, bool> predicate)
        {
            try
            {
                return _dbSet.Single<T>(predicate);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method Fetches all the data before executing query.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            try
            {
                return _dbSet.AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method Checks whether dbset has anything entity in it or not.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _dbSet.Any(predicate);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method save the changes into the context
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                //commit transaction on successful save operation
                if (_transaction != null)
                    _transaction.Commit();
            }
            catch (Exception ex)
            {
               
                //Rollback all transaction if any exception occures during transaction.
                if (_transaction != null)
                    _transaction.Rollback();
              
            }
        }

        /// <summary>
        /// Method deletes the entity from the datacontext by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            try
            {
                _transaction = _context.Database.BeginTransaction();
                var entityToDelete = _dbSet.Find(id);
                if (entityToDelete != null)
                    _dbSet.Remove(entityToDelete);
                    //Delete(entityToDelete);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method deletes the entity from the datacontext.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            try
            {
                // start new sql transaction for database operation.
                _transaction = _context.Database.BeginTransaction();
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method deletes the entity based on the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                // start new sql transaction for database operation.
                _transaction = _context.Database.BeginTransaction();
                var entitiesToDelete = Fetch(predicate);
                foreach (var entity in entitiesToDelete)
                {
                    if (_context.Entry(entity).State == EntityState.Detached)
                    {
                        _dbSet.Attach(entity);
                    }
                    _dbSet.Remove(entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method call on dispose calls.
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method Disposes the Context.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            try
            {
                if (!_isDisposed)
                {
                    if (disposing)
                    {
                        if (_context != null)
                        {
                            _context.Dispose();
                        }
                        _isDisposed = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Public Destructor.
        /// </summary>
        ~DataRepository()
        {
            try
            {
                Dispose(false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
