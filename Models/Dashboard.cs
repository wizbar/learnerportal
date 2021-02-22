using System.ComponentModel.DataAnnotations;

#nullable disable

namespace learner_portal.Models
{
    public class Dashboard
    {
        [Key]
        public long DashboardId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Department { get; set; }
    }
}