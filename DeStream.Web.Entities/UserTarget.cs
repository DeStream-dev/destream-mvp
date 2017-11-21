using DeStream.Web.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities
{
    public class UserTarget:EntityWithName<long>
    {
        public decimal TargetRequiredTotal { get; set; }

        [MaxLength(16), Required, Index("IX_UserTargetCode",IsUnique =true,Order = 1)]
        public string Code { get; set; }

        [Index("IX_UserTargetCode", IsUnique = true, Order = 2)]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
