using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using LagonaDahab.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Infrastructure.Repository
{
    public class UserRepository : GenaricRepository<ApplicationUser>, IUserRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
