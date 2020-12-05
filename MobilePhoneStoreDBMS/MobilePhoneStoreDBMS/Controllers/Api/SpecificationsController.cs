using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace MobilePhoneStoreDBMS.Controllers.Api
{
    public class SpecificationsController : ApiController
    {
        private MobilePhoneStoreDBMSEntities _context;
        public SpecificationsController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        public List<ProductSpecificationDto> GetProductSpecifications()
        {
            var specifications = new List<ProductSpecificationDto>();

            foreach (var specification in this._context.ProductSpecifications.ToList())
            {
                specifications.Add(new ProductSpecificationDto(specification));
            }
            return specifications;
        }
        public ProductSpecificationDto GetSpecification(int iD)
        {
            var specification = this._context.ProductSpecifications.Find(iD);

            if (specification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProductSpecificationDto(specification);
        }
        [HttpPost]
        public ProductSpecificationDto CreateSpecification(ProductSpecificationDto specificationDto)
        {
            var specification = specificationDto.ToSpecification();

            this._context.ProductSpecifications.Add(specification);
            this._context.SaveChanges();

            return new ProductSpecificationDto(specification);
        }

        [HttpPut]
        public ProductSpecificationDto UpdateSpecification(ProductSpecificationDto specificationDto)
        {
            var specification = this._context.ProductSpecifications.Find(specificationDto.SpecificationID);

            if (specification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            specification.Name = specificationDto.Name;
            specification.Description = specificationDto.Description;

            this._context.SaveChanges();

            return new ProductSpecificationDto(specification);
        }

        [HttpDelete]
        public ProductSpecificationDto DeleteSpecification(int iD)
        {
            var specification = this._context.ProductSpecifications.Find(iD);

            if (specification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            foreach (var specificationValue in specification.SpecificationValues.ToList())
            {
                this._context.SpecificationValues.Remove(specificationValue);
            }
            this._context.ProductSpecifications.Remove(specification);

            this._context.SaveChanges();

            return new ProductSpecificationDto(specification);
        }
    }
}
