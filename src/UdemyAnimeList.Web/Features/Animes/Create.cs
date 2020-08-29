using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Domain.Enums;
using UdemyAnimeList.Domain.Models;
using UdemyAnimeList.Services.Amazon;

namespace UdemyAnimeList.Web.Features.Animes
{
    public class Create 
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.JapaneseName).NotEmpty().When(x => string.IsNullOrEmpty(x.EnglishName));
                RuleFor(x => x.EnglishName).NotEmpty().When(x => string.IsNullOrEmpty(x.JapaneseName));
                RuleFor(x => x.Image).NotNull().WithMessage("An Image is required.");
                
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
            public DateTimeOffset? BroadcastTime { get; set; }

            public ShowType ShowType { get; set; }
            public TVRating TVRating { get; set; }

            public IFormFile Image { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IAmazonS3Service _s3;

            public CommandHandler(ApplicationDbContext context, IMapper mapper, IAmazonS3Service s3)
            {
                _context = context;
                _mapper = mapper;
                _s3 = s3;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var anime = _mapper.Map<Anime>(request);
                _context.Add(anime);

                await _context.SaveChangesAsync();

                await _s3.Put(request.Image, anime.Id.ToString());

                return anime.Id;
            }
        }

    }
}
