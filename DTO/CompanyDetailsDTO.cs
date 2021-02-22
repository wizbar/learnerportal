using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace learner_portal.DTO
{
    public class CompanyDetailsDTO
    {
        [DisplayName("Company Id")]
        public long CompanyId { get; set; }
        [DisplayName("Company Registration No")]
        // Company Information
        public string CompanyName { get; set; }
        [DisplayName("Company Registration No")]
        public string CompanyRegistrationNo { get; set; }
        [DisplayName("Date Business Commenced")]
        public DateTime DateBusinessCommenced { get; set; }
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [DisplayName("Contact Surname")]
        public string ContactSurname { get; set; }
        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }
        [DisplayName("Contact Mobile")]
        public string ContactMobile { get; set; }
        [DisplayName("Contact Telephone")]
        public string ContactTelephone { get; set; }
        [DisplayName("Photo Path")]
        public string PhotoPath { get; set; }
        
        [DisplayName("Photo Name")]
        public string PhotoName { get; set; }
        
        
        // Address Information
        
        [DisplayName("House No")]
        public string HouseNo { get; set; }      
        [DisplayName("Street Name")]
        public string StreetName { get; set; }
        [DisplayName("Suburb")]
        public string SuburbName{ get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        [DisplayName("Province")]
        public string ProvinceName { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }

        public List<DocumentDetailsDTO> Documents = new List<DocumentDetailsDTO>();
        
    }
}