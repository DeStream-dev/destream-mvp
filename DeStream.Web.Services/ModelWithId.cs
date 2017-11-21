using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public class ModelWithId<T>
    {
        public T Id { get; set; }
    }

    public class ModelWithName<T>:ModelWithId<T>
    {
        public string Name { get; set; }
    }
}
