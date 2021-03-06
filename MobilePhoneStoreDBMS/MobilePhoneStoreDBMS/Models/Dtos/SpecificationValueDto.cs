﻿using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class SpecificationValueDto
    {
        public SpecificationValueDto()
        {
        }
        public SpecificationValueDto(SpecificationValue specificationValue)
        {
            this.SpecificationID = specificationValue.SpecificationID;
            this.Value = specificationValue.Value;

            if (specificationValue.Products.Count() == 0)
                this.IsHavingProduct = false;
            else
                this.IsHavingProduct = true;
        }
        public int SpecificationID { get; set; }
        public string Value { get; set; }
        public bool IsHavingProduct { get; set; }
        public SpecificationValue CreateModel()
        {
            var specificationValue = new SpecificationValue();
            specificationValue.SpecificationID = this.SpecificationID;
            specificationValue.Value = this.Value;

            return specificationValue;
        }
    }
}