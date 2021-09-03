using System.Threading;
using System.Threading.Tasks;
using Application.Core;
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
            public string Data { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var targetDiff = await _context.Diffs.FindAsync(request.Id);

                if (targetDiff == null)
                {
                    var diff = new Diff()
                    {
                        Id = request.Id,
                        Right = request.Data
                    };

                    _context.Diffs.Add(diff);

                    var res = await _context.SaveChangesAsync() > 0;

                    if (res)
                        return Result<Unit>.Success(Unit.Value);
                    return Result<Unit>.Failure("Falied to add the right diff");
                }

                targetDiff.Right = request.Data;

                var result = await _context.SaveChangesAsync() > 0;

                if (result)
                {
                    return Result<Unit>.Success(Unit.Value);
                }
                return Result<Unit>.Failure("Failed to edit a diff");
            }
        }
    }
}