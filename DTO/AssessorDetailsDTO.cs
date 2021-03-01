using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace learner_portal.DTO
{
    public class AssessorDetailsDTO
    {
        public long AssessorId { get; set; }
        public Guid Person { get; set; }
        public DateTime AccredStartDate { get; set; }
        public DateTime AccredEndDate { get; set; }
        public string AccreditationStatusDesc { get; set; }
        public string RegistrationNo { get; set; }
        public string Evaluator { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ProcessIndicatorsDesc { get; set; }
        public string ApplicationTypesDesc { get; set; }
        public DateTime? SendForApprovalDate { get; set; }
        public string CertificateIssuedYn { get; set; }
        public DateTime? CertificateDate { get; set; }
        public string CertificateNo { get; set; }
        public DateTime? ReissueDate { get; set; }
        public string EtqeName { get; set; }
        
        public string HouseNo { get; set; }
        public string StreetName { get; set; }
        public string SuburbName{ get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string ProvinceName { get; set; }
        public string CountryName { get; set; }
        public string AddressType { get; set; }
    }
}