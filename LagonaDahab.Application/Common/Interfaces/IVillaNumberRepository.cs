﻿using LagonaDahab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Application.Common.Interfaces
{
    public interface IVillaNumberRepository:IGenaricRepository<VillaNumber>
    {
        void Update(VillaNumber entity);
        void SaveChanges();
    }
}
