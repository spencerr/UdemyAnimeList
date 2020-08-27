using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyAnimeList.Web.Features.Animes
{
    public class AnimesController : Controller
    {
        private readonly IMediator _mediator;

        public AnimesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
            => View(await _mediator.Send(new Index.Query()));

        public async Task<IActionResult> View(View.Query query)
            => View(await _mediator.Send(query));

        public async Task<IActionResult> Edit(Edit.Query query)
            => View(await _mediator.Send(query));

        public IActionResult Create()
            => View(new Create.Command());

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command model)
        {
            await _mediator.Send(model);
            return RedirectToAction("View", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command model)
        {
            var id = await _mediator.Send(model);
            return CreatedAtAction("View", new { id });
        }
    }
}
