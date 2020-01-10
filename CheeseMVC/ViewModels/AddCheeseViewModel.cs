using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseViewModel
    {
        // validation
        [Required]
        [Display(Name = "Cheese Name")]
        // setting the property
        public string Name { get; set; }

        // validation
        [Required(ErrorMessage = "You must give your cheese a description")]
        // setting the property
        public string Description { get; set; }

        // validation
        [Required]
        [Range(0, 5, ErrorMessage = "Must select a number 0-5")]
        // setting the property
        public int Rating { get; set; }

        // validation
        [Required]
        [Display(Name = "Category")]
        // setting the property
        // should link as a foreign key to Category object table
        public int CategoryID { get; set; }

        // creating a list of categories for select option in view
        public List<SelectListItem> Categories { get; set; }

        // empty constructor, needed bc we also made one that takes parameters
        public AddCheeseViewModel() { }

        /* constructor for adding a new cheese so that it can grab a list of
         * available / already created categories */
        public AddCheeseViewModel(IEnumerable<CheeseCategory> categories)
        {
            Categories = new List<SelectListItem>();

            foreach (CheeseCategory chzcat in categories)
            {
                Categories.Add(new SelectListItem
                {
                    Value = chzcat.ID.ToString(),
                    Text = chzcat.Name
                });
            }
        }

        // helper function to create a new cheese
        public Cheese CreateCheese(CheeseCategory newCheeseCategory)
        {
            return new Cheese
            {
                Name = this.Name,
                Description = this.Description,
                CategoryID = newCheeseCategory.ID,
                Rating = this.Rating
            };
        }
    }

}
