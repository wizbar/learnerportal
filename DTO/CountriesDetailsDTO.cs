using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class CountriesDetailsDTO
    {
        public long Id { get; set; }
        [ Required  ]
        [DisplayName("Country Name")]
        public string CountryName { get; set; }
        [DisplayName("Country Code")]
        public string CountryCode { get; set; }
    }
}