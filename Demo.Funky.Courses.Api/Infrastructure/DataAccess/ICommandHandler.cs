namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public interface ICommand
{
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Aff<string> ExecuteAsync(TCommand command);
}