using MyApp.Domain.Interfaces.Repositories;
using MyApp.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyApp.Infra.Data.Repositories
{
    /// <summary>
    /// The intention is to have a single, generic class to perform CRUD, 
    /// where you can pass a T entity to it, and that class will work on top of that entity. 
    /// The IRepositoryBase interface is inherited, where the class is obliged to implement the methods 
    /// previously defined in the domain layer.
    /// </summary>
    /// <typeparam name="TEntity">Class Entity</typeparam>
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected MyAppContext Db = new MyAppContext();

        public void Add(TEntity obj)
        {
            Db.Set<TEntity>().Add(obj);
            Db.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Db.Set<TEntity>().ToList();
        }

        public TEntity GetById(int Id)
        {
            return Db.Set<TEntity>().Find(Id);
        }

        public void Remove(TEntity obj)
        {
            Db.Set<TEntity>().Remove(obj);
            Db.SaveChanges();
        }

        public void Update(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
        }
    }
}
