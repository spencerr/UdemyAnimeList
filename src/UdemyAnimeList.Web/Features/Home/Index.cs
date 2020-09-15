using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Domain.Common;
using UdemyAnimeList.Domain.Enums;
using UdemyAnimeList.Domain.Models;
using UdemyAnimeList.Services.Amazon;
using UdemyAnimeList.Services.Cache;
using UdemyAnimeList.Web.Intrastructure;
using DbAnime = UdemyAnimeList.Domain.Models.Anime;

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

                    var season = _context.Seasons
                        .ProjectTo<Model.Season>(_mapper.ConfigurationProvider)
                        .FirstOrDefault(x => x.Id == currentSeason);

                    return new Model
                    {
                        CurrentSeason = season,
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
            public Season CurrentSeason { get; set; }
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

                [CdnUrl]
                public string ImageUrl { get; set; }
            }

            public class Season
            {
                public Guid Id { get; set; }
                public AiringSeason AiringSeason { get; set; }
                public short Year { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<DbAnime, Model.Anime>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.JapaneseName))
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? "/images/no-icon.svg"));

                CreateMap<Season, Model.Season>();
            }
        }
    }
}
