# Storie-04: Renomear e Reorganizar Frames no ZIP para Experiência Amigável ao Usuário

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Como consumidor final do ZIP gerado pela Lambda, quero que os frames dentro do arquivo estejam renomeados de forma sequencial e ordenados pelo instante do vídeo, para que a estrutura de chunks não contamine a experiência e os arquivos possam ser interpretados facilmente em qualquer visualizador.

## Objetivo
Implementar renomeação automática e opcionalmente ativável dos frames no ZIP: quando o parâmetro `ordenaAutomaticamente` (default `true`) estiver ativo, extrair o instante de tempo do sufixo do arquivo (ex.: `frame_0001_20s.jpg` → 20 s), ordenar todos os frames por instante crescente, renumerá-los sequencialmente (frame_0001, frame_0002, …) preservando o sufixo de tempo original, e incluí-los diretamente na raiz do ZIP sem pastas de chunk. Quando o instante não puder ser extraído do sufixo, manter o comportamento anterior. Se o total ultrapassar 9999 imagens, a largura do número é ajustada automaticamente (5+ dígitos).

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda, System.IO.Compression
- Arquivos afetados:
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FramesZipService.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/IFramesZipService.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Function.cs`
  - `src/tests/VideoProcessing.Finalizer.Tests/FramesZipServiceTests.cs`
- Componentes/Recursos:
  - `FinalizerInput` — novo campo `OrdenaAutomaticamente` (bool, default `true`)
  - `FramesZipService.CreateZip` — sobrecarga/ajuste para aceitar flag e aplicar renomeação
  - `FrameRenamer` — classe estática ou helper interno com a lógica de parse de tempo e renomeação
- Pacotes/Dependências:
  - System.IO.Compression (incluído no runtime .NET 10 — sem versão externa)
  - AWSSDK.S3 (3.7.300.2 — sem alteração)

## Dependências e Riscos (para estimativa)
- Dependências: Story 01 (pipeline S3 → ZIP) e Story 02 (contrato `FinalizerInput`) devem estar concluídas.
- Riscos/Pré-condições:
  - Nomes dos arquivos de frame devem seguir o padrão `*_<N>s.<ext>` para que a extração de tempo funcione; caso contrário, o comportamento antigo é mantido graciosamente.
  - Se dois frames tiverem o mesmo instante de tempo (edge case), a ordenação secundária deve ser estável (por nome original).
  - O total de frames deve respeitar o espaço em `/tmp`; sem alteração nesse limite.

## Subtasks
- [ ] [Subtask 01: Adicionar campo OrdenaAutomaticamente ao FinalizerInput](./subtask/Subtask-01-Campo_OrdenaAutomaticamente_FinalizerInput.md)
- [ ] [Subtask 02: Implementar helper FrameRenamer (parse de tempo e geração de nomes sequenciais)](./subtask/Subtask-02-FrameRenamer_Helper.md)
- [ ] [Subtask 03: Adaptar CreateZip para renomear e colocar frames na raiz do ZIP](./subtask/Subtask-03-Adaptar_CreateZip_Renomeacao.md)
- [ ] [Subtask 04: Propagar flag na Function e ajustar IFramesZipService](./subtask/Subtask-04-Propagar_Flag_Function_Interface.md)
- [ ] [Subtask 05: Testes unitários — FrameRenamer e CreateZip com renomeação](./subtask/Subtask-05-Testes_Unitarios_Renomeacao.md)

## Critérios de Aceite da História
- [ ] O campo `ordenaAutomaticamente` em `FinalizerInput` tem valor padrão `true` e é deserializado corretamente; quando omitido no payload, aplica a renomeação
- [ ] Com `ordenaAutomaticamente = true`, o ZIP gerado contém os arquivos diretamente na raiz (sem pastas `chunk-XXX/frames/`)
- [ ] Os arquivos no ZIP estão renomeados sequencialmente por instante de tempo crescente: `frame_0001_0s.jpg`, `frame_0002_5s.jpg`, `frame_0003_10s.jpg`, etc.
- [ ] Quando o sufixo de tempo não for identificável (`*_<N>s`), o arquivo é incluído no ZIP com seu nome original sem causar erro
- [ ] Quando o total de frames ultrapassar 9999, o padding do número é ajustado automaticamente (ex.: `frame_10000_300s.jpg`)
- [ ] Com `ordenaAutomaticamente = false`, o comportamento anterior é mantido integralmente (estrutura de diretórios e nomes originais preservados)
- [ ] Testes unitários para `FrameRenamer` cobrindo: parse de tempo válido, sufixo ausente, ordenação com empate, padding dinâmico; cobertura ≥ 80% das novas classes/métodos

## Rastreamento (dev tracking)
- **Início:** 12/03/2026, às 00:49 (Brasília)
- **Fim:** —
- **Tempo total de desenvolvimento:** —
