using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence;

namespace Application.Diffs
{
    public class CreateRight
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public DiffModelDto RightDiff { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IDiffRepository _repository;
            public Handler(IDiffRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if(request.RightDiff.Data==null)
                {
                    return Result<Unit>.Failure("Data from request is null");
                }

                var targetDiff = await _repository.GetDiffAsync(request.Id);

                if (targetDiff == null)
                {
                    var diff = new Diff()
                    {
                        Id = request.Id,
                        Right = request.RightDiff.Data
                    };

                    var res = await _repository.CreateDiffAsync(diff);

                    if (res)
                        return Result<Unit>.Success(Unit.Value);
                    return Result<Unit>.Failure("Falied to add the right diff");
                }

                var result = await _repository.UpdateRightDiffAsync(targetDiff,request.RightDiff.Data);

                if (result)
                {
                    return Result<Unit>.Success(Unit.Value);
                }
                return Result<Unit>.Failure("Failed to edit a diff");
            }
        }
    }
}