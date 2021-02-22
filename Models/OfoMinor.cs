using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace learner_portal.Models
{
    public partial class OfoMinor
    {
        public OfoMinor()
        {
            OfoUnit = new HashSet<OfoUnit>();
        }
        [Key]
        public long OfoMinorId { get; set; }
        [DisplayName("OFO Minor Code")]
        public string OfoMinorCode { get; set; }
        [DisplayName("OFO Minor Title")]
        public string OfoMinorTitle { get; set; }
        [DisplayName("OFO Sub Major")]
        public long? OfoSubMajorId { get; set; }
        [DisplayName("Financial Year")]
        public long? FinancialYearId { get; set; }

        public virtual Financialyear Financialyear { get; set; }
        public virtual ICollection<OfoUnit> OfoUnit { get; set; }
    }
}