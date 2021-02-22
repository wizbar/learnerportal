using System;
using System.ComponentModel;

namespace learner_portal.DTO
{
    public class DocumentTypesDetailsDTO
    {
        public Guid Id { get; set; }
        [DisplayName("Type Name")]
        public string TypeName { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Role")]
        public string RoleName { get; set; }
        [DisplayName("Active?")]
        public string ActiveYn { get; set; }
    }
} 