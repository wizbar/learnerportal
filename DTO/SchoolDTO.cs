using System.ComponentModel;

namespace learner_portal.DTO
{
    public class SchoolDTO
    {
        public long Id { get; set; }
        [DisplayName("School Code")]
        public string SchoolCode { get; set; }
        [DisplayName("EMIS No")]
        public string EmisNo { get; set; }
        [DisplayName("School Name")]
        public string SchoolName { get; set; }
    }
}
