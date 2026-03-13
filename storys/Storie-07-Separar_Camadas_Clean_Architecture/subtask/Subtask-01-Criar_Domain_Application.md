# Subtask-01: Criar projetos Domain e Application em src/Core/

## Descrição
Criar dois projetos Class Library em `src/Core/`: `VideoProcessing.Finalizer.Domain` (modelos de domínio) e `VideoProcessing.Finalizer.Application` (ports/interfaces de casos de uso). Mover `FinalizerInput`, extrair `FinalizerResult` e mover `IFramesZipService` para os namespaces corretos.

## Passos de implementação
1. Criar `src/Core/VideoProcessing.Finalizer.Domain/VideoProcessing.Finalizer.Domain.csproj` (Class Library, net10.0).
2. Criar `src/Core/VideoProcessing.Finalizer.Domain/Models/FinalizerInput.cs` com namespace `VideoProcessing.Finalizer.Domain.Models` (código copiado do Lambda, apenas namespace alterado).
3. Criar `src/Core/VideoProcessing.Finalizer.Domain/Models/FinalizerResult.cs` extraindo o record `FinalizerResult` de `Function.cs` para o novo namespace.
4. Criar `src/Core/VideoProcessing.Finalizer.Application/VideoProcessing.Finalizer.Application.csproj` (Class Library, net10.0) referenciando o projeto Domain.
5. Criar `src/Core/VideoProcessing.Finalizer.Application/Ports/IFramesZipService.cs` com namespace `VideoProcessing.Finalizer.Application.Ports` (código copiado do Lambda, apenas namespace alterado).

## Formas de teste
1. `dotnet build src/Core/VideoProcessing.Finalizer.Domain/VideoProcessing.Finalizer.Domain.csproj` compila sem erros.
2. `dotnet build src/Core/VideoProcessing.Finalizer.Application/VideoProcessing.Finalizer.Application.csproj` compila sem erros.
3. Verificar que `FinalizerInput`, `FinalizerResult` e `IFramesZipService` estão acessíveis nos novos namespaces via IntelliSense ou `dotnet build`.

## Critérios de aceite
- [ ] Os dois projetos existem em `src/Core/` e compilam individualmente.
- [ ] `FinalizerInput` e `FinalizerResult` possuem namespace `VideoProcessing.Finalizer.Domain.Models`.
- [ ] `IFramesZipService` possui namespace `VideoProcessing.Finalizer.Application.Ports`.
