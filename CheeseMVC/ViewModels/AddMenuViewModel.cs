using System;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    public class AddMenuViewModel
    {
        // validation
        [Required]
        [Display(Name = "Menu Name")]
        // setting property
        public string Name { get; set; }

        /* don't need an ID because that is automatically generated
         * to be a unique ID in the actual Menu Model */
    }
}
