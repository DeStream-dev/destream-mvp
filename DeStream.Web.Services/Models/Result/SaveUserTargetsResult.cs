using DeStream.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models.Result
{
    public class SaveUserTargetsResult
    {
        public OperationResultType Status { get; set; }
        public List<string> Errors { get; set; }
        public SaveUserTargetsResult()
        {
            Errors = new List<string>();
        }
    }
}
