# Projeto - Gerenciamento e Cálculo de Produtos de Pedido

## 📋 Descrição
Este projeto tem como objetivo desenvolver uma solução utilizando .NET 8 para gerenciar e calcular os produtos de um pedido. Os pedidos serão recebidos de um Sistema Externo A, processados, e posteriormente disponibilizados para o Sistema Externo B.


## 🚀 Tecnologias Utilizadas
- **.NET 8:** Framework principal para a aplicação.

- **Entity Framework Core:** ORM para manipulação do banco de dados.

- **Serilog:** Sistema de logs estruturados.

- **SQL Server:** Banco de dados relacional.

- **XUnit, FluentAssertions, Bogus e NSubstitute:** Validação de dados e testes unitários.

- **Rate Limiting:** Prevenção de ataques de força bruta.

- **Git Flow e Commit semântico**.


## 📌 Arquitetura
A aplicação segue um padrão de divisão em camadas:

- API: Camada de apresentação (Controllers, Middlewares)

- Application: Regras de negócio, factories e strategies.

- Domain: Entidades

- Infra: Repositórios e acesso a dados


## 🎯 Requisitos Técnicos

1. Framework: **.NET 8**

2. Padrão de arquitetura: Divisão em camadas (API, Application, Domain e Infra)

3. Registro de logs: **Utilizar Serilog**

4. Banco de dados: Livre para escolha (pode ser In-memory)

5. Controle de versão: **Aplicar Git Flow e Commit semântico**

6. Boas práticas: **Clean Code, SOLID, DRY, YAGNI e Object Calisthenics**

7. APIs REST:

- Seguir boas práticas de **RESTful**

- Utilizar **respostas HTTP apropriadas** (200, 201, 400, 404, etc.)


## ✅ Requisitos Funcionais e Não Funcionais

1. Validação de duplicidade de pedidos

2. Suporte a alta volumetria (150 a 200 mil pedidos/dia)

3. Cálculo do imposto do pedido, com as regras:

- Cálculo em vigor: Valor total dos itens * 0,3

- Cálculo reforma tributária: Valor total dos itens * 0,2

4. Feature flag para ativar o novo cálculo de imposto

5. Disponibilização dos pedidos para o Sistema B com o imposto calculado


## 📂 Estrutura do Projeto
Abaixo está a organização principal do projeto:
```
/src
  /GestaoProdutos.API
    /logs
        - log-api-YYYYMMDD.txt
    /controllers
        /v1
            - OrderController.cs
  /GestaoProdutos.Application
    /interfaces
        - ICalculateTaxFactory.cs
        - ICalculateTaxStrategy.cs
        - IOrderService.cs
    /factories
        - CalculateTaxFactory.cs
    /strategies
        - CalculateTaxDefault.cs
        - CalculateTaxReform.cs
    /services
        - OrderService.cs
    - ApplicationDependencyRegister.cs
  /GestaoProdutos.Domain
    /entities
        - Order.cs
        - OrderItem.cs
  /GestaoProdutos.Infra
    /interfaces
        - IRepository.cs
    /repositories
        - Repository.cs
    - InfraDependencyRegister.cs
  /GestaoProdutos.Tests
    /api
        /controllers
            - OrderControllerTests.cs
    /application
        /services
            - OrderServiceTests.cs
    /infra
        - RepositoryTests.cs
  /doc
    GestaoProdutos.postman_collection.json
GestaoProdutos.sln
readme.md
```

## 🔧 Configuração e Execução Local
Siga as etapas abaixo para configurar e executar a aplicação localmente:

**1. Pré-requisitos**

Certifique-se de que você possui os seguintes itens instalados:

