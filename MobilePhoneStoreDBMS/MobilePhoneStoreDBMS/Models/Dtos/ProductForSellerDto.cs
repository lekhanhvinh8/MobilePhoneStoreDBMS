using MobilePhoneStoreDBMS.Controllers.Api;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class ProductForSellerDto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductForSellerDto()
        {
            this.SpecificationValuesDto = new List<SpecificationValueDto>();
        }

        public ProductForSellerDto(Product product)
        {
            if (product == null)
                throw new Exception("product object is null");

            this.ProductID = product.ProductID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.CategoryID = product.CategoryID;
            this.CategoryName = product.Category.Name;
            this.Price = product.Price;
            this.ProducerID = product.ProducerID;
            this.ProducerName = product.Producer.Name;
            this.Quantity = product.Quantity;
            this.Status = product.Status;
            this.SpecificationValuesDto = new List<SpecificationValueDto>();

            foreach (var specificationValue in product.SpecificationValues)
            {
                this.SpecificationValuesDto.Add(new SpecificationValueDto(specificationValue));
            }
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        public string Image { get; set; }

        [Required]
        public int ProducerID { get; set; }
        public string ProducerName { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<SpecificationValueDto> SpecificationValuesDto { get; set; }

        public Product CreateModel()
        {
            Product product = new Product();

            product.Name = this.Name;
            product.Description = this.Description;
            product.CategoryID = this.CategoryID;
            product.Price = this.Price;
            product.ProducerID = this.ProducerID;
            product.Quantity = this.Quantity;
            product.Status = this.Status;

            /*
            foreach (var specificationValueDto in this.SpecificationValuesDto)
            {
                var context = new MobilePhoneStoreDBMSEntities(); -->> it will throw an exception: Operation Exception because of the conflic context, calling in 2 different place.

                var specificationValue = context.SpecificationValues.SingleOrDefault(s => s.SpecificationID == specificationValueDto.SpecificationID
                                                                                          && s.Value == specificationValueDto.Value);
                if (specificationValue == null)
                    throw new Exception("Can not find this specification or value");

                product.SpecificationValues.Add(specificationValue);
            }
            */
            return product;
        }

        public void UpdateModel(Product product)
        {
            product.Name = this.Name;
            product.Description = this.Description;
            product.CategoryID = this.CategoryID;
            product.Price = this.Price;
            product.ProducerID = this.ProducerID;
            product.Quantity = this.Quantity;
            product.Status = this.Status;

            /*
            foreach (var specificationValueDto in this.SpecificationValuesDto)
            {
                product.SpecificationValues.Add(specificationValueDto.CreateModel());
            }
            */
        }
    }
}