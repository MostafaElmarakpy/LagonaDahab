using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Application.Common.Utility;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using LagonaDahab.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LagonaDahab.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                if (villa.Image != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(villa.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var filestream = new FileStream(Path.Combine(imagePath,fileName), FileMode.Create, FileAccess.Write);
                    
                        villa.Image.CopyTo(filestream);
                    
                    villa.ImageUrl = @"\images\VillaImage\"+ fileName;
                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }


                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Save();
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


                if (villa.Image != null)
                {

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    string filePath = Path.Combine(uploadsFolder, fileName);



                    if (!string.IsNullOrEmpty(villa.ImageUrl))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
                }



                _unitOfWork.Villa.Update(villa);
                _unitOfWork.Save();
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
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == VillaNumberId.Id);
            if (villa is not null)
            {

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }


                _unitOfWork.Villa.Remove(villa);
                _unitOfWork.Save();
                TempData["error"] = "The Villa has been deleted Successfuly";
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

    }
}
