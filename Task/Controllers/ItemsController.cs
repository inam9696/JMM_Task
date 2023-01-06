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
        public IActionResult Index()
        {
            var listdata = _appDbContext.Items.ToList();
            return View(listdata);
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
