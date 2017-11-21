using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Identity
{
    public class ApplicationUser:IdentityUser
    {
        //public int? UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public virtual ICollection<UserTarget> UserTargets { get; set; }

    }
}
