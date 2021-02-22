using System;
using System.Collections.Generic;
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
        public string AddressTypeName { get; set; }
        public string AddressTypeCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}