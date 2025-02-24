using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace LagonaDahab.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (villa.Name == villa.Description)
            {
                ModelState.AddModelError("name", "the Description can't exactly match Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Villa.SaveChanges();
                TempData["success"] = "The Villa has been Created Successfuly";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid && villa.Id > 0)
            {
                _unitOfWork.Villa.Update(villa);
                _unitOfWork.Villa.SaveChanges();
                TempData["success"] = "The Villa has been Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa VillaNumberId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == VillaNumberId.Id);
            if (obj is not null)
            {
                _unitOfWork.Villa.Remove(obj);
                _unitOfWork.Villa.SaveChanges();
                TempData["error"] = "The Villa has been deleted Successfuly";
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

    }
}
