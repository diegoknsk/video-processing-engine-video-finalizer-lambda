# Subtask 04: Upload do ZIP ao S3 e limpeza de temporários

## Descrição
Implementar o upload do ZIP gerado para o bucket de destino (`outputBucket`) informado no payload, na chave `consolidated/<framesBasePrefix>/<jobId>_frames.zip`, e garantir a limpeza completa de arquivos temporários em `/tmp` ao final de cada execução — inclusive em casos de erro — usando bloco `finally`. O bucket de destino pode ser diferente do bucket de origem dos frames.

## Passos de Implementação
1. No `FramesZipService`, implementar `UploadZipToS3Async(string outputBucket, string zipLocalPath, string zipS3Key)` usando `PutObjectAsync` com `BucketName = outputBucket` e `FilePath = zipLocalPath`, retornando a chave S3 do arquivo enviado para log.
2. Derivar a chave de destino S3 como `$"consolidated/{framesBasePrefix}/{jobId}_frames.zip"`, mantendo a mesma estrutura de prefixo do input para rastreabilidade. O bucket de escrita vem de `input.OutputBucket`, não do `input.FramesBucket`.
3. No handler (`Function.cs`), envolver todo o pipeline em `try/finally`: no `finally`, remover o diretório `/tmp/frames/` (se existir) e o arquivo `/tmp/<jobId>_frames.zip` (se existir), logando warnings para erros de limpeza sem relançá-los.

## Formas de Teste
1. **Unitário (mock):** mockar `IAmazonS3.PutObjectAsync` e verificar que é chamado com `BucketName`, `Key` e `FilePath` corretos.
2. **Unitário:** simular falha no upload (mock lança exceção) e verificar que a exceção é relançada E que os arquivos temporários foram removidos mesmo assim.
3. **Manual:** após execução completa, verificar no console AWS S3 que o arquivo ZIP está no prefixo `consolidated/...` e que não há arquivos residuais no `/tmp` (verificar via log de limpeza).

## Critérios de Aceite
- [x] ZIP é enviado ao bucket `outputBucket` (do payload) na chave `consolidated/<framesBasePrefix>/<jobId>_frames.zip`
- [x] Limpeza de `/tmp/frames/` e `/tmp/<jobId>_frames.zip` ocorre no bloco `finally`, garantida mesmo em caso de exceção
- [x] Erros de limpeza são logados como `Warning` e não mascaram o erro original do pipeline
- [x] Log de conclusão registra a chave S3 do ZIP enviado
