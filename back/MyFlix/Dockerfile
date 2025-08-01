# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solution e arquivos de projeto
COPY MyFlix.sln ./
COPY MyFlix.Api/MyFlix.Api.csproj MyFlix.Api/
COPY MyFlix.Application/MyFlix.Application.csproj MyFlix.Application/
COPY MyFlix.Domain/MyFlix.Domain.csproj MyFlix.Domain/
COPY MyFlix.Infra/MyFlix.Infra.csproj MyFlix.Infra/
COPY MyFlix.Test/MyFlix.Test.csproj MyFlix.Test/

# Restaura dependências
RUN dotnet restore

# Copia todo o código
COPY . .

# Publica a aplicação
RUN dotnet publish MyFlix.Api/MyFlix.Api.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MyFlix.Api.dll"]
