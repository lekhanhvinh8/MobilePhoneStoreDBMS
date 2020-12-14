using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models
{
    public class LoginModel
    {
        private MobilePhoneStoreDBMSEntities context = null;
        cyptop cyptop = new cyptop();
        public LoginModel()
        {
            context = new MobilePhoneStoreDBMSEntities();
        }
        public int Login(string username, string password)
        {
            string pwd = cyptop.Encrypt(password, true);
            SqlParameter[] sqlParams =
            {
                new SqlParameter("@username", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@password", System.Data.SqlDbType.NVarChar)
            };

            sqlParams[0].Value = username;
            sqlParams[1].Value = pwd;

            var res = context.Database.SqlQuery<int>("sp_Account_Login @username, @password", sqlParams).SingleOrDefault();

            return res;
        }
    }
}