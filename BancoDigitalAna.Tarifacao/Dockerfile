# # See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# # This stage is used when running from VS in fast mode (Default for Debug configuration)
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER $APP_UID
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081


# # This stage is used to build the service project
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["BancoDigitalAna.Conta.csproj", "BancoDigitalAna.Conta/"]
# RUN dotnet restore "./BancoDigitalAna.Conta/BancoDigitalAna.Conta.csproj"
# COPY . .
# WORKDIR "/src/BancoDigitalAna.Conta"
# RUN dotnet build "./BancoDigitalAna.Conta.csproj" -c $BUILD_CONFIGURATION -o /app/build

# # This stage is used to publish the service project to be copied to the final stage
# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./BancoDigitalAna.Conta.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "BancoDigitalAna.Conta.dll"]

# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projetos
COPY ./BancoDigitalAna.Tarifacao/BancoDigitalAna.Tarifacao.csproj ./BancoDigitalAna.Tarifacao/
COPY ./BancoDigitalAna.BuildingBlocks/BancoDigitalAna.BuildingBlocks.csproj ./BancoDigitalAna.BuildingBlocks/
# Se houver mais projetos compartilhados, adicione-os aqui

# Restaura as dependências
RUN dotnet restore ./BancoDigitalAna.Tarifacao/BancoDigitalAna.Tarifacao.csproj

# Copia o restante do código
COPY . .

# Publica
RUN dotnet publish ./BancoDigitalAna.Tarifacao/BancoDigitalAna.Tarifacao.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BancoDigitalAna.Tarifacao.dll"]