using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models.Request
{
    public class AuthorizeRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
