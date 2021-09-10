using System.Threading;
using System.Threading.Tasks;
using Application.Diffs;
using Application.DTOs;
using Application.Interfaces;
using AutoFixture;
using Domain;
using Moq;
using Xunit;
using static Application.Diffs.CreateLeft;

namespace Application.UnitTests
{
    public class CreateLeftTests:UnitTest
    {
        private readonly Command _command;
        public CreateLeftTests()
        {
            _command = _fixture.Create<Command>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_AttemptingToCreateANewDiff_ReturnsAppropriateResponse(bool isCreated)
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync((Diff)null);
            _repositoryStub.Setup(repo => repo.CreateDiffAsync(It.IsAny<Diff>()))
            .ReturnsAsync(isCreated);

            var handler = new CreateLeft.Handler(_repositoryStub.Object);

            // Act
            var result = await handler.Handle(_command, _cancellationToken);

            // Assert
            Assert.Equal(isCreated,result.IsSuccess);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_AttemptingToUpdateADiff_ReturnsAppropriateResponse(bool isUpdated)
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Create<Diff>());
            _repositoryStub.Setup(repo => repo.UpdateLeftDiffAsync(It.IsAny<Diff>(),It.IsAny<string>()))
                .ReturnsAsync(isUpdated);

            var handler = new CreateLeft.Handler(_repositoryStub.Object);

            // Act
            var result = await handler.Handle(_command, _cancellationToken);

            // Assert
            Assert.Equal(isUpdated,result.IsSuccess);
        }

        [Fact]
        public async Task Handle_DataSentIsNull_ReturnsABadRequest()
        {
            // Arrange
            var handler = new CreateLeft.Handler(_repositoryStub.Object);
            var diff = new DiffModelDto();
            var command = new Command();
            command.Id=1;
            command.LeftDiff=diff;
            command.LeftDiff.Data=null;

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.Equal(false,result.IsSuccess);
        }
    }
}
