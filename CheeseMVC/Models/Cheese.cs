using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CheeseMVC.Models
{
    public class Cheese : Document
    {

        // basic properties for the Cheese object
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }

        /* how we relate Cheese objects to the Category
         * objects in a one-to-many relationship
         */
        public CheeseCategory Category { get; set; }
        public int CategoryID { get; set; }

        /* allows us to late display what menus the cheese belows to,
         * possibly reference to the join table, required in asp.net?
         */
        public IList<CheeseMenu> CheeseMenus { get; set; }
    }
}
