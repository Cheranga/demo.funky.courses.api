FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Funky.Courses.Api/Demo.Funky.Courses.Api.csproj", "Demo.Funky.Courses.Api/"]
RUN dotnet restore "Demo.Funky.Courses.Api/Demo.Funky.Courses.Api.csproj"
COPY . .
WORKDIR "/src/Demo.Funky.Courses.Api"
RUN dotnet build "Demo.Funky.Courses.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Funky.Courses.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Funky.Courses.Api.dll"]
