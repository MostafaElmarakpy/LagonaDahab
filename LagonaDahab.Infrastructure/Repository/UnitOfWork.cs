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

    public IVillaRepository Villa { get; private set; }
    public IVillaNumberRepository VillaNumber { get; private set; }
    public IAmenityRepository Amenity { get; private set; }
    public IUserRepository User { get; private set; }
    public IBookingRepository Booking { get; private set; }
    




    private Hashtable _Repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _dbContext = context;
        Villa = new VillaRepository(_dbContext);
        VillaNumber = new VillaNumberRepository(_dbContext);
        Amenity = new AmenityRepository(_dbContext);
        User = new UserRepository(_dbContext);
        Booking = new BookingRepository(_dbContext);
    }

    public void Save()
    {
         _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    // 
    //public IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : class
    //{
    //    if (_Repositories == null)
    //        _Repositories = new Hashtable();

    //    var type = typeof(TEntity).Name;

    //    if (!_Repositories.ContainsKey(type))
    //    {
    //        var Repository = new GenaricRepository<TEntity>(_dbContext);
    //        _Repositories.Add(type, Repository);
    //    }
    //    return (IGenaricRepository<TEntity>)_Repositories[type];
    //}
}
