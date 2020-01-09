using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {

        private CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            // old call, from before chzcat database
            //List<Cheese> cheeses = context.Cheeses.ToList();
            ViewBag.title = "My Cheeses";
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();
            return View(cheeses);
        }

        public IActionResult Add()
        {
            ViewBag.title = "Add a Cheese";
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel(context.Categories.ToList());
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            ViewBag.title = "Add a Cheese";
            /* Cheese newCheese = new Cheese();
             * newCheese.Name = Request.get("name");
             * newCheeseDescription = Request.get("description");
             */
            CheeseCategory newCheeseCategory =
                    context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

            if (ModelState.IsValid)
            {
                Cheese newCheese = addCheeseViewModel.CreateCheese(newCheeseCategory);
                // Add the new cheese to my existing cheeses
                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/");
            }

            return View(addCheeseViewModel);

        }

        [HttpPost]
        [Route("/Cheese")]
        [Route("/Cheese/Index")]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/Cheese/Index");
        }

        public IActionResult Edit(int cheeseId)
        {
            ViewBag.title = "Edit Cheese";
            Cheese chz = context.Cheeses.Single(c => c.ID == cheeseId);

            AddEditCheeseViewModel vm = new AddEditCheeseViewModel(chz);

            return View(vm);
        }

        [HttpPost]
        [Route("/Cheese/Edit")]
        public IActionResult Edit(AddEditCheeseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Cheese editedCheese = context.Cheeses.Single(c => c.ID == vm.CheeseId);
                //Cheese editedCheese = CheeseData.GetById(editCheeseViewModel.CheeseId);

                editedCheese.Name = vm.Name;
                editedCheese.Description = vm.Description;
                editedCheese.CategoryID = vm.CategoryID;
                editedCheese.Rating = vm.Rating;

                context.SaveChanges();

                return Redirect("/");
            }
            return View(vm);
            
        }

        //public IActionResult Category(int id)
        //{
        //    if (id == 0)
        //    {
        //        return Redirect("/Category");
        //    }

        //    CheeseCategory theCat = context.Categories.Include(cat => cat.Cheeses).Single(cat => cat.ID == id);

        //    ViewBag.title = "Cheese in Category: " + theCat.Name;
        //    return View("Index", theCat.Cheeses);
        //}
    }
}
