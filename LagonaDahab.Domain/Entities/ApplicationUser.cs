﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
