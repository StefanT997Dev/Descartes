using System.Threading;
using Application.Interfaces;
using AutoFixture;
using Moq;

namespace Application.UnitTests
{
    public class UnitTest
    {
        protected readonly Fixture _fixture;
        protected readonly Mock<IDiffRepository> _repositoryStub;
        protected readonly CancellationToken _cancellationToken;
        public UnitTest()
        {
            _repositoryStub=new Mock<IDiffRepository>();
            _fixture=new Fixture();
            _cancellationToken=new CancellationToken();
        }
    }
}