﻿using LagonaDahab.Domain.Entities;

namespace LagonaDahab.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Villa>? VillaList { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
