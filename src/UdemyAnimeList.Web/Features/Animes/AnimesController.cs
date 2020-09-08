﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UdemyAnimeList.Web.Features.Animes
{
    [ApiController]
    [Route("/api/anime")]
    public class AnimesController : Controller
    {
        private readonly IMediator _mediator;

        public AnimesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await _mediator.Send(new Index.Query()));
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<View.Model>> View([FromRoute] View.Query query)
            => Ok(await _mediator.Send(query));

        [HttpGet("{id:guid}/edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.Command>> Edit([FromRoute] Edit.Query query)
            => Ok(await _mediator.Send(query));

        [HttpPut("{id:guid}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit(Edit.Command model)
        {
            await _mediator.Send(model);
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Create.Command model)
        {
            var id = await _mediator.Send(model);
            return CreatedAtAction("View", new { id });
        }

        [HttpDelete("{id:guid}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Delete.Command model)
        {
            await _mediator.Send(model);
            return NoContent();
        }
    }
}
