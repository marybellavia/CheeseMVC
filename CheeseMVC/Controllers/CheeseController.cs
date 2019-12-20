using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    { 
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.cheeses = CheeseData.GetAll();

            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("/Cheese/Add")]
        public IActionResult NewCheese(Cheese newCheese)
        {

            /* Cheese newCheese = new Cheese();
             * newCheese.Name = Request.get("name");
             * newCheeseDescription = Request.get("description");
             */

            // Add the new cheese to my existing cheeses
            CheeseData.Add(newCheese);

            return Redirect("/");
        }

        public IActionResult Remove()
        {
            ViewBag.Title = "Remove Cheese";
            ViewBag.cheeses = CheeseData.GetAll();

            return View();
        }

        [HttpPost]
        [Route("/Cheese/Remove")]
        public IActionResult RemoveCheese(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                CheeseData.Remove(cheeseId);
            }
            return Redirect("/");
        }
    }
}
