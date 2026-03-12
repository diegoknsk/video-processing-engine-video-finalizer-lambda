# Subtask-03: Atualizar Function.cs para repassar os novos campos

## Descrição
Atualizar o handler `Function.cs` para validar os novos campos obrigatórios (`videoId` e `outputBasePrefix`) e repassá-los à chamada de `BuildZipS3Key`, substituindo a lógica anterior que derivava a chave de `framesBasePrefix`.

## Passos de Implementação
1. Abrir `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Function.cs`.
2. Adicionar validação de `input.VideoId` (null/whitespace) antes de iniciar o pipeline; lançar `ArgumentException` com mensagem descritiva se ausente.
3. Adicionar validação de `input.OutputBasePrefix` (null/whitespace) antes de iniciar o pipeline; lançar `ArgumentException` com mensagem descritiva se ausente.
4. Substituir a chamada de `BuildZipS3Key(input.FramesBasePrefix!, jobId)` por `BuildZipS3Key(input.OutputBasePrefix!, input.VideoId!)`.
5. Remover a chamada a `GetJobIdFromPrefix` se não for mais necessária após a mudança.

## Formas de Teste
1. Invocar a Lambda com payload sem `videoId` e verificar que retorna erro com mensagem descritiva antes de acessar o S3.
2. Invocar a Lambda com payload sem `outputBasePrefix` e verificar que retorna erro com mensagem descritiva antes de acessar o S3.
3. Invocar a Lambda com payload completo e verificar que a chave S3 do ZIP segue o padrão `<outputBasePrefix>/<videoId>_frames.zip`.

## Critérios de Aceite
- [ ] Se `videoId` for nulo ou vazio, a Lambda lança `ArgumentException` com mensagem contendo "videoId" antes de executar qualquer operação S3.
- [ ] Se `outputBasePrefix` for nulo ou vazio, a Lambda lança `ArgumentException` com mensagem contendo "outputBasePrefix" antes de executar qualquer operação S3.
- [ ] A chave S3 do ZIP resultante segue o padrão `<outputBasePrefix>/<videoId>_frames.zip`.
- [ ] A chamada obsoleta a `GetJobIdFromPrefix` é removida do `Function.cs` (se não houver mais uso).
