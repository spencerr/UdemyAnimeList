using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UdemyAnimeList.Web.Features.Home
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult<Index.Model>> Retrieve()
            => Ok(await _mediator.Send(new Index.Query()));
    }
}
