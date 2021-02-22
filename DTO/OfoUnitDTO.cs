using System.ComponentModel;

namespace learner_portal.DTO
{
    public class OfoUnitDTO
    {
        public long Id { get; set; }
        [DisplayName("OFO Unit Code")]
        public string OfoUnitCode { get; set; }

        [DisplayName("OFO Unit Title")]
        public string OfoUnitTitle { get; set; }

        [DisplayName("OFO Minor")]
        public string OfoMinorTitle { get; set; }
        [DisplayName("Financial Year")]
        public string FinancialYearDesc { get; set; }
    }
}