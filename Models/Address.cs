using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Address
    {
        [Key]
        public long AddressId { get; set; }
        [DisplayName("House No")]
        public string HouseNo { get; set; }
        [DisplayName("Street Name")]
        public string StreetName { get; set; }
        [DisplayName("Suburb")]
        public long? SuburbId { get; set; }
        [DisplayName("City")]
        public long? CityId { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        [DisplayName("Province")]
        public long? ProvinceId { get; set; }
        [DisplayName("Country")]
        public long? CountryId { get; set; }       
        [DisplayName("Company")]
        public long? CompanyId { get; set; }
        [DisplayName("Address Type")]
        public long? AddressTypeId { get; set; }
        [DisplayName("Person")]
        public Guid? PersonId { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }
        public virtual Suburb  Suburb { get; set; }
        public virtual City  City { get; set; }
        public virtual Province  Province { get; set; } 
        public virtual Company  Company { get; set; }
        public virtual Country  Country { get; set; }
        public virtual AddressType  AddressType { get; set; }
        public virtual Person  Person { get; set; }
    }
}