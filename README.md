# Video Processing Engine — Lambda Finalizer

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=diegoknsk_video-processing-engine-video-finalizer-lambda&metric=alert_status)](https://sonarcloud.io/summary/new_code?project=diegoknsk_video-processing-engine-video-finalizer-lambda)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=diegoknsk_video-processing-engine-video-finalizer-lambda&metric=coverage)](https://sonarcloud.io/summary/new_code?project=diegoknsk_video-processing-engine-video-finalizer-lambda)

AWS Lambda responsável pela finalização do processamento de vídeo: consolida os frames gerados (imagens no S3), cria o arquivo ZIP final e disponibiliza-o no Amazon S3.

---

## Arquitetura

O projeto segue **Clean Architecture** com separação explícita por camadas:

```
┌──────────────────────────────────────────────────────┐
│  InterfacesExternas                                  │
│  └─ VideoProcessing.Finalizer.Lambda                 │
│     Handler Lambda (ponto de entrada)                │
│     Faz o bootstrap de Infra → Application → Domain  │
└───────────────────┬──────────────────────────────────┘
                    │ usa
┌───────────────────▼──────────────────────────────────┐
│  Core / Application                                  │
│  └─ VideoProcessing.Finalizer.Application            │
│     Ports (interfaces): IFramesZipService            │
└───────────────────┬──────────────────────────────────┘
        usa ▲       │ usa
            │ ┌─────▼──────────────────────────────────┐
            │ │  Core / Domain                         │
            │ │  └─ VideoProcessing.Finalizer.Domain   │
            │ │     Models: FinalizerInput, FinalizerResult │
            │ └────────────────────────────────────────┘
┌───────────┴──────────────────────────────────────────┐
│  Infra                                               │
│  └─ VideoProcessing.Finalizer.Infra                  │
│     Implementações: FramesZipService, FrameRenamer   │
│     Dependência externa: AWSSDK.S3                   │
└──────────────────────────────────────────────────────┘
```

### Responsabilidade de cada camada

| Camada | Projeto | Responsabilidade |
|--------|---------|------------------|
| **Domain** | `VideoProcessing.Finalizer.Domain` | Modelos puros (`FinalizerInput`, `FinalizerResult`); sem dependências externas |
| **Application** | `VideoProcessing.Finalizer.Application` | Contratos/ports (`IFramesZipService`); orquestra casos de uso sem saber de infra |
| **Infra** | `VideoProcessing.Finalizer.Infra` | Implementações que dependem de serviços externos (S3); `FramesZipService`, `FrameRenamer` |
| **Lambda** | `VideoProcessing.Finalizer.Lambda` | Ponto de entrada: parse/validação do input, bootstrapping manual de dependências, invocação do pipeline |
| **Tests** | `VideoProcessing.Finalizer.Tests` | Testes unitários de todas as camadas (xUnit + NSubstitute + FluentAssertions) |

---

## Estrutura do Repositório

```
.
├── VideoProcessing.Finalizer.slnx          # Solution com pastas virtuais
├── src/
│   ├── Core/
│   │   ├── VideoProcessing.Finalizer.Domain/
│   │   │   └── Models/
│   │   │       ├── FinalizerInput.cs
│   │   │       └── FinalizerResult.cs
│   │   └── VideoProcessing.Finalizer.Application/
│   │       └── Ports/
│   │           └── IFramesZipService.cs
│   ├── Infra/
│   │   └── VideoProcessing.Finalizer.Infra/
│   │       ├── Services/
│   │       │   └── FramesZipService.cs
│   │       └── Helpers/
│   │           └── FrameRenamer.cs
│   ├── InterfacesExternas/
│   │   └── VideoProcessing.Finalizer.Lambda/
│   │       └── Function.cs                 # Handler Lambda
│   └── tests/
│       └── VideoProcessing.Finalizer.Tests/
│           ├── FinalizerInputTests.cs
│           ├── FrameRenamerTests.cs
│           ├── FramesZipServiceTests.cs
│           └── FunctionTests.cs
├── storys/                                 # Histórias técnicas
└── .github/
    └── workflows/
        └── deploy-lambda.yml               # CI/CD via GitHub Actions
```

---

## Pipeline de Execução

```
Invocação (Step Functions / SQS)
    │
    ▼
Function.FunctionHandler
    ├─ ParseInput    — suporte a payload direto e envelope SQS (Records[0].body)
    ├─ ValidateInput — valida campos obrigatórios
    │
    ▼
IFramesZipService (implementado por FramesZipService)
    ├─ ListAllFrameKeysAsync   — lista chaves S3 (.jpg/.jpeg/.png) com paginação
    ├─ DownloadFramesAsync     — baixa frames para /tmp/frames/
    ├─ CreateZip               — compacta em ZIP (com renomeação sequencial via FrameRenamer)
    └─ UploadZipToS3Async      — envia ZIP ao bucket de saída
    │
    ▼
FinalizerResult { ZipBucket, ZipS3Key, FramesCount }
```

---

## Contrato de Entrada

### Payload direto (Step Functions)

```json
{
  "framesBucket": "meu-bucket-frames",
  "framesBasePrefix": "processed/guid-usuario/guid-video",
  "outputBucket": "meu-bucket-saida",
  "videoId": "guid-video",
  "outputBasePrefix": "guid-usuario/guid-video",
  "ordenaAutomaticamente": true
}
```

### Envelope SQS

```json
{
  "Records": [
    {
      "body": "{\"framesBucket\":\"meu-bucket-frames\",\"framesBasePrefix\":\"processed/guid-usuario/guid-video\",\"outputBucket\":\"meu-bucket-saida\",\"videoId\":\"guid-video\",\"outputBasePrefix\":\"guid-usuario/guid-video\"}"
    }
  ]
}
```

### Campos do input

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `framesBucket` | string | ✅ | Bucket S3 de origem dos frames |
| `framesBasePrefix` | string | ✅ | Prefixo base dos frames no S3 (ex.: `processed/userGuid/videoGuid`) |
| `outputBucket` | string | ✅ | Bucket S3 de destino do ZIP |
| `videoId` | string | ✅ | Identificador do vídeo (usado no nome do ZIP) |
| `outputBasePrefix` | string | ✅ | Prefixo de saída do ZIP no S3 (ex.: `userGuid/videoGuid`) |
| `ordenaAutomaticamente` | bool | ❌ | Quando `true` (padrão), renomeia frames sequencialmente por instante de tempo |

### Resposta

```json
{
  "zipBucket": "meu-bucket-saida",
  "zipS3Key": "guid-usuario/guid-video/guid-video_frames.zip",
  "framesCount": 42
}
```

---

## Handler Lambda

```
VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler
```

---

## Requisitos

- .NET 10 SDK
- AWS CLI (para invocação local e deploy manual)
- Acesso AWS configurado (credenciais ou perfil)

---

## Como Executar

### Build e Testes Locais

```bash
# Restaurar dependências
dotnet restore VideoProcessing.Finalizer.slnx

# Compilar toda a solution
dotnet build VideoProcessing.Finalizer.slnx

# Executar testes
dotnet test VideoProcessing.Finalizer.slnx

# Executar testes com cobertura (XPlat)
dotnet test --collect:"XPlat Code Coverage"

# Executar testes com cobertura OpenCover (para SonarCloud)
dotnet test /p:CollectCoverage=true /p:CoverageReporter=opencover /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage.opencover.xml
```

---

## Deploy via GitHub Actions

### Configuração de Secrets

Configure os seguintes **Secrets** no repositório: `Settings > Secrets and variables > Actions > Secrets`

| Secret | Descrição |
|--------|-----------|
| `AWS_ACCESS_KEY_ID` | Access Key do IAM User para deploy |
| `AWS_SECRET_ACCESS_KEY` | Secret Access Key correspondente |
| `AWS_SESSION_TOKEN` | (Opcional) Token de sessão quando usar credenciais temporárias |

### Configuração de Variables

Configure as seguintes **Variables** no repositório: `Settings > Secrets and variables > Actions > Variables`

| Variable | Descrição | Exemplo |
|----------|-----------|---------|
| `SONAR_PROJECT_KEY` | Chave do projeto no SonarCloud | `diegoknsk_video-processing-engine-video-finalizer-lambda` |
| `SONAR_ORGANIZATION` | Slug da organização no SonarCloud | `diegoknsk` |

### SonarCloud

O pipeline executa análise estática e cobertura de código no SonarCloud em **push** e **pull request** para `main`. Para ativar:

1. **Secrets:**
   - `SONAR_TOKEN` — Token gerado em [sonarcloud.io/account/security](https://sonarcloud.io/account/security).

2. **Variables:**
   - `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION` conforme tabela acima.

3. **SonarCloud (Administration → Analysis Method):** Desative **Automatic Analysis** para evitar conflito com o pipeline CI.

4. **Branch Protection:** Habilite "Require status checks" e adicione o check **SonarCloud Analysis** na branch `main`.

5. **Badges:** Os badges usam o project key `diegoknsk_video-processing-engine-video-finalizer-lambda`. Se usar outra org/user, ajuste os valores nas URLs dos badges no topo deste README.

### Triggers

- **Push para `main`:** Análise SonarCloud → build → testes → deploy automático
- **Pull Request para `main`:** Análise SonarCloud → build → testes (sem deploy)
- **workflow_dispatch:** Execução manual em qualquer branch

### Execução Manual

1. Acesse: `Actions > Deploy Lambda Finalizer > Run workflow`
2. Selecione a branch
3. Clique em `Run workflow`

---

## Variáveis de Ambiente

A Lambda não utiliza variáveis de ambiente diretamente — toda a configuração vem pelo **payload de invocação** (campos de `FinalizerInput`). As credenciais AWS são fornecidas pela **IAM Role** associada à função Lambda.

| Variável de Ambiente AWS | Descrição |
|--------------------------|-----------|
| `AWS_REGION` | Região padrão (gerenciada pelo runtime Lambda) |
| `AWS_LAMBDA_FUNCTION_NAME` | Nome da função (gerenciado pelo runtime Lambda) |

> Para execução local com `aws lambda invoke`, configure as credenciais via `aws configure` ou variáveis de ambiente (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_SESSION_TOKEN`).

---

## Invocação da Lambda

### Via AWS Console

1. Acesse AWS Console → Lambda → `video-processing-engine-dev-finalizer`
2. Crie um evento de teste com payload:
   ```json
   {
     "framesBucket": "meu-bucket-frames",
     "framesBasePrefix": "processed/guid-usuario/guid-video",
     "outputBucket": "meu-bucket-saida",
     "videoId": "guid-video",
     "outputBasePrefix": "guid-usuario/guid-video"
   }
   ```
3. Execute o teste e verifique a resposta

### Via AWS CLI

```bash
aws lambda invoke \
  --function-name video-processing-engine-dev-finalizer \
  --payload '{"framesBucket":"meu-bucket","framesBasePrefix":"processed/x/y","outputBucket":"meu-out","videoId":"y","outputBasePrefix":"x/y"}' \
  --cli-binary-format raw-in-base64-out \
  response.json

cat response.json
```

**Resposta esperada:**
```json
{"zipBucket":"meu-out","zipS3Key":"x/y/y_frames.zip","framesCount":42}
```

---

## Logs no CloudWatch

Os logs da execução aparecem em:

- **Grupo de logs:** `/aws/lambda/video-processing-engine-dev-finalizer`
- **Conteúdo:** Progresso de cada etapa (listagem, download, zip, upload) e limpeza de temporários

Para visualizar:

1. AWS Console → CloudWatch → Log groups
2. Localize `/aws/lambda/video-processing-engine-dev-finalizer`
3. Abra o log stream mais recente

---

## Testes

Os testes unitários cobrem todas as camadas:

| Arquivo | Camada testada |
|---------|----------------|
| `FinalizerInputTests.cs` | Domain — desserialização e defaults de `FinalizerInput` |
| `FrameRenamerTests.cs` | Infra — extração de tempo e renomeação sequencial de frames |
| `FramesZipServiceTests.cs` | Infra — listagem S3, download, criação de ZIP, upload |
| `FunctionTests.cs` | Lambda — parse de input (direto e SQS), validação, orquestração |

**Cobertura mínima:** 80%. O CI usa Coverlet (OpenCover) para integração com SonarCloud.

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage.opencover.xml
```

---

## Pré-condições para Deploy

- A função Lambda `video-processing-engine-dev-finalizer` deve estar provisionada na AWS (via IaC)
- Runtime: .NET 10
- Handler configurado: `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`
- IAM Role com permissões: `s3:ListObjectsV2`, `s3:GetObject`, `s3:PutObject` nos buckets de entrada e saída
- Credenciais AWS configuradas nos GitHub Secrets
