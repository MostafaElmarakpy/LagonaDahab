using LagonaDahab.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace LagonaDahab.Infrastructure.Configurations
{
    class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder.HasKey(vn => vn.Id);

            builder.HasOne(vn => vn.Villa)
                   .WithMany()
                   .HasForeignKey(vn => vn.VillaId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(vn => vn.Id).ValueGeneratedOnAdd();

            builder.HasOne(va => va.Villa)
                   .WithMany(v => v.VillaAmenity)
                   .HasForeignKey(va => va.VillaId);

        }
    }

}
