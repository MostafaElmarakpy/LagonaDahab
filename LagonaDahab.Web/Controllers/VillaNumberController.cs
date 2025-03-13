using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using LagonaDahab.Web.Models;
using LagonaDahab.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LagonaDahab.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var VillaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperty: "Villa").ToList();
            return View(VillaNumbers);
        }

        public IActionResult Create()
        {

            VillaNumberViewModel villaNumbervm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).OrderBy(u => u.Text)
            };

            return View(villaNumbervm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberViewModel villaNumberVM)
        {
            // Check if the Villa_Number already exists
            bool villaNumberExists = _unitOfWork.VillaNumber.GetAll()
                .Any(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (villaNumberExists)
            {
                ModelState.AddModelError("VillaNumber.Villa_Number", "Villa Number already exists.");
                TempData["error"] = "Villa Number already exists";
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Add(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(villaNumberVM);
        }

        public IActionResult Update(int VillaNumberId)
        {
            VillaNumberViewModel villaNumbervm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == VillaNumberId)
            };

            if (villaNumbervm.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumbervm);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberViewModel villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(villaNumberVM);
        }

        public IActionResult Delete(int VillaNumberId)
        {
            VillaNumberViewModel villaNumbervm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == VillaNumberId)
            };

            if (villaNumbervm is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }
            return View(villaNumbervm);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberViewModel villaNumberVM)
        {
            VillaNumber? obj = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (obj is not null)
            {
                _unitOfWork.VillaNumber.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been deleted Successfuly";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa Number could not be deleted ";
            return View();
        }
    }
}