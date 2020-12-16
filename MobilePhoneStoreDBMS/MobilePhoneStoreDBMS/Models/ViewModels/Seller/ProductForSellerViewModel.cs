using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.ViewModels.Seller
{
    public class ProductForSellerViewModel
    {
        public string Mode { get; set; }
        public bool IsRoleSellerLogin { get; set; }
        public int SellerID { get; set; }
    }
}