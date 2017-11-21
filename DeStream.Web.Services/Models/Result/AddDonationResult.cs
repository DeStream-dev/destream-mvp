using DeStream.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models.Result
{
    public class AddDonationResult
    {
        public OperationResultType Status { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
