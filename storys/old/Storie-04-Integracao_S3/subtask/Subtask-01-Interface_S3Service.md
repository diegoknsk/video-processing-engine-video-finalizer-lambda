# Subtask 01: Criar interface e implementação do S3Service

## Descrição
Criar interface `IS3Service` e implementação `S3Service` para abstrair operações com Amazon S3, incluindo listagem de objetos por prefixo com paginação, download de objetos, e upload de arquivos.

## Passos de Implementação
1. Adicionar pacote `AWSSDK.S3` ao projeto:
   - Executar `dotnet add package AWSSDK.S3 --version 3.7.400.3`
2. Criar interface `IS3Service.cs` em `src/VideoProcessing.Finalizer/Services/`:
   - Métodos:
     - `IAsyncEnumerable<string> ListObjectKeysAsync(string bucketName, string prefix, CancellationToken cancellationToken = default)` — lista chaves de objetos (com paginação)
     - `Task<byte[]> DownloadObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default)` — baixa objeto como byte[]
     - `Task<string> UploadObjectAsync(string bucketName, string key, string filePath, CancellationToken cancellationToken = default)` — faz upload de arquivo; retorna URL ou chave
3. Criar classe `S3Service.cs`:
   - Injetar `IAmazonS3` e `ILogger<S3Service>` via construtor primário
   - Implementar `ListObjectKeysAsync`:
     - Usar `AmazonS3Client.Paginators.ListObjectsV2` para listar com paginação
     - `yield return` cada chave encontrada (suporta >1000 objetos)
     - Logar quantidade de páginas processadas
   - Implementar `DownloadObjectAsync`:
     - Usar `GetObjectAsync` e `ResponseStream.CopyToAsync` para ler conteúdo
     - Retornar byte[]
     - Logar download com tamanho do objeto
   - Implementar `UploadObjectAsync`:
     - Usar `PutObjectAsync` com `FilePath`
     - Logar upload com tamanho do arquivo
     - Retornar chave do objeto no S3
4. Adicionar tratamento de exceções:
   - Catch `AmazonS3Exception` (permissões, bucket não existe, etc.)
   - Logar erros e re-throw ou retornar erro apropriado
5. Adicionar logging estruturado:
   - Log de listagem: "Listing objects in bucket {BucketName} with prefix {Prefix}"
   - Log de download: "Downloaded object {Key} from bucket {BucketName}, size: {SizeBytes} bytes"
   - Log de upload: "Uploaded file to bucket {BucketName}, key: {Key}, size: {SizeBytes} bytes"

## Formas de Teste
1. **Teste de compilação:**
   - Executar `dotnet build` e verificar ausência de erros
2. **Teste unitário com mock (Subtask 04):**
   - Mockar `IAmazonS3` e testar lógica do `S3Service`
3. **Teste de integração (Subtask 05):**
   - Testar com buckets reais na AWS

## Critérios de Aceite da Subtask
- [ ] Pacote `AWSSDK.S3` adicionado ao projeto com versão especificada
- [ ] Interface `IS3Service` criada com métodos para listar (IAsyncEnumerable), baixar, fazer upload
- [ ] Classe `S3Service` implementa interface com injeção de `IAmazonS3` e `ILogger<S3Service>`
- [ ] Método `ListObjectKeysAsync` implementado usando `ListObjectsV2Paginator` para paginação
- [ ] Método `DownloadObjectAsync` implementado com streaming e retorno de byte[]
- [ ] Método `UploadObjectAsync` implementado com upload de arquivo via `PutObjectAsync`
- [ ] Logging estruturado implementado para cada operação (listar, baixar, fazer upload)
- [ ] Tratamento de exceções `AmazonS3Exception` implementado
- [ ] Build da solution executado com sucesso
