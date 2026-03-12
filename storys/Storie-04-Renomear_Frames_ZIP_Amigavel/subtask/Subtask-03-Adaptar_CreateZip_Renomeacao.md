# Subtask 03: Adaptar CreateZip para renomear e colocar frames na raiz do ZIP

## Descrição
Modificar `FramesZipService.CreateZip` para aceitar o parâmetro `bool ordenaAutomaticamente`. Quando `true`, usar `FrameRenamer.GenerateRenamedEntries` para obter os pares `(localPath, entryName)` e inserir cada arquivo diretamente na raiz do ZIP com o `entryName` calculado (sem subdiretórios). Quando `false`, manter o comportamento atual (estrutura de diretórios relativa ao `framesDir`).

## Passos de Implementação
1. Alterar a assinatura de `CreateZip` em `IFramesZipService` e `FramesZipService`:
   ```csharp
   void CreateZip(string framesDir, string zipPath, bool ordenaAutomaticamente = true);
   ```
2. Substituir a chamada `ZipFile.CreateFromDirectory` por construção manual com `ZipArchive`:
   - Quando `ordenaAutomaticamente = true`:
     - Listar arquivos com `Directory.GetFiles(framesDir, "*", SearchOption.AllDirectories)`.
     - Chamar `FrameRenamer.GenerateRenamedEntries(files)` para obter `(localPath, entryName)`.
     - Para cada par: `archive.CreateEntryFromFile(localPath, entryName, CompressionLevel.Fastest)`.
   - Quando `ordenaAutomaticamente = false`:
     - Manter lógica equivalente à anterior (nome relativo ao `framesDir`).
3. Garantir que a validação de diretório vazio e criação do diretório do ZIP permaneçam.

## Formas de Teste
1. Teste unitário: criar diretório com arquivos `chunk1/frames/frame_0001_0s.jpg` e `chunk2/frames/frame_0001_5s.jpg`, chamar `CreateZip` com `ordenaAutomaticamente = true`; abrir ZIP e verificar que as entradas estão na raiz com nomes renomeados.
2. Teste unitário: chamar `CreateZip` com `ordenaAutomaticamente = false`; verificar que a estrutura relativa de diretórios foi preservada.
3. Teste unitário: diretório vazio deve lançar `InvalidOperationException`.

## Critérios de Aceite
- [ ] Com `ordenaAutomaticamente = true`, as entradas do ZIP estão na raiz (nenhuma barra `/` no `entryName`)
- [ ] Com `ordenaAutomaticamente = true`, os arquivos estão renomeados e ordenados por instante de tempo
- [ ] Com `ordenaAutomaticamente = false`, a estrutura relativa de diretórios é preservada (comportamento anterior)
- [ ] Assinatura de `IFramesZipService.CreateZip` atualizada e compatível com a implementação
- [ ] Nenhuma exceção é lançada para arquivos sem sufixo de tempo quando `ordenaAutomaticamente = true`
