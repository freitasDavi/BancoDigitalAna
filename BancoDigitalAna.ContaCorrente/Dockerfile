# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projetos
COPY ./BancoDigitalAna.ContaCorrente/BancoDigitalAna.Conta.csproj ./BancoDigitalAna.ContaCorrente/
COPY ./BancoDigitalAna.BuildingBlocks/BancoDigitalAna.BuildingBlocks.csproj ./BancoDigitalAna.BuildingBlocks/
# Se houver mais projetos compartilhados, adicione-os aqui

# Restaura as dependências
RUN dotnet restore ./BancoDigitalAna.ContaCorrente/BancoDigitalAna.Conta.csproj

# Copia o restante do código
COPY . .

# Publica
RUN dotnet publish ./BancoDigitalAna.ContaCorrente/BancoDigitalAna.Conta.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BancoDigitalAna.Conta.dll"]