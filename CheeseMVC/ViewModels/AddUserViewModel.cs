using System;
using System.ComponentModel.DataAnnotations;
using CheeseMVC.Models;

namespace CheeseMVC.ViewModels
{

    public class AddUserViewModel
    {
        [Required(ErrorMessage = "You must input a username")]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Your password must be 6-20 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Verify Password")]
        public string Verify { get; set; }

        public AddUserViewModel() { }

        //public User CreateUser()
        //{
        //    return new User
        //    {
        //        Username = this.Username,
        //        Email = this.Email,
        //        Password = this.Password
        //    };
        //}
    }
}
