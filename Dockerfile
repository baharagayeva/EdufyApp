# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# csproj faylları əvvəl copy olunur (cache üçün optimal)
COPY Edufy.API/*.csproj Edufy.API/
COPY Edufy.Application/*.csproj Edufy.Application/
COPY Edufy.Domain/*.csproj Edufy.Domain/
COPY Edufy.SqlServer/*.csproj Edufy.SqlServer/

# restore
RUN dotnet restore Edufy.API/Edufy.API.csproj

# bütün source kod
COPY . .

# publish
RUN dotnet publish Edufy.API/Edufy.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# container port
EXPOSE 8080

# ASP.NET bind
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Edufy.API.dll"]
