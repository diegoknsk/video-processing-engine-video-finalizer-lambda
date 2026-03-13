# Storie-07: Separar Camadas Clean Architecture

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Como arquiteto do sistema, quero separar o código atualmente concentrado no projeto Lambda em camadas independentes (Domain, Application, Infra), para que a estrutura reflita Clean Architecture de forma análoga ao projeto VideoManagement de referência.

## Objetivo
Criar os projetos `VideoProcessing.Finalizer.Domain`, `VideoProcessing.Finalizer.Application` e `VideoProcessing.Finalizer.Infra` nas pastas `src/Core/` e `src/Infra/`, mover os artefatos para as camadas corretas, enxugar o Lambda como puro ponto de entrada, garantir que todos os testes continuem passando e atualizar README e arquivo de solution.

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda
- Arquivos criados:
  - `src/Core/VideoProcessing.Finalizer.Domain/VideoProcessing.Finalizer.Domain.csproj`
  - `src/Core/VideoProcessing.Finalizer.Domain/Models/FinalizerInput.cs`
  - `src/Core/VideoProcessing.Finalizer.Domain/Models/FinalizerResult.cs`
  - `src/Core/VideoProcessing.Finalizer.Application/VideoProcessing.Finalizer.Application.csproj`
  - `src/Core/VideoProcessing.Finalizer.Application/Ports/IFramesZipService.cs`
  - `src/Infra/VideoProcessing.Finalizer.Infra/VideoProcessing.Finalizer.Infra.csproj`
  - `src/Infra/VideoProcessing.Finalizer.Infra/Services/FramesZipService.cs`
  - `src/Infra/VideoProcessing.Finalizer.Infra/Helpers/FrameRenamer.cs`
  - `VideoProcessing.Finalizer.slnx`
- Arquivos atualizados:
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Function.cs`
  - `src/tests/VideoProcessing.Finalizer.Tests/VideoProcessing.Finalizer.Tests.csproj`
  - `src/tests/VideoProcessing.Finalizer.Tests/FinalizerInputTests.cs`
  - `src/tests/VideoProcessing.Finalizer.Tests/FrameRenamerTests.cs`
  - `src/tests/VideoProcessing.Finalizer.Tests/FramesZipServiceTests.cs`
  - `src/tests/VideoProcessing.Finalizer.Tests/FunctionTests.cs`
  - `README.md`
- Arquivos removidos do projeto Lambda:
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/IFramesZipService.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FramesZipService.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FrameRenamer.cs`
- Pacotes/Dependências:
  - Nenhum pacote externo novo; redistribuição de referências existentes entre projetos

## Dependências e Riscos (para estimativa)
- Dependências: Stories 01–06 devem estar funcionais (pipeline, contrato, renomeação, SonarCloud).
- Riscos/Pré-condições:
  - `FrameRenamer` é `internal`; o Infra.csproj precisará de `InternalsVisibleTo` para os testes.
  - Namespace de cada tipo muda; todos os `using` nos testes precisam ser atualizados.
  - `FinalizerResult` estava embutido em `Function.cs`; precisa ser extraído para Domain antes de remover o record do Lambda.
  - Deploy pipeline não muda: o handler e o projeto Lambda continuam os mesmos.

## Subtasks
- [ ] [Subtask 01: Criar projetos Domain e Application em src/Core/](./subtask/Subtask-01-Criar_Domain_Application.md)
- [ ] [Subtask 02: Criar projeto Infra em src/Infra/](./subtask/Subtask-02-Criar_Infra.md)
- [ ] [Subtask 03: Atualizar Lambda (remover arquivos movidos, novos ProjectReferences)](./subtask/Subtask-03-Atualizar_Lambda.md)
- [ ] [Subtask 04: Atualizar projeto de testes (referências e using statements)](./subtask/Subtask-04-Atualizar_Tests.md)
- [ ] [Subtask 05: Criar solution .slnx e atualizar README](./subtask/Subtask-05-Solution_E_README.md)

## Critérios de Aceite da História
- [ ] Existem os projetos `VideoProcessing.Finalizer.Domain`, `VideoProcessing.Finalizer.Application` e `VideoProcessing.Finalizer.Infra` nas pastas `src/Core/` e `src/Infra/`
- [ ] `FinalizerInput` e `FinalizerResult` residem no namespace `VideoProcessing.Finalizer.Domain.Models`
- [ ] `IFramesZipService` reside no namespace `VideoProcessing.Finalizer.Application.Ports`
- [ ] `FramesZipService` e `FrameRenamer` residem no namespace `VideoProcessing.Finalizer.Infra.*` e não existem mais no Lambda
- [ ] O projeto Lambda contém apenas `Function.cs` como arquivo de código; as pastas `Models/` e `Services/` foram removidas
- [ ] `dotnet build` compila sem erros ou warnings em todos os projetos
- [ ] `dotnet test` passa 100% dos testes existentes sem alteração de comportamento
- [ ] Existe o arquivo `VideoProcessing.Finalizer.slnx` na raiz do repositório com os 5 projetos organizados em pastas virtuais
- [ ] README atualizado reflete a nova estrutura de camadas, a arquitetura e o guia de variáveis/configurações

## Rastreamento (dev tracking)
- **Início:** dia 13/03/2026, às 17:37 (Brasília)
- **Fim:** —
- **Tempo total de desenvolvimento:** —
