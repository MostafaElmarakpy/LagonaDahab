using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }
    }
}
