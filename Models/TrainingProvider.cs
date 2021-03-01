using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class TrainingProvider
    {
        [Key]
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string TrainingCode { get; set; }
        public long ProvinceId { get; set; }
        public DateTime? AccredStartDate { get; set; }
        public DateTime? AccredEndDate { get; set; }
        public string AccreditationNo { get; set; }
        public long AccreditationStatusId { get; set; }
        public long EtqeId { get; set; }
        public string RegisteredWithDoe { get; set; }
        public DateTime? DoeRegStartDate { get; set; }
        public DateTime? DoeRegEndDate { get; set; }
        public string DoeRegNo { get; set; }
        public long? BeeRatingId { get; set; }
        public decimal? BeeRecognition { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelNo { get; set; }
        public string ContactFaxNo { get; set; }
        public string ContactCellNo { get; set; }
        public long NoFullTimeStaff { get; set; }
        public long NoContractStaff { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public long? SetaId { get; set; }
        public string CompanyRegNo { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string ApprovedBy { get; set; }
        public string EvaluatorsId { get; set; }
        public long ProcessIndicatorId { get; set; }
        public string MouRequestLetter { get; set; }
        public long ApplicationTypesId { get; set; }
        public string TradingName { get; set; }
        public long LongitudeDegree { get; set; }
        public long LongitudeMinute { get; set; }
        public decimal LongitudeSecond { get; set; }
        public long LatitudeDegree { get; set; }
        public long LatitudeMinute { get; set; }
        public decimal LatitudeSecond { get; set; }
        public string CertificateIssuedYn { get; set; }
        public DateTime? CertificateDate { get; set; }
        public string CertificateNo { get; set; }
        public DateTime? ReissueDate { get; set; }
        public long? ProgrammeTypeId { get; set; }

        public virtual Province Province { get; set; }
        public virtual AccreditationStatuses AccreditationStatuses { get; set; }
        public virtual Etqe Etqe { get; set; }
        public virtual ProcessIndicators ProcessIndicators { get; set; }
        public virtual Evaluator Evaluator { get; set; }
        public virtual ApplicationType ApplicationType { get; set; }
        public virtual BbbeeRating BbbeeRating { get; set; }
        public virtual Seta Seta { get; set; }
        public virtual ProgrammeType ProgrammeType { get; set; }
    }
}
