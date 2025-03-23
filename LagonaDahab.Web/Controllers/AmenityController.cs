using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LagonaDahab.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var Amenities = _unitOfWork.Amenity.GetAll(includeProperty: "Villa").ToList();
            return View(Amenities);
        }

        public IActionResult Create()
        {

            AmenityViewModel amenityvm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).OrderBy(u => u.Text)
            };

            return View(amenityvm);
        }

        [HttpPost]
        public IActionResult Create(AmenityViewModel amenityVM)
        {

            if (ModelState.IsValid && amenityVM.Amenity != null)
            {
                _unitOfWork.Amenity.Add(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenities has been Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Update(int amenityId)
        {
            AmenityViewModel amenityvm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (amenityvm.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityvm);
        }

        [HttpPost]
        public IActionResult Update(AmenityViewModel amenityVM)
        {
            if (ModelState.IsValid && amenityVM.Amenity != null)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenities has been Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityViewModel amenityvm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (amenityvm is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }
            return View(amenityvm);
        }

    [HttpPost]
public IActionResult Delete(AmenityViewModel amenityVM)
{
    if (amenityVM.Amenity == null)
    {
        TempData["error"] = "The Amenity is null.";
        return RedirectToAction(nameof(Index));
    }

    Amenity? obj = _unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);
    if (obj is not null)
    {
        _unitOfWork.Amenity.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "The Amenities has been deleted Successfully";
        return RedirectToAction(nameof(Index));
    }
    TempData["error"] = "The Amenities could not be deleted ";
    return View();
}
    }
}