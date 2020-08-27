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
using UdemyAnimeList.Data.Enums;

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

                public QueryHandler(ApplicationDbContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
                {
                    var currentSeason = Guid.Empty;
                    var nextSeason = Guid.Empty;

                    var topAiringAnime = await _context.Animes
                        .Where(x => x.SeasonId == currentSeason)
                        .ProjectTo<Model.PopularAnime>(_mapper.ConfigurationProvider)
                        .Take(5).ToListAsync();

                    var topUpcomingAnime = await _context.Animes
                        .Where(x => x.SeasonId == nextSeason)
                        .ProjectTo<Model.PopularAnime>(_mapper.ConfigurationProvider)
                        .Take(5).ToListAsync();

                    var mostPopularAnime = await _context.Animes
                        .ProjectTo<Model.PopularAnime>(_mapper.ConfigurationProvider)
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
            public IEnumerable<PopularAnime> TopAiringAnime { get; set; }

            public IEnumerable<PopularAnime> TopUpcomingAnime { get; set; }

            public IEnumerable<PopularAnime> MostPopularAnime { get; set; }


            public class PopularAnime
            {
                public string Name { get; set; }
                public ShowType ShowType { get; set; }
                public int Episodes { get; set; }
                public decimal Score { get; set; }
                public int MembersLiked { get; set; }
            }
        }
    }
}
