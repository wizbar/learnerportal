using System.ComponentModel;

namespace learner_portal.DTO
{
    public class SuburbsDetailsDTO
    {
        public long Id { get; set; }
        [DisplayName("Suburb Name")]
        public string SuburbName { get; set; }
        [DisplayName("Suburb Code")]
        public string SuburbCode { get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
    }
}