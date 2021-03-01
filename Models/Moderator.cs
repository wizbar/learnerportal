using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Moderator
    {
        [Key]
        public long ModeratorId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime AccredStartDate { get; set; }
        public DateTime AccredEndDate { get; set; }
        public long AccreditationStatusId { get; set; }
        public string RegistrationNo { get; set; }
        public string EvaluatorsId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string ApprovedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; } 
        public long ProcessIndicatorsId { get; set; } 
        public long ApplicationTypesId { get; set; }
        public DateTime? SendForApprovalDate { get; set; }
        public string CertificateIssuedYn { get; set; }
        public DateTime? CertificateDate { get; set; }
        public string CertificateNo { get; set; }
        public DateTime? ReissueDate { get; set; }
        public long EtqeId { get; set; }
        
        public virtual AccreditationStatuses AccreditationStatuses { get; set; }
        public virtual Evaluator Evaluator { get; set; }
        public virtual ProcessIndicators ProcessIndicators { get; set; }
        public virtual ApplicationType ApplicationType { get; set; }
        public virtual Etqe Etqe { get; set; }
        public virtual Person Person { get; set; }

    }
}
