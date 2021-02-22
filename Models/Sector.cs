using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace learner_portal.Models
{
    public partial class Sector
    {
        public Sector()
        {
            Job = new HashSet<Job>(); 
        }

        [Key]
        public long SectorId { get; set; }
        [DisplayName("Description")]
        public string SectorDesc { get; set; }
        [DisplayName("Active(Y/N)")]
        public string ActiveYn { get; set; }
        public virtual ICollection<Job> Job { get; set; }
    }
}