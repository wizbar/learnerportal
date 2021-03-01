using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace learner_portal.Models
{
    public class Assessor
    {
        [Key]
        public long AssessorId { get; set; }
        [DisplayName("Person")]
        public Guid PersonId { get; set; }
        [DisplayName("Accred Start Date")]
        public DateTime AccredStartDate { get; set; }
        [DisplayName("Accred End Date")]
        public DateTime AccredEndDate { get; set; }
        [DisplayName("Accreditation Status")]
        public long AccreditationStatusesId { get; set; }
        [DisplayName("Registration No")]
        public string RegistrationNo { get; set; }
        [DisplayName("Evaluator")]
        public string EvaluatorsId { get; set; }
        [DisplayName("Application Date")]
        public DateTime ApplicationDate { get; set; }
        [DisplayName("Registration Date")]
        public DateTime? RegistrationDate { get; set; }
        [DisplayName("Approved By")]
        public string ApprovedBy { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [DisplayName("Process Indicator")]
        public long ProcessIndicatorsId { get; set; }
        [DisplayName("Application Type")]
        public long ApplicationTypesId { get; set; }
        [DisplayName("Send For Approval Date")]
        public DateTime? SendForApprovalDate { get; set; }
        [DisplayName("Certificate Issued(Y/N)")]
        public string CertificateIssuedYn { get; set; }
        [DisplayName("Certificate Date")]
        public DateTime? CertificateDate { get; set; }
        [DisplayName("Certificate No")]
        public string CertificateNo { get; set; }
        [DisplayName("Reissue Date")]
        public DateTime? ReissueDate { get; set; }
        [DisplayName("ETQE")]
        public long EtqeId { get; set; }

      
        public virtual AccreditationStatuses AccreditationStatuses { get; set; }

        public virtual Evaluators Evaluators { get; set; }
   
        public virtual ProcessIndicators ProcessIndicators { get; set; }

        public virtual ApplicationTypes ApplicationTypes { get; set; }
        public virtual Etqe Etqe { get; set; }
        public virtual Person Person { get; set; }
    }
}