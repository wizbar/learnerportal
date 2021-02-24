using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace learner_portal.Models
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            Documents = new HashSet<Document>();
        }
        
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Type Name")]
        public string TypeName { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Role")]
        
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        [ForeignKey("Active(Y/N)")]
        public string ActiveYn { get; set; }
        [NotMapped]
        public bool IsActive { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")] 
        public DateTime? DateUpdated { get; set; }

        public virtual Roles Role { get; set; }
        
        public virtual ICollection<Document> Documents { get; set; }
    }
}
