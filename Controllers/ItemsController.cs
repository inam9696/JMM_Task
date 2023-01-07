using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Models;

namespace Task.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private AppDBContext _appDbContext;
        public ItemsController(AppDBContext AppDBContext)
        {
            _appDbContext = AppDBContext;
        }
        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "price_Asc";
            //ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_Asc" : "";
            ViewData["DesStortParm"] = String.IsNullOrEmpty(sortOrder) ? "des_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var sd = _appDbContext.Items.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                sd = sd.Where(i => i.Name.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sd = sd.OrderByDescending(i => i.Name).ToList();
                    break;
                case "price_desc":
                    sd = sd.OrderByDescending(i => i.Price).ToList();
                    break;
                    case "des_desc":
                    sd = sd.OrderByDescending(i => i.Description).ToList();
                    break;
                case "price_Asc":
                    sd = sd.OrderBy(i => i.Price).ToList();
                    break;
              

                default:
                    sd = sd.OrderBy(i => i.Name).ToList();
                    break;
            }
            return View(sd);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public IActionResult PostCreate(Item item)
        {

            _appDbContext.Items.Add(item);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var make = _appDbContext.Items.Find(id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        [HttpPost]
        public IActionResult Edit(Item model)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Update(model);
                _appDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var make = _appDbContext.Items.Find(id);
            if (make == null)
            {
                return NotFound();
            }
            else
            {
                _appDbContext.Remove(make);
                _appDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
