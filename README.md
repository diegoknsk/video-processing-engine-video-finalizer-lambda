# Video Processing Engine - Lambda Finalizer

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=diegoknsk_video-processing-engine-video-finalizer-lambda&metric=alert_status)](https://sonarcloud.io/summary/new_code?project=diegoknsk_video-processing-engine-video-finalizer-lambda)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=diegoknsk_video-processing-engine-video-finalizer-lambda&metric=coverage)](https://sonarcloud.io/summary/new_code?project=diegoknsk_video-processing-engine-video-finalizer-lambda)

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

### SonarCloud

O pipeline executa análise estática e cobertura de código no SonarCloud em **push** e **pull request** para `main`. Para ativar:

1. **Secrets (Settings > Secrets and variables > Actions):**
   - `SONAR_TOKEN` — Token gerado em [sonarcloud.io/account/security](https://sonarcloud.io/account/security).

2. **Variables (Settings > Variables > Actions):**
   - `SONAR_PROJECT_KEY` — Chave do projeto no SonarCloud (ex.: `minha-org_video-processing-engine-video-finalizer-lambda`).
   - `SONAR_ORGANIZATION` — Slug da organização no SonarCloud.

3. **SonarCloud (Administration → Analysis Method):** Desative **Automatic Analysis** para evitar falha do pipeline com "You are running CI analysis while Automatic Analysis is enabled".

4. **Branch Protection (Settings > Branches):** Na regra da branch `main`, habilite "Require status checks to pass before merging" e adicione o check **SonarCloud Analysis**. No SonarCloud (Project Settings > GitHub), ative o webhook para reportar o Quality Gate na PR.

5. **Badges no README:** Os badges usam o project key `diegoknsk_video-processing-engine-video-finalizer-lambda` (o `id` na URL do projeto no SonarCloud). Se usar outra org/user, configure `SONAR_PROJECT_KEY` no GitHub e altere esse valor nas URLs dos badges.

Para armadilhas comuns e checklist completo, consulte `.cursor/skills/sonarcloud-dotnet/SKILL.md`.

### Triggers

- **Push para `main`:** Análise SonarCloud, build, testes e deploy automático
- **Pull Request para `main`:** Análise SonarCloud, build e testes (sem deploy)
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

**Cobertura mínima:** 80% (meta da story). O CI usa Coverlet (OpenCover) para o SonarCloud.

```bash
dotnet test --collect:"XPlat Code Coverage"
# ou com OpenCover (compatível com SonarCloud):
dotnet test /p:CollectCoverage=true /p:CoverageReporter=opencover /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage.opencover.xml
```

O relatório é gerado em `**/TestResults/**/coverage.opencover.xml` (OpenCover) ou `coverage.cobertura.xml` (XPlat).

## Pré-condições para Deploy

- A função Lambda `video-processing-engine-dev-finalizer` deve estar provisionada na AWS (via IaC)
- Runtime: .NET 10 (ou compatível)
- Handler configurado: `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`
- Credenciais AWS configuradas nos GitHub Secrets
