using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobilePhoneStoreDBMS.Controllers.Api
{
    public class CategoriesController : ApiController
    {
        private MobilePhoneStoreDBMSEntities _context;
        public CategoriesController()
        {
            this._context = new MobilePhoneStoreDBMSEntities(); 
        }

        public List<Category> GetCategories()
        {
            return this._context.Categories.ToList();
        }

        public Category GetCategory(int iD)
        {
            var category = this._context.Categories.Find(iD);

            if (category == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return category;
        }

        [HttpPost]
        public Category CreateCategory(Category category)
        {
            this._context.Categories.Add(category);
            this._context.SaveChanges();

            return category;
        }

        [HttpPut]
        public Category UpdateCategory(Category category)
        {
            var categoryInDb = this._context.Categories.Find(category.CategoryID);

            if(categoryInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            categoryInDb.Name = category.Name;

            this._context.SaveChanges();

            return categoryInDb;
        }

        public void DeleteCategory(int id)
        {
            var categoryInDb = this._context.Categories.Find(id);

            if (categoryInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._context.Categories.Remove(categoryInDb);
            this._context.SaveChanges();
        }
    }
}
