# Subtask-02: Atualizar BuildZipS3Key para usar outputBasePrefix

## Descrição
Alterar a assinatura e a lógica do método estático `BuildZipS3Key` em `FramesZipService` para receber `outputBasePrefix` e `videoId` como parâmetros, montando a chave S3 de destino no formato `<outputBasePrefix>/<videoId>_frames.zip`. Remover a dependência de `framesBasePrefix` nessa montagem.

## Passos de Implementação
1. Abrir `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FramesZipService.cs`.
2. Alterar a assinatura de `BuildZipS3Key(string framesBasePrefix, string jobId)` para `BuildZipS3Key(string outputBasePrefix, string videoId)`.
3. Atualizar o corpo do método para retornar `$"{outputBasePrefix.Trim().TrimEnd('/')}/{videoId}_frames.zip"`.
4. O método `GetJobIdFromPrefix` pode ser mantido para outros usos ou removido se não for mais referenciado; verificar e ajustar conforme necessário.
5. Atualizar a interface `IFramesZipService` (se `BuildZipS3Key` estiver na interface) para refletir a nova assinatura.

## Formas de Teste
1. Testar `BuildZipS3Key("abc123/def456", "def456")` e verificar que o resultado é `"abc123/def456/def456_frames.zip"`.
2. Testar com `outputBasePrefix` com barra no final e confirmar que é normalizado corretamente.
3. Verificar que a chave gerada **não** começa mais com `"consolidated/"`.

## Critérios de Aceite
- [ ] `BuildZipS3Key(outputBasePrefix, videoId)` retorna `"<outputBasePrefix>/<videoId>_frames.zip"` com o prefixo normalizado (sem barra dupla).
- [ ] O padrão antigo `"consolidated/<framesBasePrefix>/<jobId>_frames.zip"` não é mais gerado pelo método.
- [ ] Todos os testes unitários existentes que chamam `BuildZipS3Key` são atualizados para usar a nova assinatura.
