# Subtask 02: Listar e baixar frames de múltiplos chunks no S3

## Descrição
Implementar a lógica de descoberta e download de frames: usar `ListObjectsV2` do SDK S3 para varrer recursivamente todo o prefixo base (`framesBasePrefix`) e coletar todas as chaves de imagem (`.jpg`, `.jpeg`, `.png`) presentes em qualquer subdiretório `chunk-XXX/frames/`. Em seguida, baixar cada imagem para `/tmp/frames/<chunk>/<filename>` preservando a estrutura relativa.

## Passos de Implementação
1. Criar `FramesZipService` com método `ListAllFrameKeysAsync(string bucket, string basePrefix)` que executa `ListObjectsV2Request` com `Prefix = basePrefix`, paginando via `ContinuationToken` até `IsTruncated == false`, filtrando apenas chaves que terminem em `.jpg`, `.jpeg` ou `.png`.
2. Implementar `DownloadFramesAsync(string bucket, IEnumerable<string> keys, string localDir)` que, para cada chave, cria o subdiretório espelhando a estrutura relativa ao `basePrefix` dentro de `/tmp/frames/` e faz `GetObjectAsync` + stream para o arquivo local.
3. Garantir que o diretório `/tmp/frames/` é limpo e recriado no início de cada execução (`Directory.Delete(path, true)` + `Directory.CreateDirectory`) para evitar contaminação de invocações anteriores (warm start).

## Formas de Teste
1. **Unitário (mock):** mockar `IAmazonS3` para retornar uma lista paginada simulada de 3 chaves de imagem e verificar que `ListAllFrameKeysAsync` retorna exatamente essas 3 chaves, sem duplicatas.
2. **Unitário (mock):** mockar `GetObjectAsync` e verificar que `DownloadFramesAsync` cria os arquivos locais nos caminhos corretos dentro de `/tmp/frames/`.
3. **Unitário:** passar lista vazia de chaves e verificar que método lança `InvalidOperationException` com mensagem "Nenhum frame encontrado no prefixo informado".

## Critérios de Aceite
- [x] `ListAllFrameKeysAsync` pagina corretamente quando `IsTruncated == true` e retorna todas as chaves de imagem
- [x] Apenas arquivos com extensão `.jpg`, `.jpeg` ou `.png` são incluídos (case-insensitive)
- [x] Diretório `/tmp/frames/` é recriado limpo a cada execução
- [x] Exceção descritiva é lançada quando nenhum frame é encontrado
