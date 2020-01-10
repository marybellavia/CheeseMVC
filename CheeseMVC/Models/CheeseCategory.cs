using System;
using System.Collections.Generic;

namespace CheeseMVC.Models
{
    public class CheeseCategory
    {
        // basic properties of CheeseCategory class/object
        public int ID { get; set; }
        public string Name { get; set; }

        // list of all the cheeses assigned to this category
        public IList<Cheese> Cheeses { get; set; }

    }
}
