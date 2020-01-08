using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseViewModel
    {
        [Required]
        [Display(Name = "Cheese Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must give your cheese a description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        public List<SelectListItem> Categories { get; set; }

        [Required]
        [Range(0,5, ErrorMessage = "Must select a number 0-5")]
        public int Rating { get; set; }

        //public List<SelectListItem> CheeseTypes { get; set; }

        public AddCheeseViewModel() { }

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

            //CheeseTypes = new List<SelectListItem>();

            //CheeseTypes.Add(new SelectListItem
            //{
            //    Value = ((int)CheeseType.Hard).ToString(),
            //    Text = CheeseType.Hard.ToString()
            //});
        }



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
