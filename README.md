# 🏢 Customer Management API

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org/)

API RESTful para gerenciamento de clientes, desenvolvida como **desafio técnico** demonstrando domínio de arquitetura de software, boas práticas e padrões de projeto.

---

## 📋 Sobre o Projeto

Sistema de cadastro e gerenciamento de clientes com validação completa de documentos (CPF/CNPJ), seguindo os princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **CQRS**.

### Funcionalidades Implementadas

| Funcionalidade | Status | Descrição |
|----------------|--------|-----------|
| Cadastro de Clientes | ✅ | Criação de novos clientes com validação |
| Busca por ID | ✅ | Consulta de cliente específico |
| Validação de CPF | ✅ | Validação completa com dígitos verificadores |
| Validação de CNPJ | ✅ | Validação completa com dígitos verificadores |
| Documento Único | ✅ | Verificação de duplicidade no banco |
| Ativação/Desativação | ✅ | Controle de status do cliente |

---

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa)

```
┌─────────────────────────────────────────────────────────────────┐
│                         API (Controllers)                       │
│                    Apresentação e Contratos                     │
├─────────────────────────────────────────────────────────────────┤
│                      APPLICATION                                │
│            Commands, Queries, Handlers, Mediator                │
├─────────────────────────────────────────────────────────────────┤
│                         DOMAIN                                  │
│     Entities, Value Objects, Interfaces, Business Rules         │
├─────────────────────────────────────────────────────────────────┤
│                     INFRASTRUCTURE                              │
│          Repositories, ORM Mappings, External Services          │
└─────────────────────────────────────────────────────────────────┘
```

### Fluxo de uma Requisição

```
HTTP Request → Controller → Mediator → Handler → Repository → Database
                                ↓
                          Domain Entity
                          (Validações)
```

### Camadas e Responsabilidades

| Camada | Projeto | Responsabilidade |
|--------|---------|------------------|
| **Apresentação** | `CustomerManagement.Api` | Controllers REST, DTOs de entrada/saída, configuração da aplicação |
| **Aplicação** | `CustomerManagement.Application` | Orquestração de casos de uso, Commands, Queries, Handlers, Mediator |
| **Domínio** | `CustomerManagement.Domain` | Entidades, Value Objects, regras de negócio, interfaces de repositório |
| **Infraestrutura** | `CustomerManagement.Infrastructure` | Implementação de repositórios, mapeamento ORM, acesso a banco |

---

## 🎨 Padrões de Projeto Utilizados

### 1. Mediator Pattern

**O que é?** Padrão comportamental que encapsula como um conjunto de objetos interage, promovendo acoplamento fraco.

**Por que usar?**
- Controllers não precisam conhecer os Handlers diretamente
- Facilita adicionar comportamentos cross-cutting (logging, validação, cache)
- Simplifica a injeção de dependência nos controllers

**Estrutura do Mediator:**
```
CustomerManagement.Application/
└── Mediator/
    ├── IRequest.cs              # Interface base para Commands/Queries
    ├── IRequestHandler.cs       # Interface para Handlers
    ├── IMediator.cs             # Contrato do Mediator
    ├── Mediator.cs              # Implementação com resolução via DI
    └── ServiceCollectionExtensions.cs  # Registro automático de handlers
```

### 2. CQRS (Command Query Responsibility Segregation)

**O que é?** Separação entre operações de leitura (Queries) e escrita (Commands).

**Por que usar?**
- Cada operação pode ser otimizada independentemente
- Facilita escalabilidade (leituras podem ir para réplicas)
- Código mais organizado e com responsabilidade única

**Implementação:**
```
Application/
├── Commands/           # Operações de escrita
│   └── CreateCustomerCommand.cs
├── Queries/            # Operações de leitura
│   └── GetCustomerByIdQuery.cs
└── Handlers/           # Processam Commands e Queries
    ├── CreateCustomerCommandHandler.cs
    └── GetCustomerByIdQueryHandler.cs
```

### 3. Repository Pattern

**O que é?** Abstração sobre a camada de persistência de dados.

