using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Diffs;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;
using static Application.Diffs.CreateLeft;

namespace Application.UnitTests
{
    public class CreateLeftTests
    {
        [Fact]
        public async Task Handle_DiffByRequestIdDoesNotExist_CreateANewDiff()
        {
            // Arrange
            var repositoryStub = new Mock<IDiffRepository>();
            repositoryStub.Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync((Diff)null);
            var commandStub = new Mock<Command>();
            var cancellationToken = new CancellationToken();
            var resultResponse = new Result<Unit>()
            {
                IsSuccess=true
            };

            var handler = new CreateLeft.Handler(repositoryStub.Object);

            // Act
            var result = await handler.Handle(commandStub.Object,cancellationToken);
            
            // Assert
            Assert.Equal(resultResponse.IsSuccess,result.IsSuccess);
        }
    }
}
