using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        // this private field allows controller to access the database
        private readonly CheeseDbContext context;
        // actually setting value to this private field
        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            // creating title for view
            ViewBag.title = "Cheese Categories";
            // getting my list of categories to pass into the view for display
            List<CheeseCategory> cheeseCategories = context.Categories.ToList();
            // returning view with list of categories passed in
            return View(cheeseCategories);
        }

        // Add action for the GET request
        public IActionResult Add()
        {
            // creating title for view
            ViewBag.title = "Add a Cheese Category";
            // using the viewmodel to access and proccess the information
            AddCategoryViewModel addCatViewModel = new AddCategoryViewModel();
            // returning view with viewmodel passed in
            return View(addCatViewModel);
        }

        // Add action for the POST request to process inputs from form
        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCatViewModel)
        {
            // creating title for view
            ViewBag.title = "Add a Cheese Category";

            // checking if the model/information the user input into the form is valid
            if (ModelState.IsValid)
            {
                // creating the new category using the viewmodel
                CheeseCategory newCategory = new CheeseCategory { Name = addCatViewModel.Name };

                // adding category to the database and saving
                context.Categories.Add(newCategory);
                context.SaveChanges();

                // redirecting back to index to list all categories
                return Redirect("/Category");
            }

            // if model is not valid, re-rendering the form with error messages
            return View(addCatViewModel);
        }

        // ViewCategory action to list all cheese in a category via a GET request
        public IActionResult ViewCategory(int id)
        {
            // conditional to check if the int is 0 from GET request
            if (id == 0)
            {
                // redirecting to the category index 
                return Redirect("/Category");
            }

            /* calling up the category object from the database using the int Id
             * from the GET request. need .Include(cat => cat.Cheeses) to include the
             * list of cheese objects stored as a list in the category object for
             * display in the view */
            CheeseCategory theCategory = context.Categories
                .Include(cat => cat.Cheeses)
                .Single(cat => cat.ID == id);

            // To query for the cheeses from the other side of the relationship
            /*
             * IList<Cheese> theCheeses = context.Cheeses.Include(c => c.Category).Where(c => c.CategoryID == id).ToList();
             */

            // creating title for view
            ViewBag.title = "Cheeses in Category: " + theCategory.Name;

            // returning the view with the list of cheese objects of the category
            return View("ViewCategory", theCategory.Cheeses);
        }
    }
}
