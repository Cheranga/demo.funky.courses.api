﻿using Demo.Funky.Courses.Api.Features.GetCourseById;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using FluentAssertions;
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
        mockedQueryHandler.Setup(x => x.GetAsync(It.IsAny<Query>())).Returns(LanguageExt.Aff<CourseDataModel>.Success(courseDataModel));

        var handler = new RequestHandler(mockedQueryHandler.Object);
        (await handler.Handle(new Request("C#"), CancellationToken.None))
            .Match(
                Left: error => error.Should().BeNull(),
                Right: response =>
                {
                    response.Id.Should().Be(courseDataModel.Id);
                    response.Name.Should().Be(courseDataModel.Name);
                }
            );
    }
    
    [Fact]
    public async Task CourseUnavailable()
    {
        var courseDataModel = new CourseDataModel();
        var mockedQueryHandler = new Mock<IQueryHandler<Query, CourseDataModel>>();
        mockedQueryHandler.Setup(x => x.GetAsync(It.IsAny<Query>())).Returns(LanguageExt.Aff<CourseDataModel>.Success(courseDataModel));

        var handler = new RequestHandler(mockedQueryHandler.Object);
        (await handler.Handle(new Request("C#"), CancellationToken.None))
            .Match(
                Left:error => error.Code.Should().Be(ErrorCodes.CourseNotFound),
                Right:response => response.Should().BeNull()
            );
    }

    [Fact]
    public async Task DataAccessError()
    {
        var mockedQueryHandler = new Mock<IQueryHandler<Query, CourseDataModel>>();
        mockedQueryHandler.Setup(x => x.GetAsync(It.IsAny<Query>())).Returns(
            TryAsync<CourseDataModel>(() => throw new Exception("data access error")).ToAff());

        var handler = new RequestHandler(mockedQueryHandler.Object);
        (await handler.Handle(new Request("C#"), CancellationToken.None))
            .Match(
                Left:error => error.Code.Should().Be(ErrorCodes.DataAccessError),
                Right:response => response.Should().BeNull()
            );
    }
}