using MobilePhoneStoreDBMS.Models.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter name with 3 to 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Your phone number should be between 6 and 20 numbers")]
        [RegularExpression(@"[0-9]{6,20}", ErrorMessage = "Your phone number is not valid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Your mail is not valid")]
        [StringLength(50, MinimumLength = 6)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your user name")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Please enter user name with 6 to 20 characters")]
        [ValidUserNameRegister]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password shoud be between 6 and 20 characters")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}