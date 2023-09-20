# Use a imagem base do SDK do .NET 7 para construir o aplicativo
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copie o arquivo csproj e restore das dependências
COPY *.csproj ./
RUN dotnet restore

# Copie todo o código-fonte e compile o aplicativo
COPY . ./
RUN dotnet publish -c Release -o out

# Use uma imagem leve como base para a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0

ENV DB_SERVER=sqlserver-gpm.database.windows.net \
    DB_PORT=1433 \
    DB_USER=elliot \
    DB_PASSWORD=Oi@12345 \
    DB_DATABASE=sqldb-teste-beneficios

WORKDIR /app
COPY --from=build-env /app/out .

# Comando de inicialização da aplicação
ENTRYPOINT ["dotnet", "watch", "run"]
