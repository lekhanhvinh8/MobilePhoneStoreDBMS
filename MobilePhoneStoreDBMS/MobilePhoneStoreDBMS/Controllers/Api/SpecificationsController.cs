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
        public ProductSpecification CreateSpecification(ProductSpecification specification)
        {
            this._context.ProductSpecifications.Add(specification);
            this._context.SaveChanges();

            return specification;
        }
    }
}
