using System.ComponentModel;

namespace learner_portal.DTO
{
    public class OfoMinorDTO
    {
        public long Id { get; set; }
        [DisplayName("OFO Minor Code")]
        public string OfoMinorCode { get; set; }
        [DisplayName("OFO Minor Title")]
        public string OfoMinorTitle { get; set; }
        [DisplayName("Financial Year")]
        public string FinancialYearDesc { get; set; }
    }
}