namespace Demo.Funky.Courses.Api.Features.Shared;

public class ErrorCodes
{
    public const int CommandError = 666;
    public const int InvalidRequest = 400;
    public const int CourseNotFound = 404;
    public const int DataAccessError = 500;
    public const int CreateCourseDataAccessError = 501;
}

public class ErrorMessages
{
    public const string CourseNotFound = "course not found";
    public const string DataAccessError = "error occurred when accessing data store";
    public const string CreateCourseDataAccessError = "error occurred when storing a new course in the data store";
}