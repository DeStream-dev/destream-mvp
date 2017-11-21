using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models
{
    public class ListResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public ListResponse()
        {
            Items = new List<T>();
        }
    }
}
