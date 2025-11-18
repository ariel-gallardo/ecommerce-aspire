[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/ariel-gallardo/clean-code)

## Description
This project is a full e-commerce solution built with **.NET / ASP.NET Core**, using a **layered architecture** and **microservices** integrated with Aspire .NET. The system is structured to separate responsibilities clearly across layers while allowing microservices communication.  

The main features include:  
- **Layered Architecture:**  
  - **Application:** business logic and service interfaces  
  - **Domain:** core entities, value objects, and domain rules  
  - **Infra:** integration with external systems, messaging, caching, etc.  
  - **Persistence:** data access, repositories, and database configuration  
- **Microservices:** services for products, orders, carts, invoices, and notifications  
- **SQLite** for local and lightweight storage  
- Role- and claim-based authentication/authorization  
- Messaging between microservices with MassTransit  
- Invoice generation after order payment  
- Hexagonal principles applied within layers 


## Technologies Used
- .NET / ASP.NET Core  
- Layered Architecture (Application, Domain, Infra, Persistence)  
- Microservices with Aspire .NET  
- SQLite (lightweight database for all microservices)  
- Messaging with MassTransit  
- Role - and claim-based authentication/authorization  
- Docker (optional, for dependencies like RabbitMQ or Redis)  