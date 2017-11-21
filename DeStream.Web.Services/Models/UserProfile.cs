using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models
{
    public class UserProfile
    {
        public string DisplayName { get; set; }
        public string PurseNumber { get; set; }
        public string UserInfo { get; set; }
    }

    internal class UserProfileMappingProfile:Profile
    {
        public UserProfileMappingProfile()
        {
            CreateMap<Entities.Identity.UserProfile, UserProfile>();
            CreateMap<UserProfile, Entities.Identity.UserProfile>();
        }
    }
}
