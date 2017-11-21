using DeStream.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Identity
{
    public class UserProfile//:Entity<long>
    {
        [Required, MaxLength(Constants.DefaultStringMaxLength2)]
        public string DisplayName { get; set; }

        [Required, MaxLength(16)]
        public string PurseNumber { get; set; }

        [MaxLength(4000)]
        public string UserInfo { get; set; }

        [Key]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
