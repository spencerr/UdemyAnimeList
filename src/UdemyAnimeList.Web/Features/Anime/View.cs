using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Domain.Enums;

using DbAnime = UdemyAnimeList.Domain.Models.Anime;

namespace UdemyAnimeList.Web.Features.Anime
{
    public class View
    {
        public class Query : IRequest<Model>
        {
            public Guid Id { get; set; }
        }

        public class Model
        {
            public Guid Id { get; set; }

            [Display(Name = "Airing Season")]
            public Guid? SeasonId { get; set; }

            [Display(Name = "Japanese Name")]
            public string JapaneseName { get; set; }

            [Display(Name = "English Name")]
            public string EnglishName { get; set; }
            public string Synopsys { get; set; }
            public string Background { get; set; }
            public string Source { get; set; }

            [Display(Name = "Number of Episodes")]
            public int? EpisodeCount { get; set; }

            [Display(Name = "Airing Start")]
            public DateTimeOffset? StartAirDate { get; set; }

            [Display(Name = "Airing End")]
            public DateTimeOffset? EndAirDate { get; set; }

            [Display(Name = "Broadcast Time")]
            public DateTimeOffset? BroadcastTime { get; set; }

            [Display(Name = "Show Type")]
            public ShowType ShowType { get; set; }

            [Display(Name = "TV Rating")]
            public TVRating TVRating { get; set; }

            public string ImageUrl { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Model>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Animes
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<DbAnime, Model>();
            }
        }
    }
}