**Por que usar?**
- Domínio não conhece detalhes de infraestrutura (SQL, ORM)
- Facilita testes unitários com mocks
- Centraliza lógica de acesso a dados

### 4. Value Object Pattern (DDD)

**O que é?** Objetos imutáveis definidos por seus atributos, sem identidade própria.

**Por que usar?**
- Encapsula validação e comportamento
- Garante consistência (sempre válido após criação)
- Facilita comparação por valor

### 5. Factory Method

**O que é?** Método estático que encapsula a criação de objetos.

**Por que usar?**
- Validação antes da criação do objeto
- Objeto sempre criado em estado válido
- Semântica clara no código

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **.NET** | 9.0 | Versão mais recente com melhorias de performance e novos recursos |
| **ASP.NET Core** | 9.0 | Framework robusto para APIs REST com excelente ecossistema |
| **NHibernate** | 5.5 | ORM maduro com suporte avançado a Value Objects e mapeamentos complexos |
| **FluentNHibernate** | 3.0 | Mapeamento fluente e type-safe, mais legível que XML |
| **PostgreSQL** | 15+ | Banco open source, robusto, com excelente performance |
| **xUnit** | 2.9 | Framework de testes moderno e extensível |
| **Moq** | 4.20 | Biblioteca de mocking para testes isolados |
| **Docker** | - | Containerização do banco de dados para ambiente de desenvolvimento |

---

## 📁 Estrutura do Projeto

```
CustomerManagement/
│
├── 📂 CustomerManagement.Api/              # Camada de Apresentação
│   ├── 📂 Controllers/
│   │   └── CustomerController.cs           # Endpoints REST
│   ├── 📂 Contracts/
│   │   ├── 📂 Requests/                    # DTOs de entrada da API
│   │   └── 📂 Response/                    # DTOs de saída da API
│   ├── 📂 Extensions/
│   │   └── DependencyInjection.cs          # Configuração de DI
│   ├── Program.cs                          # Entry point e configurações
│   ├── appsettings.json                    # Configurações da aplicação
│   └── docker-compose.yml                  # PostgreSQL containerizado
│
├── 📂 CustomerManagement.Application/      # Camada de Aplicação
│   ├── 📂 Customer/
│   │   ├── 📂 Commands/
│   │   │   └── CreateCustomerCommand.cs    # Command de criação
│   │   ├── 📂 Queries/
│   │   │   └── GetCustomerByIdQuery.cs     # Query de consulta
│   │   ├── 📂 Handlers/
│   │   │   ├── CreateCustomerCommandHandler.cs
│   │   │   └── GetCustomerByIdQueryHandler.cs
│   │   ├── 📂 DTO/
│   │   │   ├── CreateCustomerResultDTO.cs
│   │   │   └── GetCustomerByIdResultDTO.cs
│   │   └── 📂 Interface/                   # Interfaces legadas
│   └── 📂 Mediator/                        # Implementação própria do Mediator
│       ├── IMediator.cs
│       ├── Mediator.cs
│       ├── IRequest.cs
│       ├── IRequestHandler.cs
│       └── ServiceCollectionExtensions.cs
│
├── 📂 CustomerManagement.Domain/           # Camada de Domínio (Core)
│   ├── 📂 Entities/
│   │   └── CustomerEntity.cs               # Entidade principal
│   ├── 📂 ValueObjects/
│   │   └── DocumentNumber.cs               # Value Object para CPF/CNPJ
│   ├── 📂 Enums/
│   │   └── DocumentType.cs                 # CPF ou CNPJ
│   ├── 📂 Exceptions/
│   │   └── DomainException.cs              # Exceções de domínio
│   └── 📂 Interface/
│       └── 📂 Repositories/
│           └── ICustomerRepository.cs      # Contrato do repositório
│
├── 📂 CustomerManagement.Infrastructure/   # Camada de Infraestrutura
│   ├── 📂 Persistence/
│   │   ├── NHibernateSessionFactory.cs     # Configuração do NHibernate
│   │   └── 📂 Maps/
│   │       └── CustomerMap.cs              # Mapeamento ORM
│   └── 📂 Repositories/
│       └── CustomerRepository.cs           # Implementação do repositório
│
└── 📂 CustomerManagement.Tests/            # Testes Unitários
|   ├── 📂 Application/
│   |   └── CreateCustomerCommandHandlerTests.cs
│   |    └── GetCustomerByIdQueryHandlerTests.cs |
|   └── 📂 Domain/
│   |   └── CustomerEntityTests.cs
│   |   └── DocumentNumberTests.cs 
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (para o PostgreSQL)
- [Git](https://git-scm.com/)

### 1. Clone o Repositório

```bash
git clone https://github.com/LopesRebecca/CustomerManagement.git
cd CustomerManagement
```

### 2. Inicie o Banco de Dados

```bash
cd CustomerManagement.Api
docker-compose up -d
```

Isso irá criar um container PostgreSQL com as seguintes configurações:
- **Host**: localhost
- **Porta**: 5432
- **Database**: clientes
- **Usuário**: clientes_user
- **Senha**: Maria123!

### 3. Execute a Aplicação

```bash
# Na raiz do projeto
dotnet restore
dotnet build
dotnet run --project CustomerManagement.Api
```

### 4. Acesse o Swagger

Abra no navegador: **http://localhost:5000/swagger**

---

## 📡 Endpoints da API

### Resumo dos Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/clientes` | Cadastrar novo cliente |
| `GET` | `/api/clientes/{id}` | Buscar cliente por ID |

