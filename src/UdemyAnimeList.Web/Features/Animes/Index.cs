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

namespace UdemyAnimeList.Web.Features.Animes
{
    public class Index
    {
        public class Query : IRequest<IEnumerable<Model>>
        {
            public class QueryHandler : IRequestHandler<Query, IEnumerable<Model>>
            {
                private readonly ApplicationDbContext _context;
                private readonly IMapper _mapper;

                public QueryHandler(ApplicationDbContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
                {
                    return await _context.Animes.ProjectTo<Model>(_mapper.ConfigurationProvider).ToListAsync();
                }
            }
        }

        public class Model
        {

        }
    }
}
