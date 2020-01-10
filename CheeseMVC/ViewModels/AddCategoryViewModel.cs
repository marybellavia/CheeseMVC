using System;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    public class AddCategoryViewModel
    {
        // validation
        [Required]
        [Display(Name = "Category Name")]
        // setting the property
        public string Name { get; set; }

        /* don't need an ID because that is automatically generated
         * to be a unique ID in the actual Category Model */
    }
}
