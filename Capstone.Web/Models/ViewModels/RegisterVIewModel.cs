using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class RegisterViewModel
    {

        [Display(Name = "First Name")]
        [MaxLength(20, ErrorMessage = "First name maximum is 20 characters")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "Last name maximum is 20 characters")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "A valid email address is required")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

    }
}
