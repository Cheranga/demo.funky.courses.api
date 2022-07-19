﻿namespace Demo.Funky.Courses.Api.Features.Shared;

public class ErrorCodes
{
    public const int CommandError = 666;
    public const int CourseNotFound = 404;
    public const int DataAccessError = 500;
}

public class ErrorMessages
{
    public const string CourseNotFound = "course not found";
    public const string DataAccessError = "error occurred when accessing data store";
}