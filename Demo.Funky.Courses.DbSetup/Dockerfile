FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Funky.Courses.DbSetup/Demo.Funky.Courses.DbSetup.csproj", "Demo.Funky.Courses.DbSetup/"]
RUN dotnet restore "Demo.Funky.Courses.DbSetup/Demo.Funky.Courses.DbSetup.csproj"
COPY . .
WORKDIR "/src/Demo.Funky.Courses.DbSetup"
RUN dotnet build "Demo.Funky.Courses.DbSetup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Funky.Courses.DbSetup.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Funky.Courses.DbSetup.dll"]
