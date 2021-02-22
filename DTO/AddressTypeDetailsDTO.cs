using System.ComponentModel;

namespace learner_portal.DTO
{
    public class AddressTypeDetailsDTO
    {
        public long Id { get; set; }
        [DisplayName("Address Type Name")]
        public string AddressTypeName { get; set; }
        [DisplayName("Address Type Code")]
        public string AddressTypeCode { get; set; }
    }
}