using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Domain.Enums;
using UdemyAnimeList.Services.Storage;
using UdemyAnimeList.Web.Intrastructure;
using DbAnime = UdemyAnimeList.Domain.Models.Anime;

namespace UdemyAnimeList.Web.Features.Anime
{
    public class Create 
    {
        public class Command : IRequest<Guid>
        {
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

            [Display(Name = "Icon")]
            [FileType(new[] { ".jpg", ".png" })]
            public IFormFile Image { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBucketStorage _bucketStorage;

            public CommandHandler(ApplicationDbContext context, IMapper mapper, IBucketStorage bucketStorage)
            {
                _context = context;
                _mapper = mapper;
                _bucketStorage = bucketStorage;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var anime = _mapper.Map<DbAnime>(request);
                _context.Add(anime);

                await _context.SaveChangesAsync();

                if (request.Image != null)
                {
                    var success = await _bucketStorage.Put(request.Image, $"images/icons/{anime.Id}");
                    if (success)
                    {
                        anime.ImageUrl = $"images/icons/{anime.Id}";
                        await _context.SaveChangesAsync();
                    }
                }

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
                CreateMap<Command, DbAnime>();
            }
        }
    }
}
