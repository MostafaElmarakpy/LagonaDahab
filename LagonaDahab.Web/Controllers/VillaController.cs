using LagonaDahab.Domain.Entities;
using LagonaDahab.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace LagonaDahab.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext  _dbContext;

        public VillaController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var villas = _dbContext.Villas.ToList(); 
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (villa.Name==villa.Description)
            {
                ModelState.AddModelError("name", "the Description can't exactly match Name");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Villas.Add(villa);
                _dbContext.SaveChanges();
                TempData["success"] = "The Villa has been Created Successfuly";

                return RedirectToAction(nameof(Index));
            }
                return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj= _dbContext.Villas.FirstOrDefault(u=>u.Id ==villaId);
            if(obj==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid && villa.Id>0)
            {
                _dbContext.Villas.Update(villa);
                _dbContext.SaveChanges();
                TempData["success"] = "The Villa has been Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _dbContext.Villas.FirstOrDefault(u => u.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa VillaNumberId)
        {
         Villa? obj = _dbContext.Villas.FirstOrDefault(u => u.Id == VillaNumberId.Id);
            if (obj is not null)
            {
                _dbContext.Villas.Remove(obj);
                _dbContext.SaveChanges();
                TempData["error"] = "The Villa has been deleted Successfuly";
                return RedirectToAction(nameof(Index));
                
            }
            return View();
        }






    }
}
 