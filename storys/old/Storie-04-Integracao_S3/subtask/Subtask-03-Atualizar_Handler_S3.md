# Subtask 03: Atualizar handler para processar imagens do S3 e fazer upload do ZIP

## Descrição
Atualizar o handler `FunctionHandler` para detectar se o evento contém informações de S3, listar e baixar imagens do bucket de origem, gerar o ZIP em `/tmp`, fazer upload do ZIP para o bucket de saída, e retornar resposta atualizada.

## Passos de Implementação
1. Atualizar `Function.cs`:
   - Injetar `IS3Service` (ou instanciar manualmente nesta fase)
   - Adicionar método auxiliar `bool IsS3Mode(FinalizerEvent input)` que verifica se `S3SourceBucket` está preenchido
2. Atualizar fluxo do handler `FunctionHandler`:
   - **Decisão de modo:** Verificar se é modo S3 ou base64
   - **Se modo S3:**
     - **Passo 1:** Logar "Processing S3 mode for video {VideoId}, bucket: {S3SourceBucket}, prefix: {S3SourcePrefix}"
     - **Passo 2:** Criar diretório de trabalho em `/tmp` usando `TempStorageService`
     - **Passo 3:** Listar objetos S3 usando `S3Service.ListObjectKeysAsync`
     - **Passo 4:** Iterar sobre chaves e baixar cada imagem para `/tmp`:
       - `byte[] content = await S3Service.DownloadObjectAsync(bucket, key)`
       - Salvar arquivo em `/tmp` usando `TempStorageService.WriteFileAsync`
       - Logar download de cada arquivo
     - **Passo 5:** Gerar ZIP usando `ZipService.CreateZipFromDirectoryAsync`
     - **Passo 6:** Fazer upload do ZIP para bucket de saída:
       - Usar `S3Service.UploadObjectAsync(outputBucket, outputKey, zipFilePath)`
       - Logar upload com chave e bucket de destino
     - **Passo 7:** Limpar arquivos temporários em `/tmp` (bloco `finally`)
     - **Passo 8:** Retornar `FinalizerResponse.Success` com informações de S3
   - **Se modo base64:** Manter lógica existente (Storie-03)
3. Implementar geração de chave de saída padrão se não informada:
   - Se `S3OutputKey` estiver vazio, gerar: `videos/{videoId}/output.zip`
4. Adicionar tratamento de exceções:
   - Catch `AmazonS3Exception` e retornar `FinalizerResponse.Failure`
   - Logar exceções com informações de contexto (bucket, prefixo, chave)
5. Adicionar logging estruturado:
   - Log de listagem: "Found {Count} objects in S3"
   - Log de download: "Downloading {Count} images from S3"
   - Log de upload: "Uploading ZIP to S3 bucket {Bucket}, key {Key}"
   - Log de conclusão: "Finalization completed for video {VideoId}"

## Formas de Teste
1. **Teste local com mock de S3 (Subtask 04):**
   - Mockar `IS3Service` e testar fluxo completo
2. **Teste no Lambda com buckets reais (Subtask 05):**
   - Invocar Lambda com evento S3 e verificar ZIP no bucket de saída

## Critérios de Aceite da Subtask
- [ ] Handler atualizado para detectar modo S3 vs base64 usando `IsS3Mode`
- [ ] Fluxo S3 implementado: listar objetos, baixar para /tmp, gerar ZIP, fazer upload
- [ ] Paginação S3 suportada (usando `IAsyncEnumerable` do `S3Service`)
- [ ] Chave de saída padrão gerada se não informada: `videos/{videoId}/output.zip`
- [ ] Logging estruturado implementado em cada etapa do fluxo S3
- [ ] Tratamento de exceções `AmazonS3Exception` implementado
- [ ] Limpeza de arquivos temporários em `/tmp` implementada em bloco `finally`
- [ ] Modo base64 mantido funcional para compatibilidade com testes locais
- [ ] Build da solution executado com sucesso
