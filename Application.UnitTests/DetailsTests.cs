using System.Threading;
using System.Threading.Tasks;
using Application.Diffs;
using Application.Interfaces;
using AutoFixture;
using Domain;
using Moq;
using Xunit;
using static Application.Diffs.Details;

namespace Application.UnitTests
{
    public class DetailsTests:UnitTest
    {
        private readonly Details.Handler _handler;
        private readonly Query _query;
        public DetailsTests()
        {
            _handler=new Details.Handler(_repositoryStub.Object);
            _query = _fixture.Create<Query>();
        }

        [Fact]
        public async Task Handle_RequestIdDoesNotExistInDb_ReturnsAppropriateErrorMessage()
        {
            // Arrange
            _repositoryStub
                .Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync((Diff)null);
            // Act
            var result = await _handler.Handle(_query,_cancellationToken);

            // Assert
            Assert.Equal("Diff not found",result.Error);
        }

        [Theory]
        [InlineData(null,"AAAAB","Some of the values were not provided")]
        [InlineData(null,null,"Some of the values were not provided")]
        [InlineData("AAAB",null,"Some of the values were not provided")]
        public async Task Handle_RequestDiffDoesNotExistInDb_ReturnsAppropriateErrorMessage(string left, string right, string errorMessage)
        {
            // Arrange
            var diff = new Diff()
            {
                Id=1,
                Left=left,
                Right=right
            };
            _repositoryStub
                .Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync(diff);

            // Act
            var result = await _handler.Handle(_query,_cancellationToken);

            // Assert
            Assert.Equal(errorMessage,result.Error);
        }

        [Theory]
        [InlineData("AAAABB","AAB","SizeDoNotMatch")]
        [InlineData("AAAABB","AAAABB","Equals")]
        public async Task Handle_ChecksLeftAndRightDiffDifference_ReturnsAppropriateMessage(string left, string right, string errorMessage)
        {
            // Arrange
            var diff = new Diff()
            {
                Id=1,
                Left=left,
                Right=right
            };
            _repositoryStub
                .Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync(diff);

            // Act
            var result = await _handler.Handle(_query,_cancellationToken);

            // Assert
            Assert.Equal(errorMessage,result.Value.Item2.DiffResultType);
        }

        [Fact]
        public async Task Handle_DiffsAreSameSizeButDifferent_ReturnsAppropriateMessage()
        {
            // Arrange
            var diff = new Diff()
            {
                Id=1,
                Left="AAAAAA==",
                Right="AQABAQ=="
            };
            _repositoryStub
                .Setup(repo => repo.GetDiffAsync(It.IsAny<int>()))
                .ReturnsAsync(diff);

            // Act
            var result = await _handler.Handle(_query,_cancellationToken);

            // Assert
            Assert.Equal("ContentDoNotMatch",result.Value.Item1.DiffResultType);
        }
    }
}