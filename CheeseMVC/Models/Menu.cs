using System;
using System.Collections.Generic;

namespace CheeseMVC.Models
{
    public class Menu
    {
        // basic properties for Menu class/objects
        public int ID { get; set; }
        public string Name { get; set; }

         /* allows us to late display what menus the cheese belows to,
         * possibly reference to the join table, required in asp.net?
         */
        public IList<CheeseMenu> CheeseMenus { get; set; }
    }
}
