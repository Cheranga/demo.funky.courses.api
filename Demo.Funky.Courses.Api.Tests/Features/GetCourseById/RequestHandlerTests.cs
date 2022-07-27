using Demo.Funky.Courses.Api.Features.GetCourseById;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using static LanguageExt.Prelude;

namespace Demo.Funky.Courses.Api.Tests.Features.GetCourseById;

public class RequestHandlerTests
{
    [Fact]
    public async Task CourseAvailable()
    {
        var courseDataModel = new CourseDataModel
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = "C#"
        };
        var mockedQueryHandler = new Mock<IQueryHandler<Query, CourseDataModel>>();
        mockedQueryHandler.Setup(x => x.GetAsync(It.IsAny<Query>()))
            .Returns(LanguageExt.Aff<CourseDataModel>.Success(courseDataModel));

        var handler = new RequestHandler(mockedQueryHandler.Object, Mock.Of<ILogger<RequestHandler>>());
        (await handler.Handle(new Request(courseDataModel.Id), CancellationToken.None))
            .Match(
                response =>
                {
                    response.Id.Should().Be(courseDataModel.Id);
                    response.Name.Should().Be(courseDataModel.Name);
                },
                error => error.Should().BeNull());
    }

    [Fact]
    public async Task CourseUnavailable()
    {
        var courseDataModel = new CourseDataModel();
        var mockedQueryHandler = new Mock<IQueryHandler<Query, CourseDataModel>>();
        mockedQueryHandler.Setup(x => x.GetAsync(It.IsAny<Query>()))
            .Returns(LanguageExt.Aff<CourseDataModel>.Success(courseDataModel));

        var handler = new RequestHandler(mockedQueryHandler.Object, Mock.Of<ILogger<RequestHandler>>());
        (await handler.Handle(new Request("C#"), CancellationToken.None))
            .Match(
                response => response.Should().BeNull(),
                error => error.Code.Should().Be(ErrorCodes.CourseNotFound));
    }

    [Fact]
    public async Task DataAccessError()
    {
        var mockedQueryHandler = new Mock<IQueryHandler<Query, CourseDataModel>>();
        mockedQueryHandler.Setup(x => x.GetAsync(It.IsAny<Query>()))
            .Returns(
                TryAsync<CourseDataModel>(() => throw new Exception("data access error")).ToAff());

        var handler = new RequestHandler(mockedQueryHandler.Object, Mock.Of<ILogger<RequestHandler>>());
        (await handler.Handle(new Request("C#"), CancellationToken.None))
            .Match(
                response => response.Should().BeNull(),
                error => error.Code.Should().Be(ErrorCodes.DataAccessError));
    }
}