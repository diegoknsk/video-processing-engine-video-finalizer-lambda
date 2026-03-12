# Subtask 02: Criar TempStorageService para gerenciar arquivos em /tmp

## Descrição
Implementar serviço `TempStorageService` responsável por gerenciar operações de escrita, leitura e limpeza de arquivos no diretório `/tmp` do Lambda, abstraindo a lógica de I/O e garantindo uso seguro do armazenamento temporário.

## Passos de Implementação
1. Criar interface `ITempStorageService.cs` em `src/VideoProcessing.Finalizer/Services/`:
   - Métodos:
     - `Task<string> CreateWorkingDirectoryAsync(string videoId, CancellationToken cancellationToken = default)` — cria subdiretório em /tmp para o vídeo
     - `Task WriteFileAsync(string filePath, byte[] content, CancellationToken cancellationToken = default)` — escreve arquivo em /tmp
     - `Task CleanupDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default)` — limpa diretório após processamento
     - `long GetFileSize(string filePath)` — retorna tamanho do arquivo
2. Criar classe `TempStorageService.cs`:
   - Injetar `ILogger<TempStorageService>` via construtor primário
   - Implementar `CreateWorkingDirectoryAsync`:
     - Criar diretório `/tmp/finalizer/{videoId}`
     - Logar criação do diretório
     - Retornar caminho completo do diretório criado
   - Implementar `WriteFileAsync`:
     - Usar `File.WriteAllBytesAsync` para escrever conteúdo
     - Logar escrita do arquivo com nome e tamanho
   - Implementar `CleanupDirectoryAsync`:
     - Deletar diretório recursivamente usando `Directory.Delete(path, recursive: true)`
     - Logar limpeza
     - Tratar exceções (ex.: arquivo em uso, permissões)
   - Implementar `GetFileSize`:
     - Usar `FileInfo.Length`
3. Adicionar validação de limites:
   - Opcional: verificar espaço disponível em /tmp antes de escrever (ex.: `DriveInfo`)
   - Logar warning se espaço disponível for menor que 100 MB
4. Adicionar logging estruturado:
   - Log de criação de diretório: "Created working directory at {DirectoryPath}"
   - Log de escrita: "Wrote file {FileName}, size: {FileSizeBytes} bytes"
   - Log de limpeza: "Cleaned up directory {DirectoryPath}"

## Formas de Teste
1. **Teste de compilação:**
   - Executar `dotnet build` e verificar ausência de erros
2. **Teste unitário (Subtask 04):**
   - Simular `/tmp` usando diretório temporário local (`Path.GetTempPath()`)
   - Testar criação, escrita, leitura e limpeza
3. **Teste no Lambda (Subtask 05):**
   - Validar que operações funcionam corretamente no ambiente Lambda

## Critérios de Aceite da Subtask
- [ ] Interface `ITempStorageService` criada com métodos para criar diretório, escrever arquivo, limpar, obter tamanho
- [ ] Classe `TempStorageService` implementa interface com logging estruturado
- [ ] Método `CreateWorkingDirectoryAsync` cria subdiretório em `/tmp/finalizer/{videoId}`
- [ ] Método `WriteFileAsync` escreve arquivos de forma assíncrona usando `File.WriteAllBytesAsync`
- [ ] Método `CleanupDirectoryAsync` deleta diretório recursivamente com tratamento de exceções
- [ ] Logging estruturado implementado para todas as operações
- [ ] Build da solution executado com sucesso
- [ ] Código segue convenções .NET: async/await, CancellationToken, tratamento de exceções
