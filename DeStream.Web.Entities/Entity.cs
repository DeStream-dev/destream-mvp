using DeStream.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities
{
    public class Entity<T>
    {
        public T Id { get; set; }
    }

    public class EntityWithName<T>:Entity<T>
    {
        [Required, MaxLength(Constants.MediumStringMaxLength)]
        public string Name { get; set; }
    }
}
