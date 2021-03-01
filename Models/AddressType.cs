using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class AddressType
    {
        public AddressType()
        {
            Address = new HashSet<Address>();
        }
        [Key]
        public long AddressTypeId { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string AddressTypeName { get; set; }
        [DisplayName("Address Type Code")]       
        [Required(ErrorMessage = "Please enter Code")]
        
        public string AddressTypeCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}