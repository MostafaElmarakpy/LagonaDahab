using LagonaDahab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Infrastructure.Configurations
{
    public class VillaNumberConfiguration : IEntityTypeConfiguration<VillaNumber>
    {
        public void Configure(EntityTypeBuilder<VillaNumber> builder)
        {
            builder.HasKey(vn => vn.Villa_Number);

            builder.HasOne(vn => vn.Villa)
                   .WithMany()
                   .HasForeignKey(vn => vn.VillaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
