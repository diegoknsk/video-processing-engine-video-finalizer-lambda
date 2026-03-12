# Storie-01: Consolidar Frames de Múltiplos Chunks S3 em ZIP

## Status
- **Estado:** ✅ Concluída
- **Data de Conclusão:** 11/03/2026

## Descrição
Como consumidor de um Step Functions, quero receber o bucket de origem dos frames, o prefixo base e o bucket de destino, varrer todos os diretórios de chunk existentes nesse prefixo, baixar todas as imagens e compactá-las em um único arquivo ZIP, para que o resultado final esteja consolidado e disponível no bucket de saída configurado.

## Objetivo
Implementar o pipeline completo de consolidação de frames: receber o contrato de entrada (compatível com invocação direta e SQS), listar e baixar todas as imagens dos diretórios `chunk-XXX/frames/` presentes no prefixo S3 informado, compactar tudo em um único ZIP e fazer upload do ZIP no bucket de saída (`outputBucket`) informado no payload, mantendo os arquivos temporários limpos ao final.

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda, Amazon S3
- Arquivos afetados:
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Function.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Services/FramesZipService.cs`
- Componentes/Recursos: `FinalizerInput` (contrato), `FramesZipService` (pipeline S3 → /tmp → ZIP → S3), `Function` (handler Lambda)
- Pacotes/Dependências:
  - AWSSDK.S3 (3.7.300.2)
  - Amazon.Lambda.Core (2.2.0)
  - Amazon.Lambda.Serialization.SystemTextJson (2.4.0)
  - System.IO.Compression (incluído no runtime .NET 10)

## Dependências e Riscos (para estimativa)
- Dependências: Bucket S3 de origem com frames processados disponíveis; bucket S3 de destino para o ZIP; permissões IAM `s3:GetObject` no bucket de origem e `s3:PutObject` no bucket de destino (podem ser o mesmo ou buckets diferentes).
- Riscos/Pré-condições:
  - Prefixo S3 deve existir e conter ao menos um chunk com imagens; caso contrário retornar erro descritivo.
  - Espaço em `/tmp` deve ser suficiente para todos os frames + ZIP; configurar `EphemeralStorage` adequado na Lambda.
  - Step Functions envia o evento como invocação direta (payload JSON); contrato deve ser compatível também com body SQS para flexibilidade.

## Subtasks
- [x] [Subtask 01: Definir contrato de entrada FinalizerInput](./subtask/Subtask-01-Contrato_Entrada_FinalizerInput.md)
- [x] [Subtask 02: Listar e baixar frames de múltiplos chunks no S3](./subtask/Subtask-02-Listar_Baixar_Frames_Chunks_S3.md)
- [x] [Subtask 03: Compactar frames em ZIP único](./subtask/Subtask-03-Compactar_Frames_ZIP.md)
- [x] [Subtask 04: Upload do ZIP ao S3 e limpeza de temporários](./subtask/Subtask-04-Upload_ZIP_S3_Limpeza.md)
- [x] [Subtask 05: Testes unitários do pipeline](./subtask/Subtask-05-Testes_Unitarios.md)

## Critérios de Aceite da História
- [x] A Lambda recebe o payload `{ "framesBucket": "...", "framesBasePrefix": "...", "outputBucket": "..." }` e processa sem erros
- [x] Todos os diretórios `chunk-XXX/frames/` (independente do número de chunks) são descobertos via listagem S3 por prefixo
- [x] Todas as imagens encontradas nos chunks são baixadas para `/tmp` e incluídas no ZIP gerado
- [x] O ZIP é enviado ao bucket `outputBucket` informado no payload, na chave `consolidated/<framesBasePrefix>/<jobId>_frames.zip`
- [x] Arquivos temporários em `/tmp` (frames baixados, ZIP) são removidos ao final da execução, mesmo em caso de erro
- [x] Se nenhum frame for encontrado, a Lambda lança exceção descritiva com log adequado
- [x] Testes unitários passando; cobertura ≥ 80% dos cenários do `FramesZipService`

## Rastreamento (dev tracking)
- **Início:** 11/03/2026, às 20:54 (Brasília)
- **Fim:** 11/03/2026, às 21:01 (Brasília)
- **Tempo total de desenvolvimento:** 7min
