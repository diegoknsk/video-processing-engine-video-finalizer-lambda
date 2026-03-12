# Subtask 04: Criar testes unitários com mock de S3

## Descrição
Implementar testes unitários para o `S3Service` (com mock do `IAmazonS3`) e atualizar testes do handler `FunctionHandler` para cobrir o novo fluxo S3, incluindo cenários de sucesso, falhas, paginação, e limpeza de recursos.

## Passos de Implementação
1. Adicionar pacote de mocking ao projeto de testes:
   - `dotnet add test/VideoProcessing.Finalizer.Tests/VideoProcessing.Finalizer.Tests.csproj package Moq --version 4.20.70`
2. Criar `S3ServiceTests.cs` em `test/VideoProcessing.Finalizer.Tests/Services/`:
   - **Teste 1:** `ListObjectKeysAsync_WithMultiplePages_ReturnsAllKeys`
     - Mockar `IAmazonS3` para retornar múltiplas páginas de objetos
     - Verificar que todos os objetos são retornados via `IAsyncEnumerable`
   - **Teste 2:** `DownloadObjectAsync_WithValidKey_ReturnsContent`
     - Mockar `GetObjectAsync` para retornar stream com conteúdo de teste
     - Verificar que byte[] retornado contém conteúdo esperado
   - **Teste 3:** `UploadObjectAsync_WithValidFile_UploadsSuccessfully`
     - Mockar `PutObjectAsync` para retornar sucesso
     - Verificar que método retorna chave correta
   - **Teste 4:** `ListObjectKeysAsync_WithS3Exception_ThrowsException`
     - Mockar `ListObjectsV2Async` para lançar `AmazonS3Exception`
     - Verificar que exceção é propagada
3. Atualizar `FunctionTests.cs` para cobrir fluxo S3:
   - **Teste 1:** `FunctionHandler_WithS3Event_ReturnsSuccess`
     - Criar `FinalizerEvent` com propriedades S3
     - Mockar `IS3Service` para retornar 3 chaves e downloads bem-sucedidos
     - Verificar que `Response.Success` é `true` e `ProcessedFileCount` é 3
   - **Teste 2:** `FunctionHandler_WithS3EmptyPrefix_ReturnsSuccessWithZeroFiles`
     - Mockar `ListObjectKeysAsync` para retornar lista vazia
     - Verificar comportamento apropriado
   - **Teste 3:** `FunctionHandler_WithS3DownloadFailure_ReturnsFailure`
     - Mockar `DownloadObjectAsync` para lançar `AmazonS3Exception`
     - Verificar que `Response.Success` é `false` e `ErrorMessage` contém informação relevante
   - **Teste 4:** `FunctionHandler_WithS3Mode_UploadsZipToOutputBucket`
     - Verificar que `UploadObjectAsync` é chamado com bucket e chave corretos
4. Configurar cobertura:
   - Executar `dotnet test --collect:"XPlat Code Coverage"`
   - Verificar cobertura ≥ 80% para `S3Service` e `Function`

## Formas de Teste
1. **Execução local:**
   - Executar `dotnet test` e verificar que todos os testes passam
2. **Cobertura:**
   - Analisar relatório de cobertura e identificar gaps
3. **Validação de mocks:**
   - Verificar que mocks simulam corretamente comportamento do S3 SDK

## Critérios de Aceite da Subtask
- [ ] Pacote `Moq` adicionado ao projeto de testes
- [ ] Classe `S3ServiceTests` criada com mínimo 4 testes unitários
- [ ] Classe `FunctionTests` atualizada com mínimo 4 testes para fluxo S3
- [ ] Testes cobrem cenários: múltiplos objetos, paginação, bucket vazio, erro de download, erro de upload
- [ ] Todos os testes passam localmente (`dotnet test`)
- [ ] Cobertura de código de `S3Service` e `Function` ≥ 80%
- [ ] Mocks de `IAmazonS3` e `IS3Service` implementados corretamente
- [ ] Testes executam no workflow GitHub Actions
