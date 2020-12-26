using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Web.Mvc;

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

        [System.Web.Http.HttpGet]
        public bool ModifyStatus(int productID, bool status)
        {
            var product = this._context.Products.Find(productID);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            product.Status = status;
            this._context.SaveChanges();

            return status;
        }

        [System.Web.Http.HttpPost]
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

            var productInDb = this._context.Products.Include(p => p.Category)
                                                    .Include(p => p.Producer)
                                                    .SingleOrDefault(p => p.ProductID == product.ProductID);

            return new ProductForSellerDto(productInDb);
        }

        [System.Web.Http.HttpPost]
        public ProductForSellerDto CreateProductFullInfo()
        {
            var productDto = new ProductDto();

            var form = HttpContext.Current.Request.Form;

            //Get data
            productDto.Name = form["name"].ToString();
            productDto.Description = form["description"].ToString();
            productDto.ProducerID = int.Parse(form["producerID"].ToString());
            productDto.CategoryID = int.Parse(form["categoryID"].ToString());
            productDto.Price = int.Parse(form["price"].ToString());
            productDto.Quantity = int.Parse(form["quantity"].ToString());
            productDto.Status = Boolean.Parse(form["status"]);
            productDto.SpecificationValuesDto = JsonConvert.DeserializeObject<List<SpecificationValueDto>>(form.Get("specificationValuesDto"));
            //var a = JsonConvert.DeserializeObject(form.Get("item"));

            //serialize specification values to string -- format: "1,16GB|2,128GB|3,2 Sims"
            string specificationValuesString = "";
            foreach (var specificationValueDto in productDto.SpecificationValuesDto)
            {
                string record = specificationValueDto.SpecificationID.ToString() + "," + specificationValueDto.Value;
                specificationValuesString += record + "|";
            }
            if(specificationValuesString.Length > 1)
                specificationValuesString = specificationValuesString.Substring(0, specificationValuesString.Length - 1);

            //Get image file
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);
            var imageFile = reader.ReadBytes(httpPostedFile.ContentLength);
            productDto.ImageFile = imageFile;

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));
            this._context.AddNewProduct(productDto.Name
                                        , productDto.Description
                                        , productDto.Quantity
                                        , productDto.Status
                                        , productDto.Price
                                        , productDto.ProducerID
                                        , productDto.CategoryID
                                        , productDto.ImageFile
                                        , specificationValuesString
                                        , isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure inserting sequence of data");

            var product = this._context.Products.SingleOrDefault(p => p.Name == productDto.Name);

            if (product == null)
                throw new Exception("Not found");

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpPut]
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

            var productInDb = this._context.Products.Include(p => p.Category)
                                                    .Include(p => p.Producer)
                                                    .SingleOrDefault(p => p.ProductID == product.ProductID);

            return new ProductForSellerDto(productInDb);
        }

        public ProductForSellerDto DeleteFullInfo(int productID)
        {
            var product = this._context.Products.Include(p => p.Category)
                                                .Include(p => p.Producer)
                                                .SingleOrDefault(p => p.ProductID == productID);

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));
            this._context.DeleteProduct(productID, isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure deleting product"); // product ID is not found in 2 tables Products and AvatarOfProduct

            return new ProductForSellerDto(product);
        }

        [System.Web.Http.HttpDelete]
        public ProductForSellerDto Delete(int productID)
        {
            var product = this._context.Products.Include(p => p.Producer)
                                                .Include(p => p.Category)
                                                .SingleOrDefault(p => p.ProductID == productID);

            var productDto = new ProductForSellerDto(product);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            product.SpecificationValues.Clear();

            this._context.Products.Remove(product); // after deletion, this product no longer has category and producer
            this._context.SaveChanges();

            return productDto;
        }
    }
}
