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
using UdemyAnimeList.Domain;
using UdemyAnimeList.Domain.Common;
using UdemyAnimeList.Domain.Enums;
using UdemyAnimeList.Domain.Models;
using UdemyAnimeList.Services.Cache;

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
                private readonly IConfigurationCache _configurationCache;

                public QueryHandler(ApplicationDbContext context, IMapper mapper, IConfigurationCache configurationCache)
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
                        .OrderBy(x => x)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(5).ToRankedListAsync();

                    var topUpcomingAnime = await _context.Animes
                        .Where(x => x.SeasonId == nextSeason)
                        .OrderBy(x => x)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(5).ToRankedListAsync();

                    var mostPopularAnime = await _context.Animes
                        .OrderBy(x => x)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(5).ToRankedListAsync();

                    var currentSeasonAnime = await _context.Animes
                        .Where(x => x.SeasonId == currentSeason)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(15).ToListAsync();

                    var recentlyUpdatedAnime = await _context.Animes
                        .OrderBy(x => x)
                        .ProjectTo<Model.Anime>(_mapper.ConfigurationProvider)
                        .Take(15).ToListAsync();

                    return new Model
                    {
                        TopAiringAnime = topAiringAnime,
                        TopUpcomingAnime = topUpcomingAnime,
                        MostPopularAnime = mostPopularAnime,
                        CurrentSeasonAnime = currentSeasonAnime,
                        RecentlyUpdatedAnime = recentlyUpdatedAnime,
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
            public IEnumerable<Anime> RecentlyUpdatedAnime { get; set; }


            public class Anime : IRankable
            {
                public Guid Id { get; set; }
                public int Rank { get; set; }
                public string Name { get; set; }
                public ShowType ShowType { get; set; }
                public int EpisodeCount { get; set; }
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
