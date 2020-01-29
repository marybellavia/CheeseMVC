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
using CheeseMVC.Authorization;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        // this private field allows controller to access the database
        private CheeseDbContext Context { get; }
        private IAuthorizationService AuthorizationService { get; }
        private UserManager<IdentityUser> UserManager { get; }
        // actually setting value to this private field
        public CheeseController(CheeseDbContext dbContext,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base()
        {
            Context = dbContext;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var cheeses = from c in Context.Cheeses
                          select c;

            var isAuthorized = User.IsInRole(Constants.AdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized)
            {
                cheeses = cheeses.Where(c => c.UserID == currentUserId);
            }

            ViewBag.title = "My Cheeses";
            return View(cheeses.Include(c => c.Category).ToList());
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
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel(Context.Categories.ToList());
            // returning view with viewmodel passed in 
            return View(addCheeseViewModel);
        }

        // Add action for the POST request to process inputs from form
        [HttpPost]
        public async Task<IActionResult> Add(AddCheeseViewModel addCheeseViewModel)
        {
            // creating title for view
            ViewBag.title = "Add a Cheese";

            if (ModelState.IsValid)
            {
                CheeseCategory newCheeseCategory =
                    Context.Categories.FirstOrDefault(c => c.ID == addCheeseViewModel.CategoryID);

                Cheese newCheese = new Cheese()
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    Rating = addCheeseViewModel.Rating,
                    Category = newCheeseCategory,
                    UserID = UserManager.GetUserId(User)
                };

                var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                    User, newCheese,
                                                    CheeseOperations.Create);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                Context.Cheeses.Add(newCheese);
                await Context.SaveChangesAsync();

                return Redirect("/Cheese/Index");

            }

            return View(addCheeseViewModel);
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
                Cheese theCheese = Context.Cheeses.Single(c => c.ID == cheeseId);
                // removing each cheese in the list from the database
                Context.Cheeses.Remove(theCheese);
            }

            // saving changes to the database
            Context.SaveChanges();

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
            Cheese chz = Context.Cheeses.Single(c => c.ID == cheeseId);
            /* creating a viewmodel using the chz object and IEnumerable list of categories
             * IEnumerable list of categories needed to display select menu of
             * cheese category choices in the view */
            AddEditCheeseViewModel vm = new AddEditCheeseViewModel(chz, Context.Categories.ToList());

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
                Cheese editedCheese = Context.Cheeses.Single(c => c.ID == vm.CheeseId);

                // changing all the possible fields in the edit form using chz object and viewmodel
                editedCheese.Name = vm.Name;
                editedCheese.Description = vm.Description;
                editedCheese.CategoryID = vm.CategoryID;
                editedCheese.Rating = vm.Rating;
                // saving changes to the database
                Context.SaveChanges();
                //returning to index
                return Redirect("/");
            }

            // if model is not valid, re-rendering the form with error messages
            return View(vm);
            
        }
    }
}
