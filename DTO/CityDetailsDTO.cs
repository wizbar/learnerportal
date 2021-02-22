using System.ComponentModel;

namespace learner_portal.DTO
{
    public class CityDetailsDTO
    {
        public long Id { get; set; }
        [DisplayName("City Name")]
        public string CityName { get; set; }
        [DisplayName("City Code")]
        public string CityCode { get; set; }
        [DisplayName("Province")]
        public string ProvinceName { get; set; }
    }
}