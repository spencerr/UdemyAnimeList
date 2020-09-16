using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
using UdemyAnimeList.Web.Intrastructure;
using DbAnime = UdemyAnimeList.Domain.Models.Anime;

namespace UdemyAnimeList.Web.Features.Anime
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public Guid Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Animes
                    .ProjectTo<Command>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
            }
        }

        public class Command : IRequest<Guid>
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
            public TimeSpan? BroadcastTime { get; set; }

            [Display(Name = "Show Type")]
            public ShowType ShowType { get; set; }

            [Display(Name = "TV Rating")]
            public TVRating TVRating { get; set; }

            [Display(Name = "Icon")]
            [FileType(new[] { ".jpg", ".png" })]
            public IFormFile Image { get; set; }

            [CdnUrl]
            public string ImageUrl { get; set; }
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
                var anime = await _context.Animes.FindAsync(request.Id);
                _mapper.Map(request, anime);

                if (request.Image != null)
                {
                    var success = await _s3.Put(request.Image, $"images/icons/{anime.Id}");
                    if (success)
                    {
                        anime.ImageUrl = $"images/icons/{anime.Id}";
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                return anime.Id;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.JapaneseName).NotEmpty().When(x => string.IsNullOrEmpty(x.EnglishName)).WithMessage("A Japanese or English name is required.");
                RuleFor(x => x.EnglishName).NotEmpty().When(x => string.IsNullOrEmpty(x.JapaneseName)).WithMessage("A Japanese or English name is required.");
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, DbAnime>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
                CreateMap<DbAnime, Command>();
            }
        }
    }
}
