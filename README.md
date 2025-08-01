
# MyFlix - Plataforma Pessoal de Filmes

---

## Rodando com Docker Compose (Recomendado)

### Pré-requisitos
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Executando a aplicação completa
```bash
# Acesse a pasta raiz do repositorio
# Suba o docker compose
docker compose up 
```

## Rodando localmente

###  Backend (.NET 8 API)

####  Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### ️# Rodando o backend
```bash
# Acesse a pasta do backend
cd back/Myflix

# Restaurar dependências
dotnet restore

# Rodar o projeto
dotnet dotnet run --project MyFlix.Api/MyFlix.Api.csproj
```
---

###  Frontend (Next.js)

####  Pré-requisitos
- [Node.js 19+](https://nodejs.org/en/download)
- [npm](https://www.npmjs.com/) ou [yarn](https://yarnpkg.com/)

####  Rodando o frontend
```bash
# Acesse a pasta do frontend
cd front

# Instalar dependências
npm install

# Rodar o projeto
npm run dev
```
O frontend será iniciado em: `http://localhost:3000`.

---

## Tecnologias utilizadas

Linguagens: C#(.NET 8)

Frameworks: EF (Entity Framework)

Abordagens: code-first, minimal apis, dependency injection, programação assíncrona

Bancos de dados: SQLite

Interfaces / APIs: RESTful

Conteinerização: Docker, Docker compose

Arquitetura: DDD

Padrões de Projeto: Repository, Unit of Work, DTO, Validator 

Validação: FluentValidation 

Documentação: Swagger

Teste: xUnit

Frontend: React 19, Next.js 15, Typescript

---

## Endpoints do backend
| verbo  | Rota | Descrição | Retornos |
|--------| --- | --- | --- |
| GET    | `/Filme` | Lista todos os filmes | 200 |
| POST   | `/Filme` | Cria novo filme | 201 / 400 |
| PUT    | `/Filme/{id}` | Atualiza filme existente | 204 / 400 / 404 |
| PUT    | `/Filme/Nota/{id}` | Adiciona/atualiza nota (1-5) | 204 / 400 / 404 |
| DELETE | `/Filme/{id}` | Remove um filme | 204 / 400 / 404 |


Feito para o teste técnico MyFlix :)
