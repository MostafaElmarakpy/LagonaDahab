using LagonaDahab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperty = null );
        IEnumerable<Villa> Get(Expression<Func<Villa, bool>> filter , string? includeProperty = null );

        void Add(Villa entity);
        void Update(Villa entity);
        void Remove(Villa entity);
        void SaveChanges();


    }
}
