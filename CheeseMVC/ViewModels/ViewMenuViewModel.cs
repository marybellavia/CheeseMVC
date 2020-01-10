using System;
using System.Collections.Generic;
using CheeseMVC.Models;

namespace CheeseMVC.ViewModels
{
    public class ViewMenuViewModel
    {
        // setting properties
        /* with this we can access a menu object and the
         * corresponding list of cheesemenu objects from
         the join table to display a list of cheeses in a
         given menu */
        public Menu Menu { get; set; }
        public IList<CheeseMenu> Items { get; set; }
    }
}
