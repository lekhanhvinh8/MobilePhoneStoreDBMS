using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class ProductDto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductDto()
        {
            this.SpecificationValuesDto = new List<SpecificationValueDto>();
        }
        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        public string Image { get; set; }

        [Required]
        public int ProducerID { get; set; }
        public string ProducerName { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<SpecificationValueDto> SpecificationValuesDto { get; set; }

        [Required]
        public byte[] ImageFile { get; set; }
    }
}