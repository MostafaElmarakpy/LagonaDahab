using System;
using System.Collections;
using System.Threading.Tasks;
using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Infrastructure.Data;
using LagonaDahab.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IVillaRepository _villaRepository;
    private IVillaNumberRepository _villaNumberRepository;
    private IAmenityRepository _amenityRepository;
    private IBookingRepository _bookingRepository;
    //private IGenaricRepository<TEntity> _genaricRepository;

    private Hashtable _Repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public IVillaRepository Villa => _villaRepository ??= new VillaRepository(_dbContext);
    public IVillaNumberRepository VillaNumber => _villaNumberRepository ??= new VillaNumberRepository(_dbContext);
    public IAmenityRepository Amenity => _amenityRepository ??= new AmenityRepository(_dbContext);
    public IBookingRepository Booking => _bookingRepository ??= new BookingRepository(_dbContext);

    //public async Task<int> SaveAsync()
    //{
    //    return await _context.SaveChangesAsync();
    //}

    public void Save()
    {
         _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_Repositories == null)
            _Repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_Repositories.ContainsKey(type))
        {
            var Repository = new GenaricRepository<TEntity>(_dbContext);
            _Repositories.Add(type, Repository);
        }
        return (IGenaricRepository<TEntity>)_Repositories[type];
    }
}
