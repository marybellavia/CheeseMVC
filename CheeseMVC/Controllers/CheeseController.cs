using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        // this private field allows controller to access the database and sets up identity
        private readonly CheeseDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CheeseController(CheeseDbContext dbContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            context = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // old call, from before chzcat database
                //List<Cheese> cheeses = context.Cheeses.ToList();

                // creating title for view
                ViewBag.title = "My Cheeses";
                ViewBag.user = HttpContext.User.Identity.Name;
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                // getting my list of cheeses to pass into the view for display
                /* need the .Include(c => c.Category) so we can pass the category object
                 * so we can pass the category objects into the view as well */
                IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).Where(c => c.User == currentUser).ToList();
                // returning view with list of cheeses passed in
                return View(cheeses);
            }

            return Redirect("UserSignupLogin");
        }

        // Add action for the GET request
        public IActionResult Add()
        {
            // creating title for view
            ViewBag.title = "Add a Cheese";
            /* using the viewmodel to access and proccess the information
             * we created a constructor in AddCheeseViewModel that takes in a IEnumerable as
             * a parameter so we can get a list of categories for the select options in the
             * view */
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel(context.Categories.ToList());
            // returning view with viewmodel passed in 
            return View(addCheeseViewModel);
        }

        // Add action for the POST request to process inputs from form
        [HttpPost]
        public async Task<IActionResult> Add(AddCheeseViewModel addCheeseViewModel)
        {
            // creating title for view
            ViewBag.title = "Add a Cheese";

            // this is all the work that the addCheeseViewModel is doing for us that we don't need code for
            /* Cheese newCheese = new Cheese();
             * newCheese.Name = Request.get("name");
             * newCheeseDescription = Request.get("description");
             */

            /* creating a new cheese category object using info from the viewmodel
             * to be used when constructing the new cheese object */
            CheeseCategory newCheeseCategory =
                    context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

            // checking if the model/information the user input into the form is valid
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                // CANT FIGURE OUT HOW TO PARSE STRING ID TO INT
                // creating the new cheese object
                Cheese newCheese = addCheeseViewModel.CreateCheese(newCheeseCategory, currentUser);

                // adding the new cheese to my existing cheeses and updating database
                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                //returning to index
                return Redirect("/");
            }

            /* needed for return showing error messages -- creating a new AddCheeseViewModel
             * using the info from the addviewmodel passed into the view on the post request
             * and also using the constructor to re-populate the list */
            AddCheeseViewModel redoCheese = new AddCheeseViewModel(context.Categories.ToList())
            {
                Name = addCheeseViewModel.Name,
                Description = addCheeseViewModel.Description,
                Rating = addCheeseViewModel.Rating,
            };
            // if model is not valid, re-rendering the form with error messages
            return View(redoCheese);

        }
        // Remove option is in the index view, this processes the POST reuest to remove from the index view
        [HttpPost]
        [Route("/Cheese")]
        [Route("/Cheese/Index")]
        public IActionResult Remove(int[] cheeseIds)
        {
            // used checkboxes, looping through list of cheeseIds
            foreach (int cheeseId in cheeseIds)
            {
                // accessing the existing cheese object
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                // removing each cheese in the list from the database
                context.Cheeses.Remove(theCheese);
            }

            // saving changes to the database
            context.SaveChanges();

            // redirecting back to the index to show cheese list without cheeses
            return Redirect("/Cheese/Index");
        }

        // Edit view for the GET request
        public IActionResult Edit(int cheeseId)
        {
            // creating title for view
            ViewBag.title = "Edit Cheese";
            /* creating a variable to hold the cheese object based on what int
             * cheeseId integer that came in with the GET request */
            Cheese chz = context.Cheeses.Single(c => c.ID == cheeseId);
            /* creating a viewmodel using the chz object and IEnumerable list of categories
             * IEnumerable list of categories needed to display select menu of
             * cheese category choices in the view */
            AddEditCheeseViewModel vm = new AddEditCheeseViewModel(chz, context.Categories.ToList());

            // returning view with the viewmodel passed in
            return View(vm);
        }

        // Edit action for the POST request to process inputs from form
        [HttpPost]
        [Route("/Cheese/Edit")]
        public IActionResult Edit(AddEditCheeseViewModel vm)
        {
            // checking if the model/information the user input into the form is valid
            if (ModelState.IsValid)
            {
                // calling up the existing cheese from the database using the viewmodel for editing
                Cheese editedCheese = context.Cheeses.Single(c => c.ID == vm.CheeseId);

                // changing all the possible fields in the edit form using chz object and viewmodel
                editedCheese.Name = vm.Name;
                editedCheese.Description = vm.Description;
                editedCheese.CategoryID = vm.CategoryID;
                editedCheese.Rating = vm.Rating;
                // saving changes to the database
                context.SaveChanges();
                //returning to index
                return Redirect("/");
            }

            // if model is not valid, re-rendering the form with error messages
            return View(vm);
            
        }
    }
}
