using System;
using CheeseMVC.ViewModels;

namespace CheeseMVC.ViewModels
{
    public class UserSignupLoginViewModel
    {
        public AddUserViewModel AddUserVM { get; set; }
        public LoginUserViewModel LoginUserVM { get; set; }

        public UserSignupLoginViewModel()
        {
        }
    }
}
