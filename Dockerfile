# ================================================
# STAGE 1: Build project
# ================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ project file và restore package
COPY ["ShopPC.csproj", "./"]
RUN dotnet restore "./ShopPC.csproj"

# Copy tất cả source code còn lại và build
COPY . .
RUN dotnet publish "./ShopPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ================================================
# STAGE 2: Run project
# ================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Mở port container
EXPOSE 8080
EXPOSE 5000

# Khởi chạy app
ENTRYPOINT ["dotnet", "ShopPC.dll"]
