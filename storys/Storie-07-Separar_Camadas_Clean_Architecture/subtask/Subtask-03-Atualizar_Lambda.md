# Subtask-03: Atualizar Lambda (remover arquivos movidos, novos ProjectReferences)

## Descrição
Remover do projeto Lambda todos os arquivos movidos para Domain/Application/Infra, atualizar o `Lambda.csproj` com referências aos novos projetos, e atualizar `Function.cs` com os novos `using` statements.

## Passos de implementação
1. Deletar `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`.
2. Deletar `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/IFramesZipService.cs`.
3. Deletar `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FramesZipService.cs`.
4. Deletar `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FrameRenamer.cs`.
5. Atualizar `VideoProcessing.Finalizer.Lambda.csproj`: adicionar `ProjectReference` para Application e Infra; manter AWSSDK.S3 e pacotes Lambda.
6. Atualizar `Function.cs`: remover `FinalizerResult` record (movido para Domain), trocar `using VideoProcessing.Finalizer.Lambda.Models` → `using VideoProcessing.Finalizer.Domain.Models`, `using VideoProcessing.Finalizer.Lambda.Services` → `using VideoProcessing.Finalizer.Application.Ports` e `using VideoProcessing.Finalizer.Infra.Services`.

## Formas de teste
1. `dotnet build src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj` compila sem erros.
2. Verificar que as pastas `Models/` e `Services/` foram removidas do projeto Lambda.
3. `Function.cs` não define mais o record `FinalizerResult` nem importa namespaces antigos.

## Critérios de aceite
- [ ] `dotnet build` do projeto Lambda sem erros.
- [ ] `Function.cs` usa somente namespaces dos novos projetos para `FinalizerInput`, `FinalizerResult`, `IFramesZipService` e `FramesZipService`.
- [ ] As pastas `Models/` e `Services/` não existem mais em `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/`.
