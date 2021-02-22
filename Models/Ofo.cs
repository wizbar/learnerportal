using System.ComponentModel;

#nullable disable

namespace learner_portal.Models
{
    public partial class Ofo
    {
        
        public long OfoId { get; set; } 
        [DisplayName("OFO Code")]
        public string OfoCode { get; set; }
        [DisplayName("OFO Title")]
        public string OfoTitle { get; set; }
        [DisplayName("OFO Unit")]
        public long? OfoUnitId { get; set; }
        [DisplayName("Financial Year")]
        public long? FinancialYearId { get; set; }
        
        public virtual OfoUnit OfoUnit { get; set; }
        public virtual Financialyear Financialyear { get; set; }
    }
}
