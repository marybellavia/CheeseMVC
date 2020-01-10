using System;
using System.Collections.Generic;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        // setting properties
        // needed to be able to accesss CheeseMenu objects
        public int MenuID { get; set; }
        public int CheeseID { get; set; }

        // setting properties
        /* a select item list so we use it in the view to
         * only display cheeses already in the database*/
        public Menu Menu { get; set; }
        public List<SelectListItem> Cheeses { get; set; }

        // empty constructor, needed bc we also made one that takes parameters
        public AddMenuItemViewModel() {}

        /* constructor for adding a cheese to the above list so that it can grab a list of
         * available / already created cheese for the select option in the view */
        public AddMenuItemViewModel(Menu menu, IEnumerable<Cheese> cheeses)
        {
            Menu = menu;

            Cheeses = new List<SelectListItem>();
            foreach (Cheese chz in cheeses)
            {
                Cheeses.Add(new SelectListItem
                {
                    Value = chz.ID.ToString(),
                    Text = chz.Name
                });
            }

        }
    }
}
