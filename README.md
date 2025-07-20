
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

## 📌 Considerações sobre o Desafio

Abaixo estão minhas decisões e alinhamentos com os pontos propostos:

- ✅ **Testes automatizados**: implementei testes de unidade com xUnit e Moq, além de testes de comportamento (BDD) no estilo *Given-When-Then*, cobrindo os principais fluxos do sistema.
- ✅ **Uso de BDD**: segui o modelo proposto com arquivos separados de teste focando em clareza de comportamento, como sugerido.
- ✅ **Clean Architecture**: toda a estrutura do projeto foi pensada em camadas bem definidas, com separação entre domínio, aplicação, infraestrutura e API, facilitando manutenção e testes.
- ✅ **Arquitetura RESTful**: os endpoints seguem o modelo REST, com uso de verbos HTTP adequados e URIs bem definidas.
- ✅ **Uso de containers**: foi incluído um `docker-compose.yml` para facilitar a execução do MongoDB localmente. A API também pode ser facilmente containerizada, se necessário.
- ✅ **Swagger**: a documentação da API está disponível via Swagger UI, com suporte a autenticação JWT para facilitar a visualização e testes.

### ❓ O que faria para não expor uma API?

- Utilizaria **autenticação e autorização robustas**, como JWT + roles.
- Criaria um **API Gateway** com controle de rotas públicas e privadas.
- Utilizaria regras de **firewall e redes privadas**, limitando o acesso por IP/ambiente.
- Aplicaria políticas de **rate limiting** e CORS estritos.

### ❓ O que faria para não quebrar um contrato que já esteja sendo utilizado por outra aplicação?

- Usaria **versionamento de API** (ex: `v1`, `v2`), como já demonstrado neste projeto.
- Manteria endpoints antigos até garantir migração completa dos consumidores.
- Adotaria a prática de **contract-first** com ferramentas como Swagger/OpenAPI.
- Aplicaria **testes de contrato (Consumer-Driven Contracts)** usando ferramentas como Pact.

## 📬 Testes com Postman

Uma collection do Postman está incluída no projeto para facilitar a validação da API.

📂 Arquivo: `OutlistService_Postman_Collection.json`  
📥 Para importar no Postman:  
1. Abra o Postman  
2. Clique em **Import > File**  
3. Selecione o arquivo acima  
4. Configure a variável `{{baseURL}}` como `https://localhost:44388`


Desenvolvido por **Caíque Ferraz**.
