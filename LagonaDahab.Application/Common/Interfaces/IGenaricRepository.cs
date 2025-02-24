using LagonaDahab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IGenaricRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperty = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperty = null);
        bool Any(Expression<Func<T, bool>> filter);
        void Add(T entity);     
        void Remove(T entity);

    }
}
