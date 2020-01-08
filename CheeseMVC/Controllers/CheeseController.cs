using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;

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
            List<Cheese> cheeses = context.Cheeses.ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel();
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {

            /* Cheese newCheese = new Cheese();
             * newCheese.Name = Request.get("name");
             * newCheeseDescription = Request.get("description");
             */

            if (ModelState.IsValid)
            {
                Cheese newCheese = addCheeseViewModel.CreateCheese();
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
                editedCheese.Type = vm.Type;
                editedCheese.Rating = vm.Rating;

                context.SaveChanges();

                return Redirect("/");
            }
            return View(vm);
            
        }
    }
}
