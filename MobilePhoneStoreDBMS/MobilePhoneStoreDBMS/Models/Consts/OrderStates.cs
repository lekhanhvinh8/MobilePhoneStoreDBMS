using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Consts
{
    public static class OrderStates
    {
        public static int Pending = 0;

        public static int Canceled = -1;

        public static int Confirmed = 1;
    }
}