# Ecommerce Aspire

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/ariel-gallardo/ecommerce-aspire)


[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)


[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net&logoColor=white)](https://learn.microsoft.com/dotnet/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-latest-512BD4?logo=asp.net&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![gRPC](https://img.shields.io/badge/gRPC-v1.0-1f425f?logo=grpc&logoColor=white)](https://grpc.io/)
[![MassTransit](https://img.shields.io/badge/MassTransit-Active-00AEEF)](https://masstransit-project.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-broker-FF6600?logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![Redis](https://img.shields.io/badge/Redis-cache-DC382D?logo=redis&logoColor=white)](https://redis.io/)
[![SQLite](https://img.shields.io/badge/SQLite-lightweight-003B57?logo=sqlite&logoColor=white)](https://www.sqlite.org/)
[![EF Core](https://img.shields.io/badge/EntityFramework-Core-2196F3?logo=entityframework&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![Docker](https://img.shields.io/badge/Docker-containers-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![JWT](https://img.shields.io/badge/JWT-auth-000000?logo=jsonwebtokens&logoColor=white)](https://jwt.io/)

## Overview
A full e-commerce solution built with `.NET 9` / ASP.NET Core following a layered architecture and microservices approach. The project separates responsibilities across layers (Application, Domain, Infrastructure, Persistence) and uses messaging for inter-service communication.

Key capabilities:
- Layered architecture with hexagonal principles
- Microservices for common e-commerce domains
- Lightweight local storage using SQLite
- Messaging with MassTransit
- Role- and claim-based authentication & authorization
- Invoice generation workflow after order payment

## Architecture
- Application: business logic and service interfaces
- Domain: core entities, value objects and domain rules
- Infrastructure (Infra): external integrations, messaging, caching
- Persistence: database configuration and repositories

## Microservices (selected)
| Icon | Service | Purpose | Project path |
|---:|---|---|---|
| 🛍️ | `Product` | Product catalog and queries | `Services/Product/Product.API` |
| 🧾 | `Order` | Order creation, state and lifecycle | `Services/Order/Order.API` |
| 🛒 | `Cart` | Shopping cart management | `Services/Cart/Cart.API` |
| 🧾💳 | `Invoice` | Invoice generation and storage | `Services/Invoice/Invoice.API` |
| 💳 | `Payment` | Payment processing and events | `Services/Payment/Payment.API` |
| 📦 | `Inventory` | Stock management and sync | `Services/Inventory/Inventory.API` |
| 🚚 | `Shipping` | Shipping rates and tracking | `Services/Shipping/Shipping.API` |
| 🔔 | `Notification` | Email / push notifications | `Services/Notification/Notification.API` |
| 📜 | `Logs` | Application logging and audit | `Services/Logs/Logs.API` |
| 🔐 | `Security` | Authentication, roles and claims | `Services/Security/Security.API` |

Note: Each service follows the same layered layout with projects for `Domain`, `Application`, `Infrastructure`, `Controllers`, `Persistence`, `Messaging`, and optional seeders and cache keys.

## Top-level projects and common libraries
- `ApiGateway` — API gateway for routing and aggregation
- `Common` — shared domain, infrastructure and API utilities
- `Ecommerce` — solution/host helpers

## Quick start (local)
1. Restore and build solution:
   - `dotnet restore`
   - `dotnet build`
2. Run a single service (example — Product API):
   - `dotnet run --project Services/Product/Product.API/Product.API.csproj`
3. Run the API gateway or other services the same way using the corresponding `*.API.csproj` file paths.

Optional: Use Docker to run dependencies (RabbitMQ, Redis, etc.).

## Common commands
- Create a new class library module example:
  - `dotnet new classlib --name Invoice.Infrastructure.Persistence --output Services/Invoice/Infrastructure/Invoice.Infrastructure.Persistence`

## Technologies
This repository uses the following technologies and tools across services. Each item includes a link to official docs or project site:

- [.NET 9 / ASP.NET Core](https://learn.microsoft.com/dotnet/) — runtime and web framework
- [gRPC](https://grpc.io/) — internal high-performance RPC between services
- [MassTransit](https://masstransit-project.com/) — messaging abstraction used for event-driven communication
- [RabbitMQ](https://www.rabbitmq.com/) — broker used by MassTransit for message transport
- [Redis](https://redis.io/) — distributed cache
- [SQLite](https://www.sqlite.org/) — lightweight local persistence for services and seeders
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) — data access and migrations
- [Docker](https://www.docker.com/) — containerization for infrastructure and services (optional)
- [JWT](https://jwt.io/) — JSON Web Tokens for authentication and claims

(Also used across projects: `MassTransit.RabbitMQ`, `StackExchange.Redis`, `Microsoft.Data.Sqlite`, `Microsoft.EntityFrameworkCore` packages.)

## Contributing
Contributions are welcome. Follow repository conventions: keep layered separation, add tests, and update README and seeders when adding services.

## License
This project is licensed under the MIT License — see the [LICENSE](./LICENSE) file for details.