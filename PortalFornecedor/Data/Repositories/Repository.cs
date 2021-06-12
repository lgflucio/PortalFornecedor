using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.Repositories
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
                      where TEntity : class
                      where TContext : DbContext
    {
        protected readonly TContext _context;

        public Repository(TContext context)
        {
            _context = context;
        }
        protected DbSet<TEntity> _dbset => _context.Set<TEntity>();

        public TEntity Create(TEntity model)
        {
            try
            {
                _dbset.Add(model);
                Save();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void CreateRange(List<TEntity> model)
        {
            try
            {
                _dbset.AddRange(model);
                Save();
                ;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool Delete(TEntity model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            try
            {
                if (_context != null)
                    _context.Dispose();
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TEntity Find(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                return _dbset.FirstOrDefault(where);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, object> includes)
        {
            try
            {
                IQueryable<TEntity> _query = _dbset;
                if (includes != null)
                    _query = includes(_query) as IQueryable<TEntity>;

                return _query.Where(predicate).AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void InsertInContext(TEntity entity)
        {
            _context.Add(entity);
        }
        public void InsertListInContext(List<TEntity> entity)
        {
            _context.AddRange(entity);
        }
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                return _dbset.Where(where);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException exc)
            {
                // just to ease debugging
                foreach (var error in exc.EntityValidationErrors)
                {
                    foreach (var errorMsg in error.ValidationErrors)
                    {
                        // logging service based on NLog
                        Console.WriteLine( $"Error trying to save EF changes - {errorMsg.ErrorMessage}");
                    }
                }

                throw;
            }
            catch (DbUpdateException e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (var eve in e.Entries)
                {
                    sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                }

                Console.WriteLine(sb.ToString());

                throw;
            }

        }

        public bool Update(TEntity model)
        {
            throw new NotImplementedException();
        }
    }
}
