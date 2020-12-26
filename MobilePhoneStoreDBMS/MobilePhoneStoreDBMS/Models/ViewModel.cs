using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobilePhoneStoreDBMS.Models.Entities;

namespace MobilePhoneStoreDBMS.Models
{
    public class ViewModel
    {
        private MobilePhoneStoreDBMSEntities _context;
        public ViewModel()
        {
            _context = new MobilePhoneStoreDBMSEntities();
        }
        public Producer producer { get; set; }
        public Category category { get; set; }
        public Product product { get; set; }
        public List<Product> allProducts { get; set; }
        public List<Product> allProductsOfProducer { get; set; }
        public List<Product> allProductsOfCategory { get; set; }
        public List<Producer> allProducers { get; set; }
        public int countCarts { get; set; }
        public List<Category> allCategories() { 
            var res = _context.Database.SqlQuery<Category>("select * from view_Category_List").ToList();
            return res;
        }
        public List<Product> productsOfSearch { get; set; }
    }
}