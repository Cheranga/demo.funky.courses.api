namespace Demo.Funky.Courses.Api.Features.Shared;

public class GetCourseResponse
{
    public string Id { get; }
    public string Name { get; }

    public GetCourseResponse(string id, string name)
    {
        Id = id;
        Name = name;
    }
}