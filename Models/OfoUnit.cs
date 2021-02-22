using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace learner_portal.Models
{
    public partial class OfoUnit
    {
        public OfoUnit()
        {
            Ofos = new HashSet<Ofo>();
        }
        [Key]
        public long OfoUnitId { get; set; }
        [DisplayName("OFO Unit Code")]
        public string OfoUnitCode { get; set; }
        [DisplayName("OFO Unit Title")]
        public string OfoUnitTitle { get; set; }
        [DisplayName("OFO Minor")]
        public long? OfoMinorId { get; set; }
        [DisplayName("Financial Year")]
        public long? FinancialYearId { get; set; }

        public virtual Financialyear Financialyear { get; set; }
        public virtual OfoMinor OfoMinor { get; set; }

        public virtual ICollection<Ofo> Ofos { get; set; }
    }
}
