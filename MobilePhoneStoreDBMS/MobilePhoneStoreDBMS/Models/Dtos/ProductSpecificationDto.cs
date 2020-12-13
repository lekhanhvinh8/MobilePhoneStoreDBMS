using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class ProductSpecificationDto
    {
        public ProductSpecificationDto()
        {
            this.Values = new List<SpecificationValueDto>();
        }
        public ProductSpecificationDto(ProductSpecification productSpecification)
        {
            if(productSpecification != null)
            {
                SpecificationID = productSpecification.SpecificationID;
                Name = productSpecification.Name;
                Description = productSpecification.Description;
                this.Values = new List<SpecificationValueDto>();
                foreach (var value in productSpecification.SpecificationValues)
                {
                    this.Values.Add(new SpecificationValueDto(value));
                }

                this.IsHavingProduct = false;
                
                foreach (var specificationValue in productSpecification.SpecificationValues)
                {
                    if (specificationValue.Products.Count() != 0)
                    {
                        this.IsHavingProduct = true;
                        break;
                    }
                }
            }
        }
        public int SpecificationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHavingProduct { get; set; }
        public ICollection<SpecificationValueDto> Values { get; set; }
        public ProductSpecification ToSpecification()
        {
            var specificaion = new ProductSpecification();
            specificaion.Name = this.Name;
            specificaion.Description = this.Description;

            var values = new List<SpecificationValue>();
            foreach (var value in this.Values)
            {
                values.Add(value.CreateModel());
            }
            specificaion.SpecificationValues = values;

            return specificaion;
        }
    }
}