- **SDK do .NET 8:** [Baixar aqui](https://dotnet.microsoft.com/pt-br/download)

- **SQL Server** (ou outro banco compatível configurado no `appsettings.json`).

- Um editor de texto, como **Visual Studio** ou **Visual Studio Code**.

**2. Clonar o Repositório**

Clone este repositório para sua máquina local:

```
bash
git clone https://github.com/priscila-vacari/mouts-gestao-produtos.git
cd mouts-gestao-produtos
```

**3. Configurar o Banco de Dados**

1. Certifique-se de que seu banco de dados SQL Server está configurado e rodando.

2. Atualize o arquivo `appsettings.json` com a string de conexão:
```
json
"ConnectionStrings": {
  "GestaoProdutosConnection": "Server=SEU_SERVIDOR;Database=SEU_BANCO;User Id=SEU_USUARIO;Password=SUA_SENHA;Encrypt=False;Pooling=true;"
}
```

3. Execute as migrações para preparar o banco de dados:

```
bash
dotnet ef database update --project src\GestaoProdutos.Infra --startup-project src\GestaoProdutos.API
```

**4. Rodar o Projeto**

1. Compile e execute a aplicação:
```
bash
dotnet build
dotnet run
```

2.  Acesse a aplicação em:

API: https://localhost:7056/api

Worker: Logs serão exibidos no terminal e no diretório `/logs`.

**5. Testar os Endpoints**

Use **Postman** ou **cURL** para testar os endpoints.

[collection postman](https://github.com/priscila-vacari/mouts-gestao-produtos/blob/main/doc/GestaoProdutos.postman_collection.json)

**Exemplo de Criação de Pedidos**

- Rota: `POST /api/v1/pedidos`
 
- Payload:
```
json
{
  "pedidoId": 1,
  "clienteId": 1,
  "itens": [
    {
      "produtoId": 123,
      "quantidade": 1,
      "valor": 10.78
    }
  ]
}
```

- Curl:
```
curl --location 'https://localhost:7056/api/v1/pedidos' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
  "pedidoId": 1,
  "clienteId": 1,
  "itens": [
    {
      "produtoId": 123,
      "quantidade": 1,
      "valor": 10.78
    }
  ]
}'
```

**Exemplo de Consulta de Pedidos**

- Rota: `POST /api/v1/pedidos/{id}`

- Resposta:
```
json
{
    "id": 1,
    "pedidoId": 1,
    "clienteId": 1,
    "imposto": 0,
    "itens": [
        {
            "produtoId": 1001,
            "quantidade": 2,
            "valor": 52.7
        }
    ],
    "status": "Criado"
}
```

- Curl:
```
curl --location 'https://localhost:7056/api/v1/pedidos/1' \
--header 'accept: */*'
```

**Exemplo de Consulta de Pedidos por Status**

- Rota: `POST /api/v1/pedidos?status={status}`

- Resposta:
```
json[
    {
        "id": 1,
        "pedidoId": 1,
        "clienteId": 1,
        "imposto": 0,
        "itens": [
            {
                "produtoId": 1001,
                "quantidade": 2,
                "valor": 52.7
            }
        ],
        "status": "Criado"
    },
    {
        "id": 2,
        "pedidoId": 2,
        "clienteId": 1,
        "imposto": 47.34,
        "itens": [
            {
                "produtoId": 123,
                "quantidade": 10,
                "valor": 15.78
            }
        ],
        "status": "Criado"
    }
]
```

- Curl:
```
curl --location 'https://localhost:7056/api/v1/pedidos?status=Criado' \
--header 'accept: */*'
```
## 🛡️ Boas Práticas e Padrões

**1. Logs Estruturados:**

- Todos os eventos importantes (erros, conciliações, lançamentos) são registrados nos arquivos de log.

- Automaticamente será salvo o `correlation_id` correspondente à requisição, a fim de rastrear o fluxo por diversas partes do sistema, facilitando a depuração e o monitoramento de problemas.

- Logs disponíveis em `/logs/`.

**2. Segurança:**

- Proteção contra força bruta (rate limiting) aplicada globalmente.

**3. Validação:**

- Todas as entradas do usuário são validadas com o FluentValidation para garantir consistência e segurança.

**4. Configuração Dinâmica:**

- Parâmetros como a feature-flag da reforma tributária podem ser ajustados no `appsettings.json`.

**5. Factory Pattern:**

- Encapsulada a lógica de cálculo dos impostos para evitar acoplamento direto e facilitando as expansões futuras sem impactar o código já existente e facilitando a manutenção.

- Melhora a testabilidade e garante mais segurança e controle.

**6. Strategy Pattern:**

- Desacoplamento da lógica de cálculo de imposto onde as regras podem mudar com o tempo, preparando o código para futuras alterações sem impacto significativo.

- Código mais limpo e organizado.

- Mais flexibilidade e melhora na testabilidade.


## 🧪 Testes

O projeto inclui testes unitários para todas as funcionalidades principais. Execute os testes com:

```
bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator "-reports:TestResults\**\*.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html"
```

## 🛠️ Ferramentas Adicionais

**Swagger UI:**

- Acesse `https://localhost:7056/swagger/index.html` para explorar e testar as APIs interativamente.

**Serilog Dashboard (opcional):**

- Integre visualizações de logs com ferramentas como Seq ou Kibana para uma análise avançada.

## ❗️ Pendências

1. Implementar autenticação e autorização JWT (JSON Web Token).
2. Implementar containerização.
3. Criar um nuget package de conexão com banco de dados.
4. Aumentar cobertura de código.
5. Implementar uso de filas como RabbitMQ para melhorar ainda mais a escalabilidade no recebimento das requisições de pedidos e envio para o sistema externo, juntamento com a criação de workers de processamento.


## 📜 Licença

N/A.