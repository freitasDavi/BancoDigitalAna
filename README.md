# 🏦 Banco Digital Ana

Um sistema de banco digital desenvolvido com arquitetura **DDD (Domain-Driven Design)**, **CQRS (Command Query Responsibility Segregation)** e comunicação assíncrona com **Apache Kafka**. O projeto é composto por múltiplos microserviços especializados em diferentes domínios de negócio.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Execução Rápida](#execução-rápida)
- [Configuração Detalhada dos Bancos de Dados](#configuração-detalhada-dos-bancos-de-dados)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Padrões e Práticas](#padrões-e-práticas)

---

## 🎯 Visão Geral

O **Banco Digital Ana** é um sistema modular que gerencia:

- **Contas Correntes**: Criação e gerenciamento de contas de usuários
- **Transferências**: Processamento de transferências entre contas
- **Tarifação**: Cálculo e aplicação automática de tarifas em operações

Os serviços comunicam-se de forma desacoplada através do **Apache Kafka**, permitindo escalabilidade e resiliência.

---

## 🏗️ Arquitetura

### Padrões de Design

#### **DDD (Domain-Driven Design)**

Cada microserviço segue os princípios do DDD:

- **Entities**: Objetos com identidade única (ex: Conta, Transferência)
- **Value Objects**: Objetos imutáveis sem identidade (ex: Tarifa, DataOperacao)
- **Aggregates**: Grupos de objetos que formam uma unidade transacional
- **Repositories**: Abstração de acesso a dados
- **Domain Events**: Eventos que representam mudanças no domínio

#### **CQRS (Command Query Responsibility Segregation)**

Separação de responsabilidades entre leitura e escrita:

- **Commands**: Operações que modificam o estado (ex: NovaTransferenciaCommand)
- **Queries**: Operações que recuperam dados (ex: ObterContaQuery)
- **Command Handlers**: Processam comandos e emitem eventos
- **Query Handlers**: Retornam dados sem efeitos colaterais

#### **Building Blocks**

Projeto compartilhado com abstrações e infraestrutura comum:

- Interfaces CQRS
- Middlewares de tratamento de erros
- Autenticação JWT
- Mensagens Kafka
- DTOs padronizados

### Fluxo de Comunicação

```

                    Banco Digital Ana


  Conta Corrente API         Transferência API
  ├─ DDD Domain              ├─ DDD Domain
  ├─ CQRS Handlers           ├─ CQRS Handlers
  └─ Repository              └─ Repository
         │                            │
         └──────────┬───────────────┘
                     │
              [Apache Kafka]
              ├─ transferencias-realizadas (topic)
              ├─ tarifacoes-realizadas (topic)
              └─ group-ids para consumers
                     │
                     └──────────────────┐
                                          │
                            Tarifação Service
                            ├─ DDD Domain
                            ├─ Event Handlers
                            └─ Producer (Tarifa Events)


```

---

## 🚀 Pré-requisitos

- **Docker Desktop** (versão 20.10+)
- **Docker Compose** (versão 1.29+)
- **.NET SDK 8.0+** (opcional, para desenvolvimento local)
- **SQL\*Plus ou Oracle SQL Developer** (para conexão manual aos bancos)

## 🐳 Execução Rápida

### 1️⃣ Iniciar Todos os Serviços

```bash
# Na raiz do projeto
docker-compose up -d

# Ou com rebuild (se houver alterações)
docker-compose up -d --build
```

**Aguarde 2-3 minutos para que todos os serviços fiquem saudáveis.**

Verifique o status:

```bash
docker-compose ps
```

Esperado:

```
NAME                 STATUS
contacorrente-db    Up (healthy)
contacorrente-api   Up
transferencia-db    Up
transferencia-api   Up
tarifa-db          Up
tarifa-service     Up
zookeeper          Up
kafka              Up
```

### 2️⃣ Acessar as APIs

| Serviço        | URL                   | Documentação                  |
| -------------- | --------------------- | ----------------------------- |
| Conta Corrente | http://localhost:5001 | http://localhost:5001/swagger |
| Transferência  | http://localhost:5002 | http://localhost:5002/swagger |

---

## 🔧 Configuração Detalhada dos Bancos de Dados

Após os serviços subirem, é necessário conectar em cada banco Oracle e executar os scripts de inicialização.

### Credenciais Padrão

| Componente         | Host      | Porta | User   | Password | Service |
| ------------------ | --------- | ----- | ------ | -------- | ------- |
| **Conta Corrente** | localhost | 1521  | system | oracle   | FREE    |
| **Transferência**  | localhost | 1522  | system | oracle   | FREE    |
| **Tarifação**      | localhost | 1523  | system | oracle   | FREE    |

### Opção 1: Usando SQL\*Plus (Recomendado)

#### Conta Corrente

```bash
# Conectar ao banco
sqlplus system/oracle@localhost:1521/FREE

# No prompt do SQL*Plus, executar o script
@BancoDigitalAna.ContaCorrente/Database/scripts/init.sql
```

#### Transferência

```bash
sqlplus system/oracle@localhost:1522/FREE
@BancoDigitalAna.Transferencia/Database/scripts/init.sql
```

#### Tarifação

```bash
sqlplus system/oracle@localhost:1523/FREE
@BancoDigitalAna.Tarifacao/Database/scripts/init.sql
```

### Opção 2: Usando Oracle SQL Developer

1. **Criar Nova Conexão**:

   - Nome: `Conta Corrente`
   - Host: `localhost`
   - Port: `1521`
   - Service Name: `FREE`
   - Username: `system`
   - Password: `oracle`
   - Clique em "Testar" → "Conectar"

2. **Executar Script**:

   - File → Open → Selecione `BancoDigitalAna.ContaCorrente/Database/scripts/init.sql`
   - Execute (F9 ou Ctrl+Enter)

3. **Repetir para os outros bancos** (portas 1522 e 1523)

### Opção 3: Usando DBeaver

1. Criar nova conexão Oracle
2. Configurar com os dados acima
3. Right-click na conexão → SQL Script → abrir arquivo .sql
4. Execute

### Opção 4: Docker Exec (Sem Cliente Local)

Se não tiver SQL\*Plus instalado, pode executar dentro do container:

```bash
# Conta Corrente
docker exec -it contacorrente-db sqlplus -S system/oracle@localhost:1521/FREE @/opt/oracle/scripts/startup/init.sql

# Transferência
docker exec -it transferencia-db sqlplus -S system/oracle@localhost:1522/FREE @/opt/oracle/scripts/startup/init.sql

# Tarifação
docker exec -it tarifa-db sqlplus -S system/oracle@localhost:1523/FREE @/opt/oracle/scripts/startup/init.sql
```

---

## 📁 Estrutura do Projeto

```
BancoDigitalAna/
├── BancoDigitalAna.BuildingBlocks/        # Abstrações compartilhadas
│   ├── CQRS/                              # Interfaces de Command e Query
│   ├── Domain/                            # Interfaces de domínio
│   ├── Infrastructure/                    # Helpers de infraestrutura
│   ├── Kafka/                             # Modelos de mensagens Kafka
│   └── Middlewares/                       # Middlewares globais
│
├── BancoDigitalAna.ContaCorrente/         # Serviço de Contas
│   ├── Application/                       # Layer de aplicação
│   │   ├── Commands/                      # Comandos CQRS
│   │   ├── Handlers/                      # Handlers de comandos
│   │   ├── Queries/                       # Queries CQRS
│   │   ├── DTOs/                          # Data Transfer Objects
│   │   └── Services/                      # Serviços de aplicação
│   ├── Domain/                            # Layer de domínio (DDD)
│   │   ├── Entities/                      # Entidades do domínio
│   │   ├── Events/                        # Eventos de domínio
│   │   ├── Repositories/                  # Abstrações de repository
│   │   └── ValueObjects/                  # Objetos de valor
│   ├── Infrastructure/                    # Layer de infraestrutura
│   │   ├── Database/                      # Contexto EF Core
│   │   ├── Repositories/                  # Implementação de repositories
│   │   ├── Services/                      # Serviços externos
│   │   └── Mappings/                      # Mapeamentos AutoMapper
│   ├── Controllers/                       # Controllers REST
│   ├── Program.cs                         # Startup configuration
│   └── appsettings.json                   # Configurações
│
├── BancoDigitalAna.Transferencia/         # Serviço de Transferências
│   ├── Application/                       # Estrutura similar a Contas
│   ├── Domain/
│   ├── Infrastructure/
│   ├── Controllers/
│   └── Program.cs
│
├── BancoDigitalAna.Tarifacao/             # Serviço de Tarifação
│   ├── Handlers/                          # Event handlers do Kafka
│   ├── Domain/
│   ├── Infrastructure/
│   ├── Producers/                         # Producers Kafka
│   └── Program.cs
│
├── BancoDigitalAna.UnitTests/             # Testes unitários
│   ├── ContasCorrente/
│   └── Transferencia/
│
├── docker-compose.yml                     # Orquestração de containers
└── README.md                              # Este arquivo
```

---

## 🛠️ Tecnologias Utilizadas

### Backend

- **C# / .NET 8.0**
- **ASP.NET Core** - Framework web
- **Entity Framework Core 8.0** - ORM
- **Oracle Database 21c** - Banco de dados
- **MediatR** - Implementação de CQRS
- **AutoMapper** - Mapeamento de objetos
- **Asp.Versioning** - Versionamento de APIs

### Mensageria

- **Apache Kafka 7.5.0** - Message broker
- **KafkaFlow** - Cliente Kafka para .NET
- **Zookeeper** - Coordenação Kafka

### Testes

- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions fluentes

### DevOps

- **Docker** - Containerização
- **Docker Compose** - Orquestração local

---

## 🎨 Padrões e Práticas

### Domain-Driven Design (DDD)

#### Estrutura de Camadas

```
┌─────────────────────────────────────┐
│    Application Layer (MediatR)      │ ← Commands & Queries
├─────────────────────────────────────┤
│     Domain Layer (Business Logic)   │ ← Entities, Value Objects, Events
├─────────────────────────────────────┤
│   Infrastructure Layer (Data Access)│ ← Repositories, DbContext
├─────────────────────────────────────┤
│    Presentation Layer (Controllers) │ ← REST APIs
└─────────────────────────────────────┘
```

## 🧪 Testes

### Testes Unitários

```bash
# Executar todos os testes
dotnet test BancoDigitalAna.UnitTests

# Executar testes de um projeto específico
dotnet test BancoDigitalAna.UnitTests/ContasCorrente
```

Exemplos de testes incluem:

- Validações de domínio
- Handlers CQRS
- Repositórios
- Mapeamentos

---

## 👤 Autor

**Davi Freitas da Silva**  
[LinkedIn](https://linkedin.com/in/freitasDavi)
(Sim o readme.md foi feito com auxílio de agente de I.A)

---

## 📄 Licença

Este projeto é licenciado sob a MIT License - veja o arquivo LICENSE para detalhes.

```

```
