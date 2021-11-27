
using AutoMapper;
using SecureApi.Models.WebEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Models.Mapping
{
	public class WeToEntityProfile : Profile
	{
		public WeToEntityProfile()
		{
			CreateMap<RegistrationWe, User>().ForMember(user => user.UserName, map => map.MapFrom(we => we.Email));
		}
	}
}
