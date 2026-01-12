# ================================================
# STAGE 1: Build project
# ================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file và restore package trước để tận dụng layer caching
COPY ["ShopPC.csproj", "./"]
RUN dotnet restore "./ShopPC.csproj"

# Copy source code và build
COPY . .
RUN dotnet publish "./ShopPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ================================================
# STAGE 2: Run project  
# ================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Tạo user non-root cho security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

EXPOSE 8080

ENTRYPOINT ["dotnet", "ShopPC.dll"]
