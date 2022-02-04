using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StagebeheerAPI.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected StagebeheerDBContext RepositoryContext { get; set; }

        public RepositoryBase(StagebeheerDBContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void CreateRange(List<T> entity)
        {
            this.RepositoryContext.Set<T>().AddRange(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(List<T> entity)
        {
            this.RepositoryContext.Set<T>().RemoveRange(entity);
        }
    }
}
