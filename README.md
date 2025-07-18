# OutlistService

Serviço REST para gerenciamento de produtos Outlist utilizando .NET 8, MongoDB e arquitetura limpa (Clean Architecture).

---

## ✨ Tecnologias Utilizadas

* .NET 8
* MongoDB
* Clean Architecture
* Swagger
* Docker / Docker Compose

---

## 🔄 Executar com Docker

1. Certifique-se que o Docker esteja instalado e rodando.

2. Execute o comando na raiz do projeto:

```bash
docker-compose up --build
```

3. Acesse a API em:

```
http://localhost:5000/swagger
```

---

## 🔧 Executar Localmente (sem Docker)

1. Configure o appsettings ou variáveis de ambiente com:

```json
Mongo__ConnectionString = mongodb://localhost:27017
Mongo__Database = OutlistDb
```

2. Rode via Visual Studio ou com:

```bash
dotnet run --project OutlistService.API
```

---

## 🔐 Endpoints da API

> Base URL: `http://localhost:5000/api/outlist`

| Método | Rota                           | Descrição                    |
| ------ | ------------------------------ | ---------------------------- |
| POST   | `/api/outlist`                 | Adiciona produto Outlist     |
| GET    | `/api/outlist`                 | Lista paginada de produtos   |
| GET    | `/api/outlist/{code}`          | Detalhes por productCode     |
| PUT    | `/api/outlist/{code}/validity` | Atualiza vigência do produto |
| DELETE | `/api/outlist/{code}`          | Remove produto               |

---

## 🔢 Exemplo de Requisição (JSON)

### POST /api/outlist

```json
{
  "productCode": "12345",
  "validFrom": "2025-07-01T00:00:00",
  "validTo": "2025-08-01T00:00:00"
}
```

### PUT /api/outlist/12345/validity

```json
{
  "validFrom": "2025-08-01T00:00:00",
  "validTo": "2025-08-31T00:00:00"
}
```

---

## 🔒 Sobre Contratos e Versionamento

* Todas as rotas seguem padrão REST.
* Versões futuras da API podem ser adicionadas via `/api/v2/outlist`.
* A quebra de contrato deve ser evitada com evolução progressiva (ex: novos campos opcionais).

---

## 🔒 Segurança (Sugestão)

* Autenticação JWT Bearer
* API Gateway com validação de escopos
* Rate limiting com middleware

---

## 🔧 Testes (Sugestão)

* Pode-se usar `xUnit` com `Moq` para mockar o repositório e testar a camada de `UseCases`.

---

## 📦 Pasta Estrutura (Clean Architecture)

```
OutlistService.sln
├── OutlistService.API           // Controllers e Startup
├── OutlistService.Application   // UseCases
├── OutlistService.Domain        // Entities e Interfaces
├── OutlistService.Infrastructure// MongoDB Repository
```

---

## ✉ Contato

Caíque Ferraz
[https://github.com/caicoala](https://github.com/caicoala)

---
