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
    public class SpecificationValuesController : ApiController
    {
        private MobilePhoneStoreDBMSEntities _context;

        public SpecificationValuesController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        public List<SpecificationValueDto> GetAllValuesOfAllSpecifications()
        {
            var specificationvaluesDto = new List<SpecificationValueDto>();

            foreach (var specificationvalue in this._context.SpecificationValues.ToList())
            {
                specificationvaluesDto.Add(new SpecificationValueDto(specificationvalue));
            }

            return specificationvaluesDto;
        }

        public SpecificationValueDto GetValueOfSpecification(int specificationID, string value)
        {
            var specification = this._context.SpecificationValues.SingleOrDefault(s => s.SpecificationID == specificationID && s.Value == value);

            if (specification == null)
                return null;

            return new SpecificationValueDto(specification);
        }

        [HttpPost]
        public SpecificationValueDto CreateSpecificationValue(SpecificationValueDto specificationValueDto)
        {
            var specificationValue = specificationValueDto.ToSpecificationValue();

            this._context.SpecificationValues.Add(specificationValue);
            this._context.SaveChanges();

            return new SpecificationValueDto(specificationValue);
        }

        [HttpDelete]
        public SpecificationValueDto DeleteSpecificationValue(SpecificationValueDto specificationValueDto)
        {
            var specificationValue =  this._context.SpecificationValues.SingleOrDefault(s => s.SpecificationID == specificationValueDto.SpecificationID && s.Value == specificationValueDto.Value);

            if (specificationValue == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._context.SpecificationValues.Remove(specificationValue);
            this._context.SaveChanges();

            return new SpecificationValueDto(specificationValue);
        }
    }
}
