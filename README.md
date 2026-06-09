# 💰 Controle Financeiro

Sistema web para controle de lançamentos financeiros, desenvolvido como teste técnico. Permite registrar, listar e gerenciar entradas e saídas financeiras com uma API RESTful em ASP.NET Core e persistência em SQL Server.

---

## 🛠️ Tecnologias utilizadas

- **ASP.NET Core** — framework para a API web
- **C#** — linguagem principal
- **ADO.NET** — acesso a dados sem ORM
- **SQL Server** — banco de dados relacional
- **xUnit** — testes unitários (projetos `Business.Tests` e `Data.Tests`)

---

## 🏗️ Estrutura do projeto

```
controle-financeiro/
├── Api/              # Camada de apresentação: controllers, configuração da aplicação
├── Business/         # Regras de negócio e serviços
├── Business.Tests/   # Testes unitários da camada de negócio
├── Data/             # Repositórios e acesso ao banco via ADO.NET
├── Data.Tests/       # Testes unitários da camada de dados
└── ControleFinanceiro.slnx
```

---

## 🚀 Como rodar localmente

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (ou SQL Server Express)

### Passo a passo

**1. Clone o repositório**
```bash
git clone https://github.com/guilhermevictorlima/controle-financeiro.git
cd ControleFinanceiro
```

**2. Configure a connection string**

Em `Data.Repositories.RepositoryBase`, ajuste a string de conexão com seu SQL Server:

```csharp
  private readonly string connectionString = "Server=localhost;Database={DATABASE};User Id={USER};Password={PASSWORD};TrustServerCertificate=True;";
```

**3. Crie o banco de dados**

Execute o script SQL de criação da tabela disponível em `Data/Seeds/` diretamente no SQL Server Management Studio ou via `sqlcmd`.

**4. Execute a aplicação**
```bash
cd Api
dotnet run
```

A aplicação estará disponível em `http://localhost:7265`.

**5. Execute os testes**
```bash
cd ControleFinanceiro
dotnet test
```

---

## 🧠 Decisões técnicas

### RepositoryBase genérico
Implementei um RepositoryBase<TEntity> genérico que centraliza toda a infraestrutura de acesso a dados (abertura de conexão, mapeamento de parâmetros e execução de queries) expondo para as subclasses apenas a responsabilidade de mapear o SqlDataReader para a entidade concreta. A decisão foi pensada para escala: à medida que novos repositórios forem criados, eles herdam esse comportamento sem repetição, e qualquer mudança transversal (tratamento de erros, logging, timeout) tem um único ponto de alteração.


### Estrutura de Validators
Para manter os serviços com responsabilidade única, a lógica de validação foi extraída para classes dedicadas organizadas em hierarquia. A classe abstrata base (`LancamentoFinanceiroValidator`) fornece o mecanismo de execução, iterando um dicionário de violações e lançando exceção na primeira encontrada. A camada intermediária (`CriacaoEdicaoLancamentoFinanceiroValidator`) concentra as validações compartilhadas entre criação e edição. As classes concretas (`CadastroLancamentoFinanceiroValidator`, `EdicaoLancamentoFinanceiroValidator`, `AlteracaoStatusLancamentoValidator`) orquestram a sequência de validações específica de cada operação.

### Exporters (Strategy)
A exportação de lançamentos utiliza o padrão Strategy: a interface ILancamentoFinanceiroExporter define o contrato, CsvLancamentoFinanceiroExporter e ExcelLancamentoFinanceiroExporter encapsulam cada algoritmo de forma isolada, e LancamentoFinanceiroExporterFactory resolve a estratégia em runtime a partir do TipoExportacao. Adicionar um novo formato se resume a criar uma nova implementação da interface e registrá-la na factory, sem tocar no restante do sistema.

### ASP.NET Core Web API
O desafio propunha o uso de Web Forms. Durante o desenvolvimento, encontrei divergências de versões entre os projetos que inviabilizaram essa abordagem sem um retrabalho desproporcional ao tempo restante. Optei então por ASP.NET Core Web API, que resolve o mesmo problema de expor os dados via HTTP e se encaixa naturalmente na arquitetura em camadas já adotada, mas reconheço que isso representa um desvio da especificação original.
