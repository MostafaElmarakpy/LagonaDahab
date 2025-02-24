using System;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IGenaricRepository<T> Repository<T>() where T : class;
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }

        Task<int> SaveAsync();
        int Save();
    }
}