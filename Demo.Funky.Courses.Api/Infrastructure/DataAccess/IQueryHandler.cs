using LanguageExt;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public interface IQuery
{
    
}

public interface IQueryHandler<in TQuery, TDataModel> where TQuery:IQuery where TDataModel:class
{
    Aff<TDataModel> GetAsync(TQuery query);
}