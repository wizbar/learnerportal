using System;
using System.Collections.Generic;

#nullable disable

namespace learner_portal.Models
{
    public partial class Qualification
    {
        public long QualificationId { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationTitle { get; set; }
        public long NqfLevelId { get; set; }
        public int MinimumCredits { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public long? EtqeId { get; set; }
        public long? SubFrameworkId { get; set; }
        public string Purpose { get; set; }
        public string LearningAssumedInPlace { get; set; }
        public string RecognizePrevLearning { get; set; }
        public string RulesOfCombination { get; set; }
        public string EloacOutcome { get; set; }
        public string EloacAssessmentCriteria { get; set; }
        public string IntlBenchmarkingMemo { get; set; }
        public string ArticulationOptions { get; set; }
        public string ModerationOptions { get; set; }
        public string AssessorCriteria { get; set; }
        public string Notes { get; set; }
        public long? RegistrationStatusId { get; set; }
        public string SaqaDecisionNo { get; set; }
        public int TransitionPeriod { get; set; }
        public int TrainOutPeriod { get; set; }
        public DateTime LastAchievementDate { get; set; }
        public DateTime LastEnrolmentDate { get; set; }
        public string IsLearningProgramme { get; set; }
        public DateTime? SaqaDownloadDate { get; set; }
        public long? QualificationClassId { get; set; }
        public long? LpQualificationId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int MinCoreCredits { get; set; }
        public int MinCundamentalCredits { get; set; }
        public int MinElectiveCredits { get; set; }
        public string DisplayOnSiteYn { get; set; }
        public string TrainingMaterialYn { get; set; }
        public string TradeYn { get; set; }
        public string MinimumCredits1 { get; set; }
        public long? QualificationTypesId { get; set; }
        public string RegistrationEndDate1 { get; set; }
        public string RegistrationStartDate1 { get; set; }
    }
}
