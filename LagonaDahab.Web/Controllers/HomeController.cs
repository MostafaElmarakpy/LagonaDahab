using System.Diagnostics;
using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LagonaDahab.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            HomeViewModel homeView = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperty: "VillaAmenity"),
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                Nights=1
            };

            return View(homeView);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights , DateOnly checkInDate)
        {
            //Thread.Sleep(2000); // Simulate a delay for demonstration purposes

            var villaList = _unitOfWork.Villa.GetAll(includeProperty: "VillaAmenity").ToList();

            foreach (var villa in villaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }

            }
            HomeViewModel homeView = new()
            {
                VillaList = villaList,
                CheckInDate = checkInDate,
                Nights = nights

            };
            return PartialView("_VillasList", homeView);

        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
