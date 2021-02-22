using System.ComponentModel;

namespace learner_portal.DTO
{
    public class ProvinceDetailsDTO
    {
        public long Id { get; set; }
        [DisplayName("Province Name")]
        public string ProvinceName { get; set; }
        [DisplayName("Province Code")]
        public string ProvinceCode { get; set; }
        [DisplayName("Country Name")]
        public string CountryName { get; set; }
    }
}