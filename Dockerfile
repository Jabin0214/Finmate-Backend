# -------------------- Build stage --------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    COPY . .
    RUN dotnet restore "./api.csproj"
    RUN dotnet publish "./api.csproj" -c Release -o /app/publish
    
    # -------------------- Runtime stage --------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    COPY --from=build /app/publish .
    
    EXPOSE 80
    
    # 👇 保证 dotnet 命令有效
    ENTRYPOINT ["dotnet", "api.dll"]