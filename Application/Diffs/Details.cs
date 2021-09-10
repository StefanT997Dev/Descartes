using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Diffs
{
    public class Details
    {
        public class Query : IRequest<Result<Tuple<DiffDto, DiffResultTypeDto>>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Tuple<DiffDto, DiffResultTypeDto>>>
        {
            private readonly IDiffRepository _repository;
            public Handler(IDiffRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Tuple<DiffDto, DiffResultTypeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var diff = await _repository.GetDiffAsync(request.Id);

                if(diff==null)
                {
                    return Result<Tuple<DiffDto, DiffResultTypeDto>>.Failure("Diff not found");
                }

                if (diff.Left == null || diff.Right == null)
                {
                    return Result<Tuple<DiffDto, DiffResultTypeDto>>.Failure("Some of the values were not provided");
                }

                if (diff.Left.Length != diff.Right.Length)
                {
                    var diffResultTypeDto = new DiffResultTypeDto()
                    {
                        DiffResultType = "SizeDoNotMatch"
                    };

                    return Result<Tuple<DiffDto, DiffResultTypeDto>>.Success(new Tuple<DiffDto, DiffResultTypeDto>(null, diffResultTypeDto));
                }
                else if (string.Equals(diff.Left, diff.Right))
                {
                    var diffResultTypeDto = new DiffResultTypeDto()
                    {
                        DiffResultType = "Equals"
                    };

                    return Result<Tuple<DiffDto, DiffResultTypeDto>>.Success(new Tuple<DiffDto, DiffResultTypeDto>(null, diffResultTypeDto));
                }
                else
                {
                    var diffs = new List<StatsDto>();

                    for(int i=0;i<diff.Left.Length;i++)
                    {
                        if(diff.Left[i]!=diff.Right[i])
                        {
                            var statsDto=new StatsDto()
                            {
                                Offset=i,
                                Length=1
                            };
                            diffs.Add(statsDto);
                        }
                    }

                    var diffDto = new DiffDto()
                    {
                        DiffResultType = "ContentDoNotMatch",
                        Diffs = diffs
                    };

                    return Result<Tuple<DiffDto, DiffResultTypeDto>>.Success(new Tuple<DiffDto, DiffResultTypeDto>(diffDto, null));
                }
            }
        }
    }
}