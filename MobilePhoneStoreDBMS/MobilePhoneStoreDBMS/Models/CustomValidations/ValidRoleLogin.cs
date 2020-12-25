using MobilePhoneStoreDBMS.Models.Consts;
using MobilePhoneStoreDBMS.Models.Entities;
using MobilePhoneStoreDBMS.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.CustomValidations
{
    public class ValidRoleLogin : ValidationAttribute
    {
        private MobilePhoneStoreDBMSEntities _context;
        public ValidRoleLogin()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var loginViewModel = (LoginViewModel)validationContext.ObjectInstance;

            var accountDto = loginViewModel.AccountDto;

            if (String.IsNullOrEmpty(accountDto.Username) || String.IsNullOrEmpty(accountDto.Password))
                return ValidationResult.Success; // If the user has not typed information yet, pass the logic below

            var result = Login(accountDto.Username, accountDto.Password);

            if (result == 0) // Invalid user name or password
                return new ValidationResult("Your account or your password is incorrect");

            var accountInDb = this._context.Accounts.SingleOrDefault(a => a.Username == accountDto.Username);

            if (accountInDb == null)
                throw new Exception("Not found");

            var roleID = accountInDb.Role.RoleID;

            if (roleID != loginViewModel.RoleID && loginViewModel.RoleID != RoleIds.Unknown)
                return new ValidationResult("you are not logged in with the correct role");

            return ValidationResult.Success;
        }
        private int Login(string username, string password)
        {
            string pwd = Syptop.Encrypt(password, true);
            SqlParameter[] sqlParams =
            {
                new SqlParameter("@username", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@password", System.Data.SqlDbType.NVarChar)
            };

            sqlParams[0].Value = username;
            sqlParams[1].Value = pwd;

            var res = this._context.Database.SqlQuery<int>("sp_Account_Login @username, @password", sqlParams).SingleOrDefault();

            return res;
        }
    }
}