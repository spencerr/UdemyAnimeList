using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Data;
using UdemyAnimeList.Data.Enums;
using UdemyAnimeList.Data.Models;
using UdemyAnimeList.Web.Intrastructure.Services;

namespace UdemyAnimeList.Web.Features.Animes
{
    public class Create 
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

            }
        }

        public class Command : IRequest<Guid>
        {
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

            public IFormFile Image { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var anime = _mapper.Map<Anime>(request);
                _context.Add(anime);

                await _context.SaveChangesAsync();

                return anime.Id;
            }
        }

    }
}
