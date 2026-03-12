# Subtask 01: Criar modelos de evento e resposta para o handler

## Descrição
Criar os modelos de entrada (`FinalizerEvent`) e saída (`FinalizerResponse`) que definem o contrato da Lambda Finalizer, especificando a estrutura do evento recebido e da resposta retornada.

## Passos de Implementação
1. Criar record `FinalizerEvent.cs` em `src/VideoProcessing.Finalizer/Models/`:
   - Propriedades:
     - `string VideoId` (identificador do vídeo)
     - `List<FileItem> Files` (lista de arquivos a processar)
   - Criar nested record `FileItem`:
     - `string FileName` (nome do arquivo)
     - `string ContentBase64` (conteúdo do arquivo em base64) — simulação inicial
   - Adicionar validação básica (ex.: `Files` não pode ser null)
2. Criar record `FinalizerResponse.cs`:
   - Propriedades:
     - `bool Success`
     - `string VideoId`
     - `string? ZipFilePath` (caminho do ZIP em /tmp)
     - `int ProcessedFileCount`
     - `long ZipSizeBytes`
     - `string? ErrorMessage`
   - Incluir métodos estáticos `Success(...)` e `Failure(...)`
3. Documentar modelos com XML comments, incluindo exemplos de payload JSON
4. Adicionar exemplo de payload JSON no README.md:
   ```json
   {
     "videoId": "video-123",
     "files": [
       {
         "fileName": "frame-001.jpg",
         "contentBase64": "iVBORw0KG..."
       }
     ]
   }
   ```

## Formas de Teste
1. **Compilação:**
   - Executar `dotnet build` e verificar ausência de erros
2. **Serialização/Deserialização:**
   - Criar instância de `FinalizerEvent` e serializar para JSON usando `System.Text.Json`
   - Deserializar JSON de exemplo e verificar que propriedades são populadas corretamente
3. **Validação de estrutura:**
   - Verificar que records são imutáveis
   - Confirmar que nested record `FileItem` está acessível

## Critérios de Aceite da Subtask
- [ ] Record `FinalizerEvent` criado com propriedades `VideoId` e `Files`
- [ ] Nested record `FileItem` criado com `FileName` e `ContentBase64`
- [ ] Record `FinalizerResponse` criado com propriedades `Success`, `VideoId`, `ZipFilePath`, `ProcessedFileCount`, `ZipSizeBytes`, `ErrorMessage`
- [ ] Métodos estáticos `Success` e `Failure` implementados em `FinalizerResponse`
- [ ] Modelos documentados com XML comments e exemplo de payload JSON
- [ ] Build da solution executado com sucesso
- [ ] README.md atualizado com exemplo de payload de entrada
