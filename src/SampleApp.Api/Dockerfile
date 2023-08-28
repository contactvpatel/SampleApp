#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SampleApp.Api/SampleApp.Api.csproj", "src/SampleApp.Api/"]
COPY ["src/SampleApp.Application/SampleApp.Application.csproj", "src/SampleApp.Application/"]
COPY ["src/SampleApp.Domain/SampleApp.Domain.csproj", "src/SampleApp.Domain/"]
COPY ["src/SampleApp.Infrastructure/SampleApp.Infrastructure.csproj", "src/SampleApp.Infrastructure/"]
RUN dotnet restore "src/SampleApp.Api/SampleApp.Api.csproj"
COPY . .
WORKDIR "/src/src/SampleApp.Api"
RUN dotnet build "SampleApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleApp.Api.dll"]