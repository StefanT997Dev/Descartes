using System.Threading.Tasks;
using API.Contracts;
using Application.Diffs;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DiffController:BaseApiController
    {
        [HttpPatch(ApiRoutes.Diffs.AddLeft)]
        public async Task<IActionResult> AddLeft(int id, DiffModelDto diff)
        {
            return HandleResult(await Mediator.Send(new CreateLeft.Command{Id=id,LeftDiff=diff}));
        }

        [HttpPatch(ApiRoutes.Diffs.AddRight)]
        public async Task<IActionResult> AddRight(int id, DiffModelDto diff)
        {
            return HandleResult(await Mediator.Send(new CreateRight.Command{Id=id,RightDiff=diff}));
        }

        [HttpGet(ApiRoutes.Diffs.GetDiff)]
        public async Task<IActionResult> GetDiff(int id)
        {
            return HandleTupleResult(await Mediator.Send(new Details.Query{Id=id}));
        }
    }
}