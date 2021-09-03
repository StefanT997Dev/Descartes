using System.Threading.Tasks;
using Application.Diffs;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DiffController:BaseApiController
    {
        [HttpPatch("{id}/left")]
        public async Task<IActionResult> AddLeft(int id, DiffModelDto diff)
        {
            return HandleResult(await Mediator.Send(new CreateLeft.Command{Id=id,LeftDiff=diff}));
        }

        [HttpPatch("{id}/right")]
        public async Task<IActionResult> AddRight(int id, string data)
        {
            return HandleResult(await Mediator.Send(new CreateRight.Command{Id=id,Data=data}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiff(int id)
        {
            return HandleTupleResult(await Mediator.Send(new Compare.Query{Id=id}));
        }
    }
}