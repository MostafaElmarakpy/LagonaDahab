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
