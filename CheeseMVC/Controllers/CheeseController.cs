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
        static private List<Cheese> Cheeses = new List<Cheese>();


        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.cheeses = Cheeses;

            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("/Cheese/Add")]
        public IActionResult NewCheese(string name, string description)
        {
            // Add the new cheese to my existing cheeses
            Cheese chz = new Cheese(name, description);
            Cheeses.Add(chz);

            return Redirect("/Cheese");
        }

        public IActionResult Remove()
        {
            ViewBag.cheeses = Cheeses;

            return View("Remove");
        }

        [HttpPost]
        [Route("/Cheese/Remove")]
        public IActionResult RemoveCheese(string[] cheeseName)
        {
            foreach (Cheese chzz in Cheeses.ToList())
            {
                foreach (string ch in cheeseName)
                {
                    if (chzz.Name == ch)
                    {
                        Cheeses.Remove(chzz);
                    }
                }
                
            }

            return Redirect("/Cheese");
        }
    }
}
