# Subtask 01: Criar modelos de request/result e interface IZipService

## Descrição
Criar os modelos de entrada (`ZipCreationRequest`) e saída (`ZipCreationResult`) para a operação de criação de ZIP, além da interface `IZipService` que define o contrato do serviço.

## Passos de Implementação
1. Criar pasta `src/VideoProcessing.Finalizer/Models/`
2. Criar record `ZipCreationRequest.cs`:
   - Propriedades: `string SourceDirectory` (diretório de origem), `string OutputZipPath` (caminho do ZIP de saída)
   - Validar que ambos os campos são obrigatórios (init-only properties)
3. Criar record `ZipCreationResult.cs`:
   - Propriedades: `bool Success`, `string OutputZipPath`, `int FileCount`, `long ZipSizeBytes`, `string? ErrorMessage`
   - Incluir método estático `Success(...)` e `Failure(...)`
4. Criar pasta `src/VideoProcessing.Finalizer/Services/`
5. Criar interface `IZipService.cs`:
   - Método: `Task<ZipCreationResult> CreateZipFromDirectoryAsync(ZipCreationRequest request, CancellationToken cancellationToken = default)`
6. Documentar modelos e interface com XML comments

## Formas de Teste
1. **Compilação:**
   - Executar `dotnet build` e verificar que não há erros de compilação
2. **Inspeção de código:**
   - Verificar que records são imutáveis (init-only properties)
   - Confirmar que interface usa async/await e `CancellationToken`
3. **Teste de estrutura:**
   - Instanciar `ZipCreationRequest` com valores de teste e verificar propriedades
   - Criar instâncias de `ZipCreationResult` usando métodos `Success` e `Failure`

## Critérios de Aceite da Subtask
- [ ] Record `ZipCreationRequest` criado com propriedades `SourceDirectory` e `OutputZipPath`
- [ ] Record `ZipCreationResult` criado com propriedades `Success`, `OutputZipPath`, `FileCount`, `ZipSizeBytes`, `ErrorMessage`
- [ ] Métodos estáticos `Success` e `Failure` implementados em `ZipCreationResult`
- [ ] Interface `IZipService` criada com método `CreateZipFromDirectoryAsync`
- [ ] Todos os tipos documentados com XML comments
- [ ] Build da solution executado com sucesso
- [ ] Estrutura segue convenções .NET: records imutáveis, async/await, CancellationToken
