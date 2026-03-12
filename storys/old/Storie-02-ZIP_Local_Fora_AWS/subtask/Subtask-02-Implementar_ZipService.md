# Subtask 02: Implementar ZipService com lógica de criação de ZIP

## Descrição
Implementar a classe `ZipService` que consome arquivos de um diretório local, cria um arquivo ZIP contendo todos os arquivos encontrados, e retorna informações sobre o resultado da operação (quantidade de arquivos, tamanho do ZIP).

## Passos de Implementação
1. Criar classe `ZipService.cs` em `src/VideoProcessing.Finalizer/Services/`:
   - Implementar interface `IZipService`
   - Injetar `ILogger<ZipService>` via construtor primário
2. Implementar método `CreateZipFromDirectoryAsync`:
   - Validar que `SourceDirectory` existe; retornar `Failure` se não existir
   - Obter lista de arquivos do diretório (usar `Directory.GetFiles` com padrão `*.*`)
   - Logar quantidade de arquivos encontrados
   - Usar `ZipFile.CreateFromDirectory` para gerar ZIP
   - Obter tamanho do arquivo ZIP gerado (`FileInfo.Length`)
   - Logar sucesso com quantidade de arquivos e tamanho
   - Retornar `ZipCreationResult.Success` com informações
3. Implementar tratamento de exceções:
   - Catch `IOException`, `UnauthorizedAccessException`, `DirectoryNotFoundException`
   - Logar erro e retornar `ZipCreationResult.Failure` com mensagem de erro
4. Adicionar logging estruturado:
   - Log de início: "Starting ZIP creation from directory {SourceDirectory}"
   - Log de arquivos encontrados: "Found {FileCount} files in directory"
   - Log de sucesso: "ZIP created successfully at {OutputZipPath}, size: {ZipSizeBytes} bytes"
   - Log de erro: "Failed to create ZIP: {ErrorMessage}"
5. Considerar uso de `Task.Run` se operação for síncrona (opcional nesta fase)

## Formas de Teste
1. **Teste de compilação:**
   - Executar `dotnet build` e verificar ausência de erros
2. **Teste manual com harness (Subtask 03):**
   - Criar diretório local com imagens de teste
   - Invocar `CreateZipFromDirectoryAsync` e verificar ZIP gerado
3. **Teste de cenários de erro:**
   - Testar com diretório inexistente e verificar `Failure` retornado
   - Testar com caminho de saída inválido (ex.: sem permissão de escrita)

## Critérios de Aceite da Subtask
- [ ] Classe `ZipService` implementa interface `IZipService`
- [ ] Método `CreateZipFromDirectoryAsync` valida existência do diretório de origem
- [ ] ZIP criado contém todos os arquivos do diretório de origem
- [ ] `ZipCreationResult` retornado contém quantidade de arquivos e tamanho do ZIP
- [ ] Logging estruturado implementado com `ILogger<ZipService>`
- [ ] Tratamento de exceções implementado para cenários de erro (diretório inexistente, permissões, I/O)
- [ ] Build da solution executado com sucesso
- [ ] Código segue convenções .NET: construtor primário, async/await, tratamento de exceções
