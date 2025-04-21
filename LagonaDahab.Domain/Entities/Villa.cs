using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagonaDahab.Domain.Entities
{
    public class Villa
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Range(50, 1000)]
        public double Price { get; set; }
        public int Sqft { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public int Occupancy { get; set; }

        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ValidateNever]
        //VillaAmenity is a collection of Amenity
        //benfist of using this is that we can use it to get all the amenities of a villa
        //and we can use it to get all the villas of an amenity
        

        public IEnumerable<Amenity> VillaAmenity { get; set; } = new List<Amenity>();

    }
}
