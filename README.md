# Video Processing Engine - Lambda Finalizer

AWS Lambda responsável pela finalização do processamento, consolidando as imagens geradas, criando o arquivo ZIP final e disponibilizando-o no Amazon S3, além de sinalizar a conclusão do processamento.

## Estrutura do Projeto

O repositório segue convenções de Clean Architecture (ver [Estrutura do repositório](#estrutura-do-repositório) abaixo).

```
.
├── src/
│   ├── Core/                           # Reservado para Domain e Application (Clean Architecture)
│   ├── Infra/                          # Reservado para projetos de infraestrutura
│   └── InterfacesExternas/
│       └── VideoProcessing.Finalizer.Lambda/   # Projeto da Lambda
│           ├── Function.cs
│           └── VideoProcessing.Finalizer.Lambda.csproj
├── tests/
│   └── VideoProcessing.Finalizer.Tests/
│       ├── FunctionTests.cs
│       └── VideoProcessing.Finalizer.Tests.csproj
├── .github/
│   └── workflows/
│       └── deploy-lambda.yml           # CI/CD via GitHub Actions
└── README.md
```

## Estrutura do repositório

- **`src/Core/`** — Reservado para projetos Domain e Application (Clean Architecture).
- **`src/Infra/`** — Reservado para projetos de infraestrutura.
- **`src/InterfacesExternas/`** — Pontos de entrada (API, Lambda, handlers). Contém o projeto `VideoProcessing.Finalizer.Lambda`.
- **`tests/`** — Projetos de teste.

O diretório virtual da solution (.slnx) segue essa mesma organização. Convenções de camadas: `.cursor/rules/core-clean-architecture.mdc` e `.cursor/documents/quick-reference.md`. Detalhes e limites da reorganização: [docs/estrutura-repositorio.md](docs/estrutura-repositorio.md).

## Requisitos

- .NET 10 SDK
- AWS CLI (para invocação local e deploy manual)
- Acesso AWS configurado (credenciais ou perfil)

## Como Executar

### Build e Testes Locais

```bash
# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar testes
dotnet test

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Handler Lambda

O handler `FunctionHandler` recebe uma string e retorna a mesma string em maiúsculas (ToUpper). Contrato atual: entrada e saída são `string`.

**Handler (configuração na AWS):** `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`

## Deploy via GitHub Actions

### Configuração de Secrets

Configure os seguintes **Secrets** no repositório: `Settings > Secrets and variables > Actions > Secrets`

| Secret | Descrição |
|--------|-----------|
| `AWS_ACCESS_KEY_ID` | Access Key do IAM User para deploy |
| `AWS_SECRET_ACCESS_KEY` | Secret Access Key correspondente |
| `AWS_SESSION_TOKEN` | (Opcional) Token de sessão quando usar credenciais temporárias |

### Triggers

- **Push para `main`:** Build, testes e deploy automático
- **Pull Request para `main`:** Apenas build e testes (sem deploy)
- **workflow_dispatch:** Execução manual em qualquer branch

### Execução Manual

1. Acesse: `Actions > Deploy Lambda Finalizer > Run workflow`
2. Selecione a branch
3. Clique em `Run workflow`

## Invocação da Lambda

### Via AWS Console

1. Acesse AWS Console → Lambda → `video-processing-engine-dev-finalizer`
2. Crie um evento de teste com payload:
   ```json
   {
     "message": "Test invocation from Console"
   }
   ```
3. Execute o teste e verifique a resposta

### Via AWS CLI

```bash
aws lambda invoke \
  --function-name video-processing-engine-dev-finalizer \
  --payload '{"message":"Test from CLI"}' \
  response.json

cat response.json
```

**Resposta esperada:** string de entrada em maiúsculas (ex.: `"{\"MESSAGE\":\"TEST FROM CLI\"}"`).

## Logs no CloudWatch

Os logs da execução aparecem em:

- **Grupo de logs:** `/aws/lambda/video-processing-engine-dev-finalizer`
- **Conteúdo:** Log de entrada e saída da execução

Para visualizar:

1. AWS Console → CloudWatch → Log groups
2. Localize `/aws/lambda/video-processing-engine-dev-finalizer`
3. Abra o log stream mais recente

## Testes

Os testes unitários validam:

- Retorno em maiúsculas (ToUpper) para entrada válida
- Comportamento com entrada JSON
- Tratamento de input nulo (exceção)
- Entrada vazia

**Cobertura mínima:** 80% (meta da story)

```bash
dotnet test --collect:"XPlat Code Coverage"
```

O relatório é gerado em `tests/.../TestResults/.../coverage.cobertura.xml`.

## Pré-condições para Deploy

- A função Lambda `video-processing-engine-dev-finalizer` deve estar provisionada na AWS (via IaC)
- Runtime: .NET 10 (ou compatível)
- Handler configurado: `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`
- Credenciais AWS configuradas nos GitHub Secrets
