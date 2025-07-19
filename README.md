# OutlistService.API

API RESTful desenvolvida em .NET 7 para gerenciamento de produtos do tipo Outlist (fora de linha).  
Projeto criado como parte de um desafio técnico, com foco em boas práticas de arquitetura, testes automatizados e uso de MongoDB como persistência.

## 🧩 Estrutura do Projeto

O projeto segue uma arquitetura em camadas bem definida:

- **Domain**: entidades de negócio e interfaces de repositório
- **Application**: regras de uso e casos de aplicação (serviços)
- **Infrastructure**: persistência de dados (MongoDB)
- **API**: camada de exposição HTTP com endpoints REST

```
OutlistService/
├── API
├── Application
├── Domain
├── Infrastructure
├── tests
```

## ⚙️ Tecnologias Utilizadas

- .NET 7
- ASP.NET Core
- MongoDB
- xUnit + Moq (testes de unidade)
- BDD (Behavior Driven Development) com estilo Given-When-Then
- Swagger para documentação automática
- Docker + Docker Compose (MongoDB local)

## 🔧 Como Executar

### 1. Subir o MongoDB com Docker
```bash
docker-compose up -d
```

### 2. Rodar a API localmente
```bash
dotnet run --project OutlistService.API
```

A API estará disponível em:  
📍 `https://localhost:7259/swagger`  
📍 `http://localhost:5001/swagger`

> A configuração de portas pode ser ajustada no arquivo `launchSettings.json`.

## 📦 Endpoints REST

| Verbo | Rota                         | Ação                            |
|-------|------------------------------|----------------------------------|
| GET   | `/api/outlist`               | Listar produtos paginados       |
| GET   | `/api/outlist/{code}`        | Buscar produto por código       |
| POST  | `/api/outlist`               | Adicionar novo produto          |
| PUT   | `/api/outlist/{code}/validity` | Atualizar validade de produto |
| DELETE| `/api/outlist/{code}`        | Remover produto                 |

## ✅ Testes

A suíte de testes cobre os principais casos de uso da aplicação, incluindo:

- ✔️ Retorno de listas paginadas
- ✔️ Validação de produto existente ou não
- ✔️ Inserção, remoção e atualização de produtos
- ✔️ Testes de exceções e falhas de repositório
- ✔️ Testes no estilo BDD (Given-When-Then)

```bash
dotnet test
```

## 📁 Configurações

Arquivo `appsettings.json` define os parâmetros do MongoDB:
```json
{
  "Mongo": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "OutlistDb"
  }
}
```

## 💡 Decisões Técnicas

- Optei por abstrair o repositório com `IOutlistRepository` para facilitar testes e trocar implementações futuramente.
- A estrutura atual visa flexibilidade e escalabilidade para adicionar novos recursos de forma fluida.
- Swagger está embutido desde o início para facilitar a validação e consumo da API.

## 🙋 Sobre o Autor

Desenvolvido por **Caíque Ferraz**.  
