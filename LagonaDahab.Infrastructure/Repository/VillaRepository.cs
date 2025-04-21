using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Infrastructure.Repository
{
    public class VillaRepository : GenaricRepository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaRepository(ApplicationDbContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Villa entity)
        {
            _dbContext.Villas.Update(entity);
        }
    }
}