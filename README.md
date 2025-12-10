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

A solução segue um **monólito modular** em .NET, com três executáveis principais e bibliotecas de suporte:

- **OnlineSurveys.Web** – Front-end web (ASP.NET Core MVC/Razor)  
  Interface pública (listagem de questionários) e base para área administrativa (criação e visualização de pesquisas).  
  Consome a API via `HttpClient` usando JSON.

- **OnlineSurveys.Api** – Back-end (ASP.NET Core Web API)  
  Exposição das regras de negócio via endpoints REST (JSON), usados pelo Web e futuramente por outros canais.  
  Atualmente expõe endpoints para **criação** e **consulta** de questionários.

- **OnlineSurveys.Worker** – Worker de agregação (Worker Service)  
  Responsável por processamento assíncrono de respostas e geração de tabelas de resumo para leitura rápida
  (estrutura criada para suportar o requisito de escala).

Bibliotecas de suporte:

- **OnlineSurveys.Domain** – Entidades e regras de domínio  
- **OnlineSurveys.Application** – Camada de aplicação (casos de uso / orquestração – ponto de extensão futuro)  
- **OnlineSurveys.Infrastructure** – Acesso a dados com **Entity Framework Core** e demais componentes de infraestrutura  
- **OnlineSurveys.Api.Tests** – Testes automatizados da API (xUnit + EF Core InMemory)

Comunicação:

- Web ↔ Api via **HTTP/JSON (REST)** usando `HttpClient`
- Api ↔ Banco via **EF Core / SQL Server**
- Worker ↔ Banco via **EF Core / SQL Server**

Essa arquitetura foi pensada para equilibrar:

- **Prazo** (eleições chegando, necessidade de entregar rápido)
- **Simplicidade operacional** (monólito modular em vez de vários microserviços)
- **Escalabilidade** (API enxuta + worker para processamento pesado + possibilidade de cache/mensageria)

---

## Tecnologias principais

- **.NET 9**
- **ASP.NET Core 9**
  - Web App (MVC/Razor) – `OnlineSurveys.Web`
  - Web API – `OnlineSurveys.Api`
- **.NET Worker Service** – `OnlineSurveys.Worker`
- **Entity Framework Core 9** – `SurveysDbContext` em `OnlineSurveys.Infrastructure`
- **SQL Server / Azure SQL** como banco relacional
- **Swashbuckle / Swagger** (documentação da API)
- **xUnit + EF Core InMemory** para testes automatizados (`OnlineSurveys.Api.Tests`)

---

## Diagramas C4

### 1. Contexto (C4 - Level 1)

![OnlineSurveys - Containers](https://raw.githubusercontent.com/claudiojccoimbra/OnlineSurveys/master/docs/c4-context.png)

### 2. Containers (C4 - Level 2)

![OnlineSurveys - Containers](https://raw.githubusercontent.com/claudiojccoimbra/OnlineSurveys/master/docs/c4-containers.png)

### 3. Visão de Testes

![OnlineSurveys - Containers](https://raw.githubusercontent.com/claudiojccoimbra/OnlineSurveys/master/docs/c4-tests.png)

---

## Como testar a API pelo Swagger

Com a API (`OnlineSurveys.Api`) em execução, acesse o Swagger em:

`http://localhost:5222/swagger/index.html`

Lá você consegue testar os endpoints da API diretamente pelo navegador.

### Exemplo de execução: POST /api/Questionnaires

1. Acesse: `http://localhost:5222/swagger/index.html`
2. Expanda o endpoint **POST /api/Questionnaires**
3. No campo **Request body**, cole o JSON abaixo:
   ```json
   {
     "title": "Intenção de voto - Prefeito",
     "description": "Pesquisa de intenção de voto para as eleições municipais",
     "startsAt": "2025-01-01T00:00:00Z",
     "endsAt": "2025-01-31T23:59:59Z",
     "questions": [
       {
         "text": "Em quem você pretende votar para prefeito?",
         "choices": [
           { "text": "Candidato A" },
           { "text": "Candidato B" },
           { "text": "Branco/Nulo" }
         ]
       }
     ]
   }
