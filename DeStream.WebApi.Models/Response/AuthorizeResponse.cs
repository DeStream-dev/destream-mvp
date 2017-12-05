using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models.Response
{
    public class AuthorizeResponse
    {
        public ApplicationApiUser User { get; set; }
    }

    public class ApplicationApiUser
    {
        public string Username { get; set; }
        public string Uid { get; set; }
    }
}
