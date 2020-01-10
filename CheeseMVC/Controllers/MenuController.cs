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
    public class MenuController : Controller
    {

        // this private field allows controller to access the database
        private readonly CheeseDbContext context;
        // actually setting value to this private field
        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            // creating title for view
            ViewBag.title = "Menus";
            // getting my list of menus to pass into the view for display
            List<Menu> menus = context.Menus.ToList();
            // returning view with list of menus passed in
            return View(menus);
        }

        // Add action for the GET request
        public IActionResult Add()
        {
            // creating title for view
            ViewBag.title = "New Menu";
            // using the viewmodel to access and proccess the information
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            // returning view with viewmodel passed in 
            return View(addMenuViewModel);
        }

        // Add action for the POST request to process inputs from form
        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            // checking if the model/information the user input into the form is valid
            if (ModelState.IsValid)
            {
                // creating new Menu object
                Menu newMenu = new Menu { Name = addMenuViewModel.Name};
                // adding menu object into database and updating database
                context.Menus.Add(newMenu);
                context.SaveChanges();

                // redirecting to view the objects in the new menu
                // newMenu.ID is used for the GET request to display correct page
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            // if model is not valid, re-rendering the form with error messages
            return View(addMenuViewModel);
        }
        // ViewMenu action to list all cheese in a category via a GET request
        public IActionResult ViewMenu(int id)
        {
            /* calling up the menu object from the database using the int Id
             * from the GET request. */
            Menu menu = context.Menus.Single(m => m.ID == id);

            /* calling up the join table of CheeseMenu objects
             using the id from the GET request and including the cheese objects
             so that I can list them in the view */
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            // creating the viewmodel from the items and menu variables initiated above
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel()
            {
                Items = items,
                Menu = menu
            };

            // creating title for view
            ViewBag.title = "Menu: " + menu.Name;

            /* returning the view for the viewmenu page with the viewmodel
             * passed in holding all the info for display */
            return View(viewMenuViewModel);
        }

        // AddItem action for the GET request
        public IActionResult AddItem(int id)
        {
            /* calling up the menu object from the database using the int Id
             * from the GET request. */
            Menu menu = context.Menus.Single(menu => menu.ID == id);

            /* calling up a list of all the cheese to pass into the viewmodel
             to create select option in the view so that the user can only chose
             from cheeses alreadyin the database */
            List<Cheese> cheeses = context.Cheeses.ToList();


            // creating the viewmodel using the menu object and list of cheeses
            AddMenuItemViewModel addMenuItemViewModel =
                new AddMenuItemViewModel(menu, cheeses);

            // creating title for view
            ViewBag.title = "Add Item to Menu: " + menu.Name;

            // returning the view with the viewmodel passed in
            return View(addMenuItemViewModel);
        }

        // Add action for the POST request to process inputs from form
        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            // checking if the model/information the user input into the form is valid
            if (ModelState.IsValid)
            {
                // getting the relevant IDs from the viewmodel that came in with the POST request
                int cheeseID = addMenuItemViewModel.CheeseID;
                int menuID = addMenuItemViewModel.MenuID;

                // creating a variable that will tell us if the cheese is already in the menu
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == cheeseID)
                    .Where(cm => cm.MenuID == menuID).ToList();

                // conditional checking that the cheese is not already in the menu
                if (existingItems.Count == 0)
                {
                    // creating a new cheesemenu item
                    // will be linked to correct menu via MenuID
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        MenuID = menuID,
                        CheeseID = cheeseID
                    };
                    // adding new CheeseMenu object to database and updating databse
                    context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();

                    // redirecting to the page to view all cheeses in the menu
                    return Redirect("/Menu/ViewMenu/" + menuItem.MenuID);
                }
            }

            /* means cheese is already on the menu, does nothing and redirects
             * back to the view page listing all the cheeses in a menu */ 
            return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.MenuID);
        }
    }
}
