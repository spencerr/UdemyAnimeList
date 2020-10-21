using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Services.Storage;

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
            private readonly IBucketStorage _bucketStorage;

            public CommandHandler(ApplicationDbContext context, IMapper mapper, IBucketStorage bucketStorage)
            {
                _context = context;
                _mapper = mapper;
                _bucketStorage = bucketStorage;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var anime = await _context.Animes.FindAsync(request.Id);

                if (!string.IsNullOrEmpty(anime.ImageUrl))
                {
                    await _bucketStorage.Remove(anime.ImageUrl);
                }

                _context.Remove(anime);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
