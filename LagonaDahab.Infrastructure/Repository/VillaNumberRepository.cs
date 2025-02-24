using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Infrastructure.Repository
{
    public class VillaNumberRepository : GenaricRepository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaNumberRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }


        public void Update(VillaNumber entity)
        {
            _dbContext.VillaNumbers.Update(entity);
        }
    }
}
