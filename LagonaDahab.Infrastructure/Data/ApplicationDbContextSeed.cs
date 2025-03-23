using LagonaDahab.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LagonaDahab.Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {

        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                // Seed Villas 
                if (!context.Villas.Any())
                {
                    var villasData = await File.ReadAllTextAsync("../LagonaDahab.Infrastructure/Data/DataSeed/villa.json");
                    var villas = JsonSerializer.Deserialize<List<Villa>>(villasData);

                    if (villas != null)
                    {
                        foreach (var villa in villas)
                        {
                            await context.Villas.AddAsync(villa);
                        }
                        await context.SaveChangesAsync();
                    }
                }

                //Seed VillaNumbers
                if (!context.VillaNumbers.Any())
                {
                    var villaNumbersData = await File.ReadAllTextAsync("../LagonaDahab.Infrastructure/Data/DataSeed/villaNumber.json");
                    var villaNumbers = JsonSerializer.Deserialize<List<VillaNumber>>(villaNumbersData);

                    if (villaNumbers != null)
                    {
                        foreach (var villaNumber in villaNumbers)
                        {
                            await context.VillaNumbers.AddAsync(villaNumber);
                        }
                        await context.SaveChangesAsync();
                    }
                }

                //Seed Amenities
                if (!context.Amenities.Any())
                {
                    var amenitiesData = await File.ReadAllTextAsync("../LagonaDahab.Infrastructure/Data/DataSeed/amenity.json");
                    var amenities = JsonSerializer.Deserialize<List<Amenity>>(amenitiesData);
                    if (amenities != null)
                    {
                        foreach (var amenity in amenities)
                        {
                            await context.Amenities.AddAsync(amenity);
                        }
                        await context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContextSeed>();
                logger.LogError(ex.Message);
            }
        }

    }
}
