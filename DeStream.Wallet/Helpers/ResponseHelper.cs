using DeStream.Wallet.VM;
using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet.Helpers
{
    internal static class ResponseHelper
    {
        public static ErrorResponse ParseError(string responseContent)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
        }
    }
}