### POST /api/clientes

Cadastra um novo cliente no sistema.

**Request Body:**
```json
{
  "name": "Empresa Exemplo LTDA",
  "document": "11.444.777/0001-61"
}
```

**Sucesso (200 OK):**
```json
{
  "sucess": true,
  "message": "Cadastro realizado com sucesso!",
  "customerId": 1
}
```

**Erro - Documento Duplicado (400 Bad Request):**
```json
{
  "error": "Documento já cadastrado."
}
```

**Erro - Documento Inválido (400 Bad Request):**
```json
{
  "error": "CPF inválido."
}
```

### GET /api/clientes/{id}

Busca um cliente pelo ID.

**Sucesso (200 OK):**
```json
{
  "id": 1,
  "name": "Empresa Exemplo LTDA",
  "document": "11.444.777/0001-61",
  "isActive": true
}
```

**Erro - Não Encontrado (404 Not Found):**
```
Cliente com o Id informado não encontrado
```

---

## 🧪 Testes

### Executando os Testes

```bash
# Executar todos os testes
dotnet test

# Com detalhes
dotnet test --verbosity normal

# Com cobertura de código
dotnet test --collect:"XPlat Code Coverage"
```

### Estrutura de Testes

```
CustomerManagement.Tests/
├── Domain/
│   ├── DocumentNumberTests.cs     # Testes de validação CPF/CNPJ
│   └── CustomerEntityTests.cs     # Testes da entidade
└── Application/
    └── CreateCustomerHandlerTests.cs  # Testes do handler
```

## 📝 Validações Implementadas

### Regras de Negócio

| Regra | Implementação | Localização |
|-------|---------------|-------------|
| CPF válido | Algoritmo oficial Receita Federal | `DocumentNumber.cs` |
| CNPJ válido | Algoritmo oficial Receita Federal | `DocumentNumber.cs` |
| Nome obrigatório | Mínimo 2 caracteres | `CustomerEntity.cs` |
| Documento único | Verificação no banco | `CreateCustomerCommandHandler.cs` |
| Cliente ativo por padrão | Flag `Active = true` na criação | `CustomerEntity.cs` |

---

## 👩‍💻 Autora

**Rebecca Lelis**

[![GitHub](https://img.shields.io/badge/GitHub-LopesRebecca-181717?style=flat&logo=github)](https://github.com/LopesRebecca)

---

⭐ **Desenvolvido como desafio técnico** - Demonstrando conhecimentos em Clean Architecture, DDD, CQRS e boas práticas de desenvolvimento.
