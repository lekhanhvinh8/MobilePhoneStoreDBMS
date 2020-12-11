using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
namespace MobilePhoneStoreDBMS.Controllers.Api
{
    public class AvatarOfProductController : ApiController
    {
        private MobilePhoneStoreDBMSEntities _context;
        public AvatarOfProductController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }

        [HttpGet]
        public HttpResponseMessage GetAvatarOfProduct(int productID)
        {
            var avatarOfProduct = this._context.AvatarOfProducts.Find(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            return GetResponseMessage(avatarOfProduct);
        }

        [HttpGet]
        public byte[] GetAvatarOfProductAsByte(int productID)
        {
            var avatarOfProduct = this._context.AvatarOfProducts.Find(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            return avatarOfProduct.imageFile;
        }

        [HttpPost]
        public HttpResponseMessage Upload()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            var productID = HttpContext.Current.Request.Form[0];
            
            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);

            var image = reader.ReadBytes(httpPostedFile.ContentLength);

            var avatarOfProduct = new AvatarOfProduct();
            avatarOfProduct.productID = int.Parse(productID);
            avatarOfProduct.imageFile = image;

            this._context.AvatarOfProducts.Add(avatarOfProduct);
            this._context.SaveChanges();

            return GetResponseMessage(avatarOfProduct);
        }

        [HttpPut]
        public HttpResponseMessage Update()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["imageFile"];
            var productID = HttpContext.Current.Request.Form[0];

            BinaryReader reader = new BinaryReader(httpPostedFile.InputStream);

            var image = reader.ReadBytes(httpPostedFile.ContentLength);

            var avatarOfProductInDb = this._context.AvatarOfProducts.Find(int.Parse(productID));
            if (avatarOfProductInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            avatarOfProductInDb.imageFile = image;
            this._context.SaveChanges();

            return GetResponseMessage(avatarOfProductInDb);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int productID)
        {
            var avatarOfProduct = this._context.AvatarOfProducts.Find(productID);

            if (avatarOfProduct == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._context.AvatarOfProducts.Remove(avatarOfProduct);
            this._context.SaveChanges();

            return GetResponseMessage(avatarOfProduct);
        }

        private HttpResponseMessage GetResponseMessage(AvatarOfProduct avatarOfProduct)
        {
            MemoryStream ms = new MemoryStream(avatarOfProduct.imageFile);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StreamContent(ms);

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            return response;
        }
    }
}
