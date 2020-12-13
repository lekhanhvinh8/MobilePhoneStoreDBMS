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

                if (producer.Products.Count() == 0)
                    IsHavingProduct = false;
                else
                    IsHavingProduct = true;
            }
        }

        public int ProducerID { get; set; }
        public string Name { get; set; }
        public bool IsHavingProduct { get; set; }

        public Producer ToProducer()
        {
            var producer = new Producer();
            producer.Name = this.Name;
            return producer;
        }

    }
}