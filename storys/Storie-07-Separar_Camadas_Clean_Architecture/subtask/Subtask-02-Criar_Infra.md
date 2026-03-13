# Subtask-02: Criar projeto Infra em src/Infra/

## Descrição
Criar o projeto `VideoProcessing.Finalizer.Infra` em `src/Infra/`, mover `FramesZipService` e `FrameRenamer` para ele com os namespaces corretos e configurar `InternalsVisibleTo` para que os testes continuem acessando `FrameRenamer` (que permanece `internal`).

## Passos de implementação
1. Criar `src/Infra/VideoProcessing.Finalizer.Infra/VideoProcessing.Finalizer.Infra.csproj` (Class Library, net10.0) referenciando Application e o pacote AWSSDK.S3.
2. Adicionar `<InternalsVisibleTo Include="VideoProcessing.Finalizer.Tests" />` no csproj.
3. Criar `src/Infra/VideoProcessing.Finalizer.Infra/Services/FramesZipService.cs` com namespace `VideoProcessing.Finalizer.Infra.Services` — alterar o `using` para importar `IFramesZipService` do namespace Application.Ports.
4. Criar `src/Infra/VideoProcessing.Finalizer.Infra/Helpers/FrameRenamer.cs` com namespace `VideoProcessing.Finalizer.Infra.Helpers` (classe permanece `internal static`).

## Formas de teste
1. `dotnet build src/Infra/VideoProcessing.Finalizer.Infra/VideoProcessing.Finalizer.Infra.csproj` compila sem erros.
2. Confirmar que `FramesZipService` implementa `IFramesZipService` sem erros de compilação.
3. Confirmar que `FrameRenamer` permanece `internal` e que o InternalsVisibleTo está declarado.

## Critérios de aceite
- [ ] Projeto compila sem erros em `src/Infra/`.
- [ ] `FramesZipService` está em namespace `VideoProcessing.Finalizer.Infra.Services` e implementa `IFramesZipService`.
- [ ] `FrameRenamer` está em namespace `VideoProcessing.Finalizer.Infra.Helpers`, é `internal` e tem `InternalsVisibleTo` configurado para o projeto de testes.
