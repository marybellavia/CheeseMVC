using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Cheese> cheeses = CheeseData.GetAll();

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
                CheeseData.Add(newCheese);

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
                CheeseData.Remove(cheeseId);
            }
            return Redirect("/Cheese/Index");
        }

        public IActionResult Edit(int cheeseId)
        {
            Cheese chz = CheeseData.GetById(cheeseId);
            AddEditCheeseViewModel editCheeseViewModel = new AddEditCheeseViewModel(chz);

            return View(editCheeseViewModel);
        }

        [HttpPost]
        [Route("/Cheese/Edit")]
        public IActionResult Edit(AddEditCheeseViewModel editCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                Cheese editedCheese = CheeseData.GetById(editCheeseViewModel.CheeseId);
                editedCheese.Name = editCheeseViewModel.Name;
                editedCheese.Description = editCheeseViewModel.Description;
                editedCheese.Type = editCheeseViewModel.Type;
                editedCheese.Rating = editCheeseViewModel.Rating;

                return Redirect("/");
            }
            return View(editCheeseViewModel);
            
        }
    }
}
