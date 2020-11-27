using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class CategoryDto
    {
        private Category _category;
        public CategoryDto()
        {
            _category = new Category();
        }
        public CategoryDto(Category category)
        {
            if(category != null)
            {
                _category = category;
                this.CategoryID = category.CategoryID;
                this.Name = category.Name;
            }
        }
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}