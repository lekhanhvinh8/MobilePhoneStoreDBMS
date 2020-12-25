using MobilePhoneStoreDBMS.Models.CustomValidations;
using MobilePhoneStoreDBMS.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        public AccountDto AccountDto { get; set; }

        [ValidRoleLogin]
        public int RoleID { get; set; }
    }
}