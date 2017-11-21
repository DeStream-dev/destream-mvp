using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models
{
    public class UserTarget:ModelWithName<long>
    {
        public decimal TargetRequiredTotal { get; set; }
        public string Code { get; set; }
    }

    internal class UserTargetMappingProfile:Profile
    {
        public UserTargetMappingProfile()
        {
            CreateMap<Entities.UserTarget, UserTarget>();
            CreateMap<UserTarget, Entities.UserTarget>();
        }
    }
}
