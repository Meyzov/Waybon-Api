FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Waybon.Api/*.csproj Waybon.Api/
COPY Waybon.Application/*.csproj Waybon.Application/
COPY Waybon.Domain/*.csproj Waybon.Domain/
COPY Waybon.Infrastructure/*.csproj Waybon.Infrastructure/

RUN dotnet restore Waybon.Api/Waybon.Api.csproj

COPY . .

RUN dotnet publish Waybon.Api/Waybon.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

ENTRYPOINT ["dotnet", "Waybon.Api.dll"]
