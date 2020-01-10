using System;
using System.Collections.Generic;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    /* Inherits from the AddCheeseViewModel! So can use constructors
     * from that viewmodel */
    public class AddEditCheeseViewModel : AddCheeseViewModel
    {
        // setting property
        public int CheeseId { get; set; }

        // empty constructor, needed bc we also made one that takes parameters
        public AddEditCheeseViewModel() {}

        /* constructor for editing a cheese so that it can grab a list of
         * available / already created categories */
         // extends usage from the viewmodel is inherited from!
        public AddEditCheeseViewModel(Cheese ch, IEnumerable<CheeseCategory> categories) : base(categories)
        {
            // Use Cheese object to initialize the ViewModel properties
            CheeseId = ch.ID;
            Name = ch.Name;
            Description = ch.Description;
            CategoryID = ch.CategoryID;
            Rating = ch.Rating;
        }
    }
}