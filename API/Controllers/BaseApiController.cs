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
            if(!result.IsSuccess)
                return NotFound();
            if(result.IsSuccess && result.Value!=null)
                return Created("~v1/diff/{id}",result);
            if(result.IsSuccess && result.Value==null)
                return NotFound();
            return BadRequest(result.Error);
        }

        protected ActionResult HandleTupleResult<T,P>(Result<Tuple<T,P>> result)
        {
            if(result.Value.Item1==null)
                return Ok(result.Value.Item2);
            if(result.Value.Item2==null)
                return Ok(result.Value.Item1);
            return NotFound();
        }
    }
}