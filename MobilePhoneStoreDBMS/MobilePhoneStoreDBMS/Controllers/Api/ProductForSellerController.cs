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
    public class ProductForSellerController : ApiController
    {
        private MobilePhoneStoreDBMSEntities _context;
        public ProductForSellerController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }

        public List<ProductForSellerDto> GetAll()
        {
            var productForSellersDto = new List<ProductForSellerDto>();

            foreach (var product in this._context.Products.ToList())
            {
                productForSellersDto.Add(new ProductForSellerDto(product));
            }

            return productForSellersDto;
        }

        public ProductForSellerDto Get(int productID)
        {
            var product = this._context.Products.Find(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProductForSellerDto(product);
        }

        [HttpPost]
        public ProductForSellerDto Create(ProductForSellerDto productForSellerDto)
        {
            var product = productForSellerDto.CreateModel();

            foreach (var specificationValueDto in productForSellerDto.SpecificationValuesDto)
            {
                var specificationValue = this._context.SpecificationValues.SingleOrDefault(s => s.SpecificationID == specificationValueDto.SpecificationID
                                                                                          && s.Value == specificationValueDto.Value);
                if (specificationValue == null)
                    throw new Exception("Can not find this specification or value");

                product.SpecificationValues.Add(specificationValue);
            }


            this._context.Products.Add(product);
            this._context.SaveChanges();

            return new ProductForSellerDto(product);
        }

        [HttpPut]
        public ProductForSellerDto Update(ProductForSellerDto productForSellerDto)
        {
            var product = this._context.Products.Find(productForSellerDto.ProductID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            productForSellerDto.UpdateModel(product);

            product.SpecificationValues.Clear();
            
            foreach (var specificationValueDto in productForSellerDto.SpecificationValuesDto)
            {
                var specificationValue = this._context.SpecificationValues.SingleOrDefault(s => s.SpecificationID == specificationValueDto.SpecificationID
                                                                                          && s.Value == specificationValueDto.Value);
                if (specificationValue == null)
                    throw new Exception("Can not find this specification or value");

                product.SpecificationValues.Add(specificationValue);
            }

            this._context.SaveChanges();

            return new ProductForSellerDto(product);
        }

        [HttpDelete]
        public ProductForSellerDto Delete(int productID)
        {
            var product = this._context.Products.Find(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            product.SpecificationValues.Clear();

            this._context.Products.Remove(product);
            this._context.SaveChanges();

            return new ProductForSellerDto(product);
        }
    }
}
