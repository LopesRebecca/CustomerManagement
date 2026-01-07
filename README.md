# 🏢 Customer Management API

API RESTful para gerenciamento de clientes, desenvolvida como desafio técnico.

## 📋 Sobre o Projeto

Sistema de cadastro e gerenciamento de clientes com validação de documentos (CPF/CNPJ), seguindo os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**.

## 🏗️ Arquitetura

O projeto está organizado em camadas, seguindo os princípios de separação de responsabilidades:

```
CustomerManagement/
├── CustomerManagement.Api/           # Camada de Apresentação (Controllers, Contracts)
├── CustomerManagement.Application/   # Camada de Aplicação (Handlers, Commands, Queries)
├── CustomerManagement.Domain/        # Camada de Domínio (Entities, Value Objects, Interfaces)
├── CustomerManagement.Infrastructure/# Camada de Infraestrutura (Repositories, Persistence)
└── CustomerManagement.Tests/         # Testes Unitários
```

### Camadas

| Camada | Responsabilidade |
|--------|------------------|
| **API** | Exposição dos endpoints REST, validação de entrada, transformação de DTOs |
| **Application** | Orquestração de casos de uso, handlers de comandos e queries |
| **Domain** | Regras de negócio, entidades, value objects, interfaces de repositório |
| **Infrastructure** | Implementação de persistência, mapeamentos ORM, acesso a dados |

## 🛠️ Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **ASP.NET Core** - Web API
- **NHibernate + FluentNHibernate** - ORM para mapeamento objeto-relacional
- **PostgreSQL** - Banco de dados relacional
- **xUnit** - Framework de testes
- **Moq** - Biblioteca de mocking para testes

## ✨ Funcionalidades

- ✅ Cadastro de clientes
- ✅ Validação completa de CPF (com dígitos verificadores)
- ✅ Validação completa de CNPJ (com dígitos verificadores)
- ✅ Verificação de documento duplicado
- ✅ Ativação/Desativação de clientes

## 🚀 Como Executar

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (opcional, para o banco de dados)

### Configurando o Banco de Dados

1. **Usando Docker** (recomendado):

```bash
cd CustomerManagement.Api
docker-compose up -d
```

2. **Ou configure manualmente** o PostgreSQL e atualize a connection string em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=clientes;Username=clientes_user;Password=clientes_pass"
  }
}
```

### Executando a Aplicação

```bash
# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build

# Executar a API
dotnet run --project CustomerManagement.Api
```

A API estará disponível em: `https://localhost:5001` ou `http://localhost:5000`

### Executando os Testes

```bash
dotnet test
```

## 📡 Endpoints da API

### Clientes

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/clientes` | Cadastrar novo cliente |

### Exemplo de Requisição

**POST /api/clientes**

```json
{
  "name": "Empresa Exemplo LTDA",
  "document": "11.444.777/0001-61"
}
```

**Resposta de Sucesso (201 Created):**

```json
{
  "success": true,
  "error": null,
  "clientId": 1
}
```

**Resposta de Erro (400 Bad Request):**

```json
{
  "error": "Documento já cadastrado."
}
```

## 🧪 Testes

O projeto inclui testes unitários para:

- **DocumentNumber** - Validação de CPF e CNPJ
- **ClientEntity** - Regras de negócio da entidade
- **CreateClientHandler** - Casos de uso de criação

```bash
# Executar testes com detalhes
dotnet test --verbosity normal

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📁 Estrutura de Pastas Detalhada

```
CustomerManagement/
├── CustomerManagement.Api/
│   ├── Controllers/          # Controllers da API
│   ├── Contracts/
│   │   ├── Requests/         # DTOs de entrada
│   │   └── Responses/        # DTOs de saída
│   ├── Extensions/           # Extension methods para DI
│   └── Program.cs            # Configuração da aplicação
│
├── CustomerManagement.Application/
│   ├── Commands/
│   │   ├── Request/          # Command objects
│   │   └── Response/         # Response objects
│   ├── Handlers/             # Command handlers
│   └── Queries/              # Query handlers
│
├── CustomerManagement.Domain/
│   ├── Entities/             # Entidades de domínio
│   ├── ValueObjects/         # Value Objects (DocumentNumber)
│   ├── Enums/                # Enumerações
│   ├── Exceptions/           # Exceções de domínio
│   └── Interface/
│       └── Repositories/     # Interfaces de repositório
│
├── CustomerManagement.Infrastructure/
│   ├── Persistence/
│   │   └── Maps/             # Mapeamentos NHibernate
│   └── Repositories/         # Implementações dos repositórios
│
└── CustomerManagement.Tests/
    ├── Domain/               # Testes de domínio
    └── Application/          # Testes de aplicação
```

## 🎯 Decisões Técnicas

### Por que NHibernate?

- Suporte maduro a mapeamento de Value Objects
- Controle fino sobre queries SQL
- Lazy loading configurável
- Ampla compatibilidade com bancos de dados

### Por que PostgreSQL?

- Open source e gratuito
- Performance excelente para operações CRUD
- Suporte a tipos de dados avançados
- Amplamente utilizado em produção

### Value Objects

O `DocumentNumber` foi implementado como Value Object para:
- Encapsular a validação de CPF/CNPJ
- Garantir imutabilidade
- Facilitar comparações de igualdade
- Centralizar lógica de formatação

## 📝 Validações Implementadas

### CPF
- Verifica quantidade de dígitos (11)
- Rejeita CPFs com todos os dígitos iguais
- Valida os dois dígitos verificadores

### CNPJ
- Verifica quantidade de dígitos (14)
- Rejeita CNPJs com todos os dígitos iguais
- Valida os dois dígitos verificadores

### Cliente
- Nome obrigatório (2-200 caracteres)
- Documento único no sistema

## 🔄 Possíveis Evoluções

- [ ] Implementar busca de cliente por ID
- [ ] Adicionar listagem paginada de clientes
- [ ] Implementar atualização de dados do cliente
- [ ] Adicionar autenticação JWT
- [ ] Implementar soft delete
- [ ] Adicionar cache com Redis
- [ ] Implementar health checks
- [ ] Adicionar logging estruturado com Serilog

## 👩‍💻 Autora

**Rebecca Lelis**

---

⭐ Desenvolvido como desafio técnico
