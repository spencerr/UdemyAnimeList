using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Services.Amazon;

using DbAnime = UdemyAnimeList.Domain.Models.Anime;

namespace UdemyAnimeList.Web.Features.Anime
{
    public class Delete
    {
        public class Command : IRequest<Guid>
        {
            public Guid Id { get; set; }
        }

        public class CommandHandler : IRequest<Command>
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

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var anime = await _context.Animes.FindAsync(request.Id);

                if (!string.IsNullOrEmpty(anime.ImageUrl))
                {
                    await _s3.Remove(anime.ImageUrl);
                }

                _context.Remove(anime);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
