

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class AddressTypeDetailsDTO
    {
        public long Id { get; set; }
        [DisplayName("Address Type Name")]
        [Required(ErrorMessage = "Please enter Name")]
        public string AddressTypeName { get; set; }
        [DisplayName("Address Type Code")]       
        [Required(ErrorMessage = "Please enter Code")]
        
        public string AddressTypeCode { get; set; }
    }
}