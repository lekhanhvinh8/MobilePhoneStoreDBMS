using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class ProducerDto
    {
        public ProducerDto()
        {

        }
        public ProducerDto(Producer producer)
        {
            if(producer != null)
            {
                this.ProducerID = producer.ProducerID;
                this.Name = producer.Name;
            }
        }

        public int ProducerID { get; set; }
        public string Name { get; set; }

        public Producer ToProducer()
        {
            var producer = new Producer();
            producer.Name = this.Name;
            return producer;
        }

    }
}