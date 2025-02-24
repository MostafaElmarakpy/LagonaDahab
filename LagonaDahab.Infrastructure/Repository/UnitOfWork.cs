using System;
using System.Threading.Tasks;
using LagonaDahab.Application.Common.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IVillaRepository _villaRepository;
    private IVillaNumberRepository _villaNumberRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenaricRepository<T> Repository<T>() where T : class
    {
        // Implementation for generic repository
    }

    public IVillaRepository Villa => _villaRepository ??= new VillaRepository(_context);
    public IVillaNumberRepository VillaNumber => _villaNumberRepository ??= new VillaNumberRepository(_context);

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public int Save()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
