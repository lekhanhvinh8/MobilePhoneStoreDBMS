using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.CustomValidations
{
    public class ValidUserNameRegister :  ValidationAttribute
    {
        private MobilePhoneStoreDBMSEntities _context;
        public ValidUserNameRegister()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var registerDto = (RegisterDto) validationContext.ObjectInstance;

            if (String.IsNullOrEmpty(registerDto.Username))
                return ValidationResult.Success; // required username condition is handled by another validation

            var account = this._context.Accounts.SingleOrDefault(a => a.Username == registerDto.Username);

            if (account != null)
                return new ValidationResult("User name is used by another customer. Please try another one");

            return ValidationResult.Success;
        }
    }
}