using System;
using System.Collections.Generic;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public int MenuID { get; set; }
        public int CheeseID { get; set; }


        public Menu Menu { get; set; }
        public List<SelectListItem> Cheeses { get; set; }

        public AddMenuItemViewModel() {}

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
