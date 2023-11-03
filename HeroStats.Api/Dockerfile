FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HeroStats.Api/HeroStats.Api.csproj", "HeroStats.Api/"]
RUN dotnet restore "HeroStats.Api/HeroStats.Api.csproj"
COPY . .
WORKDIR "/src/HeroStats.Api"
RUN dotnet build "HeroStats.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HeroStats.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeroStats.Api.dll"]
