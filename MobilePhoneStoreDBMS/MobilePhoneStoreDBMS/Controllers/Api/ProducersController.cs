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
    public class ProducersController : ApiController
    {
        private MobilePhoneStoreDBMSEntities _context;
        public ProducersController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }

        public List<ProducerDto> GetAllProducer()
        {
            var producersDto = new List<ProducerDto>();

            foreach (var producer in this._context.Producers.ToList())
            {
                producersDto.Add(new ProducerDto(producer));
            }

            return producersDto;
        }

        public ProducerDto GetProducer(int iD)
        {
            var producer = this._context.Producers.Find(iD);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new ProducerDto(producer);
        }

        [HttpPost]
        public ProducerDto CreateProducer(ProducerDto producerDto)
        {
            var producer = producerDto.ToProducer();

            this._context.Producers.Add(producer);
            this._context.SaveChanges();

            return new ProducerDto(producer);
        }

        [HttpPut]
        public ProducerDto UpdateProducer(ProducerDto producerDto)
        {
            var producer = this._context.Producers.Find(producerDto.ProducerID);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            producer.Name = producerDto.Name;

            this._context.SaveChanges();

            return new ProducerDto(producer);
        }

        public ProducerDto DeleteProducer(int iD)
        {
            var producer = this._context.Producers.Find(iD);

            if (producer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            this._context.Producers.Remove(producer);
            this._context.SaveChanges();

            return new ProducerDto(producer);
        }
    }
}
