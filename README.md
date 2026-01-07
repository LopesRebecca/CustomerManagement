# 🏢 Customer Management API

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

API RESTful para gerenciamento de clientes, desenvolvida como **desafio técnico** demonstrando domínio de arquitetura de software, boas práticas e padrões de projeto.

---

## 📑 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Arquitetura](#-arquitetura)
- [Padrões de Projeto Utilizados](#-padrões-de-projeto-utilizados)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Como Executar](#-como-executar)
- [Endpoints da API](#-endpoints-da-api)
- [Decisões Técnicas](#-decisões-técnicas)
- [Testes](#-testes)
- [Screenshots e Demonstrações](#-screenshots-e-demonstrações)
- [Possíveis Evoluções](#-possíveis-evoluções)

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

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa), proposta por Robert C. Martin, onde as dependências apontam para dentro, mantendo o domínio isolado de frameworks e infraestrutura.

```
┌─────────────────────────────────────────────────────────────────┐
│                         API (Controllers)                        │
│                    Apresentação e Contratos                      │
├─────────────────────────────────────────────────────────────────┤
│                      APPLICATION                                 │
│            Commands, Queries, Handlers, Mediator                 │
├─────────────────────────────────────────────────────────────────┤
│                         DOMAIN                                   │
│     Entities, Value Objects, Interfaces, Business Rules          │
├─────────────────────────────────────────────────────────────────┤
│                     INFRASTRUCTURE                               │
│          Repositories, ORM Mappings, External Services           │
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

### Por que Clean Architecture?

1. **Independência de Frameworks**: O domínio não conhece ASP.NET, NHibernate ou qualquer tecnologia específica
2. **Testabilidade**: Cada camada pode ser testada isoladamente
3. **Flexibilidade**: Podemos trocar o banco de dados ou framework web sem afetar as regras de negócio
4. **Manutenibilidade**: Código organizado e com responsabilidades bem definidas

---

## 🎨 Padrões de Projeto Utilizados

### 1. Mediator Pattern

**O que é?** Padrão comportamental que encapsula como um conjunto de objetos interage, promovendo acoplamento fraco.

**Por que usar?**
- Controllers não precisam conhecer os Handlers diretamente
- Facilita adicionar comportamentos cross-cutting (logging, validação, cache)
- Simplifica a injeção de dependência nos controllers

**Implementação própria:**
```csharp
// Controller injeta apenas o Mediator
public CustomerController(IMediator mediator)
{
    _mediator = mediator;
}

// Envio simples de commands/queries
var result = await _mediator.Send(command, cancellationToken);
```

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

**Implementação:**
```csharp
// Interface no Domain (não conhece NHibernate)
public interface ICustomerRepository
{
    Task<CustomerEntity?> GetByIdAsync(int id);
    Task CreateAsync(CustomerEntity customer, CancellationToken ct);
    Task<bool> ExistDocumentNumberAsync(DocumentNumber document, CancellationToken ct);
}

// Implementação na Infrastructure (conhece NHibernate)
public class CustomerRepository : ICustomerRepository { ... }
```

### 4. Value Object Pattern (DDD)

**O que é?** Objetos imutáveis definidos por seus atributos, sem identidade própria.

**Por que usar?**
- Encapsula validação e comportamento
- Garante consistência (sempre válido após criação)
- Facilita comparação por valor

**Implementação - DocumentNumber:**
```csharp
public sealed class DocumentNumber : IEquatable<DocumentNumber>
{
    public string Value { get; }
    public DocumentType Type { get; }
    
    private DocumentNumber(string value, DocumentType type) { ... }
    
    public static DocumentNumber Create(string input)
    {
        // Valida CPF ou CNPJ
        // Lança DomainException se inválido
    }
}
```

### 5. Factory Method

**O que é?** Método estático que encapsula a criação de objetos.

**Por que usar?**
- Validação antes da criação do objeto
- Objeto sempre criado em estado válido
- Semântica clara no código

**Exemplo:**
```csharp
// Não permite criar documento inválido
var document = DocumentNumber.Create("123.456.789-09"); // Valida na criação

// Result pattern para retornos
var result = CreateCustomerResultDTO.Ok(clientId);
var result = CreateCustomerResultDTO.Failed("Erro de validação");
```

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
    └── UnitTest1.cs
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
- **Senha**: clientes_pass

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

## 🎯 Decisões Técnicas

### Por que NHibernate ao invés de Entity Framework?

| Critério | NHibernate | Entity Framework |
|----------|------------|------------------|
| **Mapeamento de Value Objects** | Suporte nativo e maduro | Requer configuração adicional |
| **Controle de SQL** | Alto controle sobre queries geradas | Abstração mais alta |
| **Lazy Loading** | Configuração granular | Configuração global |
| **Maturidade** | 20+ anos, muito estável | Mais moderno, menos features avançadas |

**Decisão**: NHibernate foi escolhido por seu suporte superior a Value Objects, essencial para DDD.

### Por que PostgreSQL ao invés de SQL Server?

| Critério | PostgreSQL | SQL Server |
|----------|------------|------------|
| **Custo** | Open source, gratuito | Licenciamento caro em produção |
| **Performance** | Excelente para CRUD | Similar |
| **Docker** | Imagem leve (~150MB) | Imagem pesada (~1.5GB) |
| **Tipos de dados** | JSONB, Arrays nativos | Suporte básico |

**Decisão**: PostgreSQL por ser gratuito, leve e com excelente performance.

### Por que Mediator próprio ao invés de MediatR?

| Critério | Mediator Próprio | MediatR |
|----------|------------------|---------|
| **Controle** | Total sobre implementação | Caixa preta |
| **Aprendizado** | Demonstra conhecimento do padrão | Usa biblioteca pronta |
| **Dependências** | Nenhuma adicional | Pacote NuGet |
| **Flexibilidade** | Customização total | Extensível via behaviors |

**Decisão**: Implementação própria para demonstrar conhecimento do padrão e ter controle total.

### Por que CQRS?

1. **Separação Clara**: Commands (escrita) vs Queries (leitura) bem definidos
2. **Escalabilidade Futura**: Permite otimizar leituras independentemente
3. **Single Responsibility**: Cada handler faz apenas uma coisa
4. **Testabilidade**: Handlers isolados são fáceis de testar

### Validação de Documentos

As validações de CPF e CNPJ seguem o algoritmo oficial da Receita Federal:

**CPF:**
- 11 dígitos numéricos
- Rejeita sequências repetidas (111.111.111-11)
- Valida 1º dígito verificador (peso 10→2)
- Valida 2º dígito verificador (peso 11→2)

**CNPJ:**
- 14 dígitos numéricos
- Rejeita sequências repetidas
- Valida 1º dígito verificador (peso 5,4,3,2,9,8,7,6,5,4,3,2)
- Valida 2º dígito verificador (peso 6,5,4,3,2,9,8,7,6,5,4,3,2)

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

### Exemplos de Testes

```csharp
[Fact]
public void Create_WithValidCPF_ShouldReturnDocumentNumber()
{
    var document = DocumentNumber.Create("529.982.247-25");
    
    Assert.Equal(DocumentType.CPF, document.Type);
}

[Fact]
public void Create_WithInvalidCPF_ShouldThrowDomainException()
{
    Assert.Throws<DomainException>(() => 
        DocumentNumber.Create("111.111.111-11"));
}
```

---

## 📸 Screenshots e Demonstrações

> ⚠️ **Instruções**: Substitua os placeholders abaixo pelas imagens reais dos testes.
> 
> Para adicionar as imagens:
> 1. Crie a pasta `docs/images/` na raiz do projeto
> 2. Salve as screenshots com os nomes indicados
> 3. As imagens aparecerão automaticamente no README

### Swagger UI

<!-- Adicione aqui a imagem do Swagger -->
<!-- ![Swagger UI](docs/images/swagger-ui.png) -->

`📷 Adicionar: docs/images/swagger-ui.png`

---

### Cadastro de Cliente - Sucesso

<!-- Adicione aqui a imagem do cadastro com sucesso -->
<!-- ![Cadastro Sucesso](docs/images/post-success.png) -->

`📷 Adicionar: docs/images/post-success.png`

---

### Cadastro de Cliente - Erro de Validação (CPF/CNPJ Inválido)

<!-- Adicione aqui a imagem do erro de validação -->
<!-- ![Erro Validação](docs/images/post-validation-error.png) -->

`📷 Adicionar: docs/images/post-validation-error.png`

---

### Cadastro de Cliente - Documento Duplicado

<!-- Adicione aqui a imagem do erro de documento duplicado -->
<!-- ![Documento Duplicado](docs/images/post-duplicate-error.png) -->

`📷 Adicionar: docs/images/post-duplicate-error.png`

---

### Busca por ID - Sucesso

<!-- Adicione aqui a imagem da busca por ID -->
<!-- ![Busca por ID](docs/images/get-success.png) -->

`📷 Adicionar: docs/images/get-success.png`

---

### Busca por ID - Não Encontrado

<!-- Adicione aqui a imagem do cliente não encontrado -->
<!-- ![Não Encontrado](docs/images/get-not-found.png) -->

`📷 Adicionar: docs/images/get-not-found.png`

---

### Testes Unitários Passando

<!-- Adicione aqui a imagem dos testes passando -->
<!-- ![Testes](docs/images/tests-passing.png) -->

`📷 Adicionar: docs/images/tests-passing.png`

---

### Banco de Dados (PostgreSQL)

<!-- Adicione aqui a imagem do banco com dados -->
<!-- ![Database](docs/images/database.png) -->

`📷 Adicionar: docs/images/database.png`

---

## 🔄 Possíveis Evoluções

| Funcionalidade | Prioridade | Descrição |
|----------------|------------|-----------|
| Listagem Paginada | Alta | Endpoint para listar clientes com paginação |
| Atualização de Cliente | Alta | Endpoint PUT para atualizar dados |
| Soft Delete | Média | Exclusão lógica ao invés de física |
| Autenticação JWT | Média | Segurança com tokens JWT |
| Validation Pipeline | Média | FluentValidation integrado ao Mediator |
| Cache com Redis | Baixa | Cache distribuído para consultas |
| Health Checks | Baixa | Endpoints de monitoramento |
| Logging Estruturado | Baixa | Serilog com Seq ou Elasticsearch |
| Rate Limiting | Baixa | Proteção contra abuso da API |

---

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

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

⭐ **Desenvolvido como desafio técnico** - Demonstrando conhecimentos em Clean Architecture, DDD, CQRS e boas práticas de desenvolvimento.
