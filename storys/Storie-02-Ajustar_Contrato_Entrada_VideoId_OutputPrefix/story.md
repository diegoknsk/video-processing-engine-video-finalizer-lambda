# Storie-02: Ajustar Contrato de Entrada com videoId e outputBasePrefix

## Status
- **Estado:** ✅ Concluída
- **Data de Conclusão:** 11/03/2026

## Descrição
Como consumidor de um Step Functions, quero informar o `videoId` e o `outputBasePrefix` no payload de entrada da Lambda, para que o ZIP gerado seja salvo sob o prefixo correto (`guidUsuario/guidVideoId`) no bucket de saída, garantindo organização e rastreabilidade por usuário e vídeo.

## Objetivo
Ajustar o contrato de entrada `FinalizerInput` para incluir os campos `videoId` (string) e `outputBasePrefix` (string no formato `guidUsuario/guidVideoId`), atualizar a lógica de montagem da chave S3 de destino para usar `outputBasePrefix` em vez de derivar o prefixo de `framesBasePrefix`, e garantir que todos os testes unitários reflitam o novo contrato.

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda, Amazon S3
- Arquivos afetados:
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FramesZipService.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Function.cs`
  - `tests/` (arquivos de teste unitário do pipeline)
- Componentes/Recursos: `FinalizerInput` (contrato), `FramesZipService.BuildZipS3Key` (lógica de chave S3), `Function` (handler Lambda)
- Pacotes/Dependências:
  - AWSSDK.S3 (3.7.300.2)
  - Amazon.Lambda.Core (2.2.0)
  - Amazon.Lambda.Serialization.SystemTextJson (2.4.0)
  - System.IO.Compression (incluído no runtime .NET 10)

## Dependências e Riscos (para estimativa)
- Dependências: Story 01 concluída (pipeline base já implementado); o `outputBasePrefix` deve ser provido pelo Step Functions no formato `guidUsuario/guidVideoId`.
- Riscos/Pré-condições:
  - O `outputBasePrefix` é obrigatório e deve ser provido pelo orquestrador; se ausente, a Lambda deve retornar erro descritivo.
  - O `videoId` é obrigatório para rastreabilidade; tratar como string (pode ser um GUID).
  - A chave S3 de saída muda de `consolidated/<framesBasePrefix>/<jobId>_frames.zip` para `<outputBasePrefix>/<videoId>_frames.zip`; ajustar quaisquer consumidores que dependem do padrão antigo.

## Subtasks
- [x] [Subtask 01: Adicionar videoId e outputBasePrefix ao FinalizerInput](./subtask/Subtask-01-Adicionar_VideoId_OutputBasePrefix_FinalizerInput.md)
- [x] [Subtask 02: Atualizar BuildZipS3Key para usar outputBasePrefix](./subtask/Subtask-02-Atualizar_BuildZipS3Key_OutputBasePrefix.md)
- [x] [Subtask 03: Atualizar Function.cs para repassar os novos campos](./subtask/Subtask-03-Atualizar_Function_Novos_Campos.md)
- [x] [Subtask 04: Atualizar testes unitários para o novo contrato](./subtask/Subtask-04-Atualizar_Testes_Unitarios.md)

## Critérios de Aceite da História
- [x] O payload de entrada aceita `{ "videoId": "...", "framesBucket": "...", "framesBasePrefix": "...", "outputBucket": "...", "outputBasePrefix": "guidUsuario/guidVideoId" }` sem erros de desserialização
- [x] O campo `videoId` é obrigatório; se ausente ou vazio, a Lambda lança exceção descritiva antes de iniciar o pipeline
- [x] O campo `outputBasePrefix` é obrigatório; se ausente ou vazio, a Lambda lança exceção descritiva antes de iniciar o pipeline
- [x] O ZIP é enviado ao bucket `outputBucket` sob a chave `<outputBasePrefix>/<videoId>_frames.zip` (ex.: `abc123/def456_frames.zip`)
- [x] A lógica antiga de derivar a chave de `framesBasePrefix` (`consolidated/...`) é removida ou substituída
- [x] Testes unitários do `FramesZipService` (incluindo `BuildZipS3Key`) passam com os novos parâmetros; cobertura ≥ 80%
- [x] Testes unitários do handler `Function` validam os novos campos obrigatórios e a chave S3 resultante

## Rastreamento (dev tracking)
- **Início:** 11/03/2026, às 21:10 (Brasília)
- **Fim:** 11/03/2026, às 21:26 (Brasília)
- **Tempo total de desenvolvimento:** ~16 min
