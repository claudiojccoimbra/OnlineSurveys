# OnlineSurveys 🎯

Plataforma de **questionários online** construída em **.NET 9**, focada em pesquisas públicas em larga escala
(ex.: pesquisas de intenção de voto nas eleições).

O objetivo é permitir:

- Criação de questionários de múltipla escolha
- Divulgação pública (links em redes sociais, anúncios etc.)
- Coleta massiva de respostas
- Exposição de resultados **sumarizados** para usuários internos (admins/analistas)

---

## Arquitetura (visão geral)

A solução segue um **monólito modular** em .NET, com três executáveis principais e três bibliotecas de suporte:

- **OnlineSurveys.Web** – Front-end web (ASP.NET Core MVC/Razor)  
  Interface pública (respondente) + área administrativa (criação de pesquisas, visualização de resultados).

- **OnlineSurveys.Api** – Back-end (ASP.NET Core Web API)  
  Exposição das regras de negócio via endpoints REST (JSON), usados pelo Web e futuramente por outros canais.

- **OnlineSurveys.Worker** – Worker de agregação (Worker Service)  
  Responsável por processamento assíncrono de respostas e geração de tabelas de resumo para leitura rápida.

Bibliotecas de suporte:

- **OnlineSurveys.Domain** – Entidades e regras de domínio  
- **OnlineSurveys.Application** – Camada de aplicação (casos de uso / orquestração)  
- **OnlineSurveys.Infrastructure** – Acesso a dados com **Entity Framework Core** e outras infra (ex.: cache)

Comunicação:

- Web ↔ Api via **HTTPS/JSON (REST)**
- Api ↔ Banco via **EF Core / SQL Server**
- Worker ↔ Banco via **EF Core / SQL Server**

---

## Tecnologias principais

- **.NET 9**
- **ASP.NET Core 9**
  - Web App (MVC/Razor)
  - Web API
- **.NET Worker Service**
- **Entity Framework Core 9**
- **SQL Server / Azure SQL**
- **Swashbuckle / Swagger** (documentação da API)

---

## Estrutura da solution

```text
OnlineSurveys.sln
OnlineSurveys.Api/
OnlineSurveys.Web/
OnlineSurveys.Worker/
OnlineSurveys.Domain/
OnlineSurveys.Application/
OnlineSurveys.Infrastructure/
