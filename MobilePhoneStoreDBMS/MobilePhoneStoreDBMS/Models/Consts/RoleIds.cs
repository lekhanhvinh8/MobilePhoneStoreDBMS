using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Consts
{
    public static class RoleIds
    {
        public const int Admin = 1;
        public const int Seller = 2;
        public const int Customer = 3;
        public const int Unknown = -1;
    }
}