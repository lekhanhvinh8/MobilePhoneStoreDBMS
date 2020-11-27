using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class ProductSpecificationDto
    {
        private ProductSpecification _productSpecification;
        public ProductSpecificationDto()
        {
            _productSpecification = new ProductSpecification();
        }

        public ProductSpecificationDto(ProductSpecification productSpecification)
        {
            if(productSpecification != null)
            {
                _productSpecification = productSpecification;
                SpecificationID = productSpecification.SpecificationID;
                Name = productSpecification.Name;
                Description = productSpecification.Description;
            }
        }
        public int SpecificationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}