using System.ComponentModel;

namespace learner_portal.DTO 
{
    public class OfoDTO
    {

        public long Id { get; set; } 
        [DisplayName("OFO Code")] 
        public string OfoCode { get; set; }
        [DisplayName("OFO Title")]
        public string OfoTitle { get; set; }


        [DisplayName("Description")]
        public string FinancialyearName { get; set; }

        [DisplayName("OFO Unit Title")]
        public string OfoUnitName { get; set; }
    }
}