using LagonaDahab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IVillaRepository : IGenaricRepository<Villa>
    {
 
        void Update(Villa entity);
        void SaveChanges();


    }
}
