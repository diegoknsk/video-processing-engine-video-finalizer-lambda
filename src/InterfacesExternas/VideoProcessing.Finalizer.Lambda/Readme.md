# Video Processing Finalizer — Lambda

Lambda que consolida frames de múltiplos chunks no S3 em um único arquivo ZIP e envia o resultado para um bucket de saída. Pensada para ser invocada por Step Functions ou via SQS.

## Contrato de entrada

O handler aceita:

1. **Payload direto** (ex.: Step Functions): JSON com os campos abaixo.
2. **Envelope SQS**: objeto com `Records[]` onde cada item tem `body` contendo o mesmo JSON.

| Campo               | Tipo   | Obrigatório | Descrição                                                                 |
|---------------------|--------|-------------|----------------------------------------------------------------------------|
| `framesBucket`      | string | Sim         | Nome do bucket S3 onde estão os frames (chunk-XXX/frames/).               |
| `framesBasePrefix`  | string | Sim         | Prefixo base no bucket (ex.: `processed/video-id/job-id`).                 |
| `outputBucket`      | string | Sim         | Nome do bucket S3 onde o ZIP consolidado será enviado.                    |

### Exemplo de payload (invocação direta)

```json
{
  "framesBucket": "meu-bucket-origem",
  "framesBasePrefix": "processed/video-123/job-456",
  "outputBucket": "meu-bucket-destino"
}
```

O `jobId` é derivado do último segmento de `framesBasePrefix` (ex.: `job-456`). O ZIP será salvo em:

- **Bucket:** `outputBucket`
- **Chave:** `consolidated/<framesBasePrefix>/<jobId>_frames.zip`  
  Ex.: `consolidated/processed/video-123/job-456/job-456_frames.zip`

## Como testar a Lambda

### 1. Ajustar o payload de exemplo

Edite `sample-payload.json` na raiz do projeto Lambda com seus buckets e prefixo reais:

```json
{
  "framesBucket": "SEU-BUCKET-DE-ORIGEM",
  "framesBasePrefix": "processed/video-id/job-id",
  "outputBucket": "SEU-BUCKET-DE-DESTINO"
}
```

Garanta que no bucket de origem existam objetos sob esse prefixo em estrutura do tipo `chunk-XXX/frames/*.jpg` (ou `.jpeg` / `.png`).

### 2. Invocar pela AWS CLI (Lambda já publicada)

```bash
aws lambda invoke \
  --function-name NOME-DA-SUA-FUNCAO \
  --payload fileb://sample-payload.json \
  --cli-binary-format raw-in-base64-out \
  response.json

cat response.json
```

### 3. Invocar com Amazon.Lambda.Tools (local ou publicada)

Instale o global tool (se ainda não tiver):

```bash
dotnet tool install -g Amazon.Lambda.Tools
```

**Contra a função publicada** (substitua o nome e o perfil/região se precisar):

```bash
cd src/InterfacesExternas/VideoProcessing.Finalizer.Lambda

dotnet lambda invoke-function \
  VideoProcessingFinalizerLambda \
  --payload fileb://sample-payload.json
```

**Apenas build** (para garantir que compila):

```bash
dotnet build
```

### 4. Resposta esperada

Em caso de sucesso, a Lambda retorna um JSON como:

```json
{
  "zipS3Key": "consolidated/processed/video-123/job-456/job-456_frames.zip",
  "framesCount": 42
}
```

Erros (payload inválido, prefixo sem frames, falha de S3, etc.) resultam em exceção e mensagem apropriada nos logs (CloudWatch ou saída do invoke).

## Pré-requisitos

- Bucket de origem com frames em `chunk-XXX/frames/` sob o `framesBasePrefix`.
- Permissões IAM da Lambda: `s3:GetObject` no bucket de origem e `s3:PutObject` no bucket de destino.
- Espaço em `/tmp` suficiente (ajustar `EphemeralStorage` da função se houver muitos frames).

## Deploy e testes unitários

**Testes** (a partir da raiz do repositório):

```bash
dotnet test
```

**Deploy** (a partir da raiz do repositório):

```bash
dotnet lambda deploy-function --project-location src/InterfacesExternas/VideoProcessing.Finalizer.Lambda
```

Ou pelo Visual Studio: clique direito no projeto → *Publish to AWS Lambda*.
