# Subtask-04: Atualizar projeto de testes (referências e using statements)

## Descrição
Adicionar `ProjectReference` para Domain, Application e Infra no projeto de testes e atualizar os `using` statements de cada arquivo de teste para apontar para os novos namespaces.

## Passos de implementação
1. Atualizar `VideoProcessing.Finalizer.Tests.csproj`: adicionar referências diretas a Domain, Application e Infra (além da referência Lambda já existente).
2. `FinalizerInputTests.cs`: trocar `using VideoProcessing.Finalizer.Lambda.Models` → `using VideoProcessing.Finalizer.Domain.Models`.
3. `FrameRenamerTests.cs`: trocar `using VideoProcessing.Finalizer.Lambda.Services` → `using VideoProcessing.Finalizer.Infra.Helpers`.
4. `FramesZipServiceTests.cs`: trocar `using VideoProcessing.Finalizer.Lambda.Services` → `using VideoProcessing.Finalizer.Infra.Services`.
5. `FunctionTests.cs`: trocar `using VideoProcessing.Finalizer.Lambda.Services` → `using VideoProcessing.Finalizer.Application.Ports`.

## Formas de teste
1. `dotnet build` do projeto de testes sem erros.
2. `dotnet test` — todos os testes existentes passam sem falhas.
3. Confirmar via output do runner que nenhum teste foi removido ou ignorado.

## Critérios de aceite
- [ ] `dotnet build` do projeto de testes sem erros ou warnings de namespace.
- [ ] `dotnet test` com 100% de aprovação (zero falhas, zero skips adicionais).
- [ ] Nenhum arquivo de teste referencia os namespaces antigos `VideoProcessing.Finalizer.Lambda.Models` ou `VideoProcessing.Finalizer.Lambda.Services`.
