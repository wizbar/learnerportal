using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace learner_portal.Models
{
    public partial class Financialyear
    {
        public Financialyear()
        { 
            Ofos = new HashSet<Ofo>();
            OfoUnit = new HashSet<OfoUnit>();
            OfoMinor = new HashSet<OfoMinor>();
        }

        [Key]
        public long FinancialyearId { get; set; }
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate { get; set; }
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndDate { get; set; }
        [DisplayName("Active For WSP?")]
        public string ActiveForWsp { get; set; }
        [DisplayName("Description")]
        public string FinancialyearDesc { get; set; }
        [DisplayName("Locked For WSP Submission?")]
        public string LockedForWspSubmission { get; set; }
        public string ActiveYn { get; set; }
        
        public virtual ICollection<Ofo> Ofos { get; set; }
        public virtual ICollection<OfoUnit> OfoUnit { get; set; }
        public virtual ICollection<OfoMinor> OfoMinor { get; set; }
    }
}
