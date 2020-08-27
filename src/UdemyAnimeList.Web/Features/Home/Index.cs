using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Data;
using UdemyAnimeList.Data.Common;
using UdemyAnimeList.Data.Enums;
using UdemyAnimeList.Data.Models;
using UdemyAnimeList.Web.Intrastructure.Services;

namespace UdemyAnimeList.Web.Features.Home
{
    public class Index
    {
        public class Query : IRequest<Model>
        {
            public class QueryHandler : IRequestHandler<Query, Model>
            {
                private readonly ApplicationDbContext _context;
                private readonly IMapper _mapper;
                private readonly ConfigurationCache _configurationCache;

                public QueryHandler(ApplicationDbContext context, IMapper mapper, ConfigurationCache configurationCache)
                {
                    _context = context;
                    _mapper = mapper;
                    _configurationCache = configurationCache;
                }

                public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
                {
                    var currentSeason = await _configurationCache.Get<Guid>(CacheKeys.CurrentSeasonKey);
                    var nextSeason = await _configurationCache.Get<Guid>(CacheKeys.NextSeasonKey);

                    var topAiringAnime = await _context.Animes
                        .Where(x => x.SeasonId == currentSeason)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(5).ToListAsync();

                    var topUpcomingAnime = await _context.Animes
                        .Where(x => x.SeasonId == nextSeason)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(5).ToListAsync();

                    var mostPopularAnime = await _context.Animes
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(5).ToListAsync();

                    return new Model
                    {
                        TopAiringAnime = topAiringAnime,
                        TopUpcomingAnime = topUpcomingAnime,
                        MostPopularAnime = mostPopularAnime
                    };
                }
            }
        }

        public class Model
        {
            public IEnumerable<Anime> TopAiringAnime { get; set; }
            public IEnumerable<Anime> TopUpcomingAnime { get; set; }
            public IEnumerable<Anime> MostPopularAnime { get; set; }
            public IEnumerable<Anime> CurrentSeasonAnime { get; set; }


            public class Anime
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public ShowType ShowType { get; set; }
                public int Episodes { get; set; }
                public decimal Score { get; set; }
                public int MembersLiked { get; set; }
                public string ImageUrl { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Anime, Model.Anime>();
            }
        }
    }
}
