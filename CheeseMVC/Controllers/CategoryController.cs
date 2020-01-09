using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {

        private readonly CheeseDbContext context;

        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<CheeseCategory> cheeseCategories = context.Categories.ToList();

            return View(cheeseCategories);
        }

        public IActionResult Add()
        {
            AddCategoryViewModel addCatViewModel = new AddCategoryViewModel();
            return View(addCatViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCatViewModel)
        {
            if (ModelState.IsValid)
            {
                CheeseCategory newCategory = new CheeseCategory { Name = addCatViewModel.Name };

                context.Categories.Add(newCategory);
                context.SaveChanges();

                return Redirect("/Category");
            }
            return View(addCatViewModel);
        }

        public IActionResult ViewCategory(int id)
        {

            if (id == 0)
            {
                return Redirect("/Category");
            }

            CheeseCategory theCategory = context.Categories
                .Include(cat => cat.Cheeses)
                .Single(cat => cat.ID == id);

            // To query for the cheeses from the other side of the relationship

            /*
             * IList<Cheese> theCheeses = context.Cheeses.Include(c => c.Category).Where(c => c.CategoryID == id).ToList();
             */

            ViewBag.title = "Cheeses in Category: " + theCategory.Name;

            return View("ViewCategory", theCategory.Cheeses);
        }
    }
}
