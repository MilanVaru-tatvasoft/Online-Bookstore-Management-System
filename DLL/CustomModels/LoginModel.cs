using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z]{3,}\.[a-z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string loginEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }

        public bool isdeleted { get; set; } 

    }
    public class RegisterModel
    {
        [Required(ErrorMessage ="This field is required")]
        public string? firstname { get; set;}
        [Required(ErrorMessage ="This field is required")]
        public string? lastname { get; set;}
        [Required(ErrorMessage ="This field is required")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z]{3,}\.[a-z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string? email { get; set;}
        [Required(ErrorMessage ="This field is required")]
        public string? Password { get; set;}

        [Compare("Password", ErrorMessage ="Password must be same")]
        public string? ConfirmPassword { get; set;}
        public string? Gender { get; set;}


        [Required(ErrorMessage = "This field is required")]
        public DateTime? birthdate { get; set;}

        [Required(ErrorMessage = "This field is required")]
        public string? Contact { get; set;}
        public string? role { get; set;}

        [Required(ErrorMessage = "This field is required")]
        public string? Address { get; set;}

        [Required(ErrorMessage = "This field is required")]
        public string? city { get; set;}

    }
}
