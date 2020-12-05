using MobilePhoneStoreDBMS.Models.Dtos;
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

        public List<CategoryDto> GetCategories()
        {
            var categories = this._context.Categories.ToList();
            var categoriesDto = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var categoryDto = new CategoryDto(category);
                categoriesDto.Add(categoryDto);
            }
            return categoriesDto;
        }

        public CategoryDto GetCategory(int iD)
        {
            var category = this._context.Categories.Find(iD);

            if (category == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new CategoryDto(category);
        }

        [HttpPost]
        public CategoryDto CreateCategory(Category category)
        {
            this._context.Categories.Add(category);
            this._context.SaveChanges();

            return new CategoryDto(category);
        }

        [HttpPut]
        public CategoryDto UpdateCategory(Category category)
        {
            var categoryInDb = this._context.Categories.Find(category.CategoryID);

            if(categoryInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            categoryInDb.Name = category.Name;

            this._context.SaveChanges();

            return new CategoryDto(categoryInDb);
        }

        [HttpDelete]
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
