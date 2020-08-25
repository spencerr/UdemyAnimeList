using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Data;
using UdemyAnimeList.Data.Enums;

namespace UdemyAnimeList.Web.Features.Anime
{
    public class Edit
    {
        public class Query : IRequest<Model>
        {
            public Guid Id { get; set; }
        }

        public class Model
        {
            public Guid Id { get; set; }
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

        public class Command : IRequest<Guid>
        {
            public Guid Id { get; set; }
            public Guid? SeasonId { get; set; }
            public string JapaneseName { get; set; }
            public string EnglishName { get; set; }
            public string Synopsys { get; set; }
            public string Background { get; set; }
            public string Source { get; set; }
            public int? EpisodeCount { get; set; }

            public DateTime? StartAirDate { get; set; }
            public DateTime? EndAirDate { get; set; }
            public DateTime? BroadcastTime { get; set; }

            public ShowType ShowType { get; set; }
            public Rating Rating { get; set; }
        }

        public class CommandHandler : IRequest<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var anime = await _context.Animes.FindAsync(request.Id);
                _mapper.Map(request, anime);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
