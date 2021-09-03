using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs;
using MediatR;
using Persistence;

namespace Application.Diffs
{
    public class Compare
    {
        public class Query : IRequest<Result<Tuple<DiffDto,DiffResultTypeDto>>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Tuple<DiffDto,DiffResultTypeDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Tuple<DiffDto,DiffResultTypeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var diff = await _context.Diffs.FindAsync(request.Id);

                if(diff.Left==null || diff.Right==null)
                {
                    return Result<Tuple<DiffDto,DiffResultTypeDto>>.Failure("Some of the values were not provided");
                }

                if(diff.Left.Length!=diff.Right.Length)
                {
                    var diffResultTypeDto = new DiffResultTypeDto()
                    {
                        DiffResultType="SizeDoNotMatch"
                    };

                    return Result<Tuple<DiffDto,DiffResultTypeDto>>.Success(new Tuple<DiffDto,DiffResultTypeDto>(null,diffResultTypeDto));
                }
                else if(string.Equals(diff.Left,diff.Right))
                {
                    var diffResultTypeDto = new DiffResultTypeDto()
                    {
                        DiffResultType="Equals"
                    };

                    return Result<Tuple<DiffDto,DiffResultTypeDto>>.Success(new Tuple<DiffDto,DiffResultTypeDto>(null,diffResultTypeDto));
                }
                else
                {
                    int lengthOfStrings=diff.Left.Length;
                    int charactersPerSection;
                    int numberOfSections = CalculateNumberOfSections(lengthOfStrings);
                    int startPointForSubstring;
                
                    if(lengthOfStrings%3==0)
                    {
                        charactersPerSection=3;
                        startPointForSubstring=lengthOfStrings-charactersPerSection;
                    }
                    else
                    {
                        charactersPerSection=lengthOfStrings%3;
                        startPointForSubstring=lengthOfStrings-charactersPerSection-1;
                    }

                    var diffs=new List<StatsDto>();
                    
                    for(int i=numberOfSections;i>=1;i--)
                    {
                        string leftSection=diff.Left.Substring(startPointForSubstring,lengthOfStrings-1);
                        string rightSection=diff.Right.Substring(startPointForSubstring,lengthOfStrings-1);

                        int sectionLength=leftSection.Length;
                        

                        // for(int j=0;j<sectionLength;j++)
                        // {
                        //     if(rightSection[j]==leftSection[j])
                        //     {
                        //         continue;
                        //     }
                        //     diffs.Add(new StatsDto
                        //     {
                        //         Offset=sectionLength-j,
                        //         Length=
                        //     });
                        // }
                    }

                    var diffDto=new DiffDto()
                    {
                        DiffResultType="ContentDoNotMatch",
                        Diffs=diffs
                    };

                    return Result<Tuple<DiffDto,DiffResultTypeDto>>.Success(new Tuple<DiffDto,DiffResultTypeDto>(diffDto,null));
                }
            }

            private int CalculateNumberOfSections(int lengthOfStrings)
            {
                return lengthOfStrings%3==0?lengthOfStrings/3:lengthOfStrings/3+1;
            }
        }
    }
}