using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyAnimeList.Web.Intrastructure
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<DateTime, TimeSpan>().ConvertUsing(s => s.TimeOfDay);
            CreateMap<TimeSpan, DateTime>().ConvertUsing(s => DateTime.Today.AddTicks(s.Ticks));

            CreateMap<DateTimeOffset, TimeSpan>().ConvertUsing(s => s.TimeOfDay);
            CreateMap<TimeSpan, DateTimeOffset>().ConvertUsing(s => DateTime.Today.AddTicks(s.Ticks));

            CreateMap<DateTimeOffset?, TimeSpan?>().ConvertUsing(s => s == null ? (TimeSpan?)null : s.Value.TimeOfDay);
            CreateMap<TimeSpan?, DateTimeOffset?>().ConvertUsing(s => s == null ? (DateTimeOffset?)null : DateTime.Today.AddTicks(s.Value.Ticks));
        }
    }
}
