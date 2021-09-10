using System;
using Application.Core;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class BaseApiController:ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
        
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if(!result.IsSuccess && result.Error=="Data from request is null")
                return BadRequest();
            if(result.IsSuccess)
                return Created("~v1/diff/{id}",result);
            return NotFound(result.Error);
        }

        protected ActionResult HandleTupleResult<T,P>(Result<Tuple<T,P>> result)
        {
            if(!result.IsSuccess)
                return NotFound();
            return Ok(result.Value);
        }
    }
}