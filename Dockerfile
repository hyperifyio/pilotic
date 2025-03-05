FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file first
COPY *.sln .

# Copy and restore projects individually
COPY Pilotic.API/*.csproj ./Pilotic.API/
COPY Pilotic.Core/*.csproj ./Pilotic.Core/
COPY Pilotic.Domain/*.csproj ./Pilotic.Domain/
COPY Pilotic.Tests/*.csproj ./Pilotic.Tests/
COPY Pilotic.App/*.csproj ./Pilotic.App/

RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build solution
RUN dotnet build Pilotic.API/Pilotic.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Pilotic.API/Pilotic.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilotic.API.dll"]
