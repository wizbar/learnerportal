using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace learner_portal.Models
{
    // Add profile data for application users by adding properties to the Users class
    public class Users : IdentityUser
    {
        public string ActiveYn { get; set; }
        
        public virtual Person Person { get; set; }
    }
}