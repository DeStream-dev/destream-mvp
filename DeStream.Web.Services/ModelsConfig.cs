using DeStream.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public static class ModelsConfig
    {
        public static void AddMappings(AutoMapper.IMapperConfigurationExpression config)
        {
            config.AddProfile<UserProfileMappingProfile>();
            config.AddProfile<UserTargetMappingProfile>();
        }

        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(AddMappings);
        }
    }
}
