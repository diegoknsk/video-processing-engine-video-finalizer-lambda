# Storie-04: Integração com S3

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** —

## Descrição
Como desenvolvedor do sistema Video Processing Engine, quero integrar a Lambda Finalizer com Amazon S3, permitindo ler imagens processadas de um bucket por prefixo, gerar o ZIP em `/tmp`, e fazer upload do ZIP para um bucket de saída, para completar o fluxo de finalização do processamento de vídeos.

## Objetivo
Implementar serviço de integração com S3 para listar objetos por prefixo, baixar imagens para `/tmp`, gerar ZIP usando o `ZipService`, fazer upload do ZIP para bucket de saída, e logar conclusão do processamento.

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda, Amazon S3 SDK, System.IO.Compression
- Arquivos criados/modificados:
  - `src/VideoProcessing.Finalizer/Services/IS3Service.cs` (nova interface)
  - `src/VideoProcessing.Finalizer/Services/S3Service.cs` (nova implementação)
  - `src/VideoProcessing.Finalizer/Models/FinalizerEvent.cs` (atualizar: adicionar `S3SourceBucket`, `S3SourcePrefix`, `S3OutputBucket`)
  - `src/VideoProcessing.Finalizer/Function.cs` (atualizar fluxo para usar S3)
  - `test/VideoProcessing.Finalizer.Tests/Services/S3ServiceTests.cs` (novos testes)
  - `test/VideoProcessing.Finalizer.Tests/FunctionTests.cs` (atualizar testes com mock de S3)
  - `README.md` (atualizar com informações sobre buckets, prefixos, permissões IAM)
- Componentes: S3Service, handler atualizado, modelos atualizados
- Pacotes/Dependências:
  - AWSSDK.S3 (3.7.400.3 ou superior)

## Dependências e Riscos (para estimativa)
- Dependências: Storie-01, Storie-02 e Storie-03 concluídas
- Riscos:
  - Paginação S3: muitos objetos (>1000) requerem paginação (implementar usando `ListObjectsV2Paginator`)
  - Limites de /tmp: muitas imagens grandes podem exceder limite de 512 MB (logar warning)
  - Performance: download de muitas imagens pode ser lento (considerar download paralelo na evolução futura)
  - Permissões IAM: Lambda deve ter permissões `s3:GetObject`, `s3:ListBucket`, `s3:PutObject`
- Pré-condições:
  - Buckets de entrada e saída provisionados
  - Lambda com role IAM configurada com permissões S3

## Subtasks
- [Subtask 01: Criar interface e implementação do S3Service](./subtask/Subtask-01-Interface_S3Service.md)
- [Subtask 02: Atualizar modelos de evento para incluir informações de S3](./subtask/Subtask-02-Atualizar_Modelos_Evento.md)
- [Subtask 03: Atualizar handler para processar imagens do S3 e fazer upload do ZIP](./subtask/Subtask-03-Atualizar_Handler_S3.md)
- [Subtask 04: Criar testes unitários com mock de S3](./subtask/Subtask-04-Testes_Unitarios_S3.md)
- [Subtask 05: Validar deploy e execução end-to-end com S3](./subtask/Subtask-05-Validacao_Deploy_S3.md)
- [Subtask 06: Documentar permissões IAM e configuração de buckets](./subtask/Subtask-06-Documentar_IAM_Buckets.md)

## Critérios de Aceite da História
- [ ] S3Service implementado com métodos: listar objetos por prefixo (com paginação), baixar objeto, fazer upload
- [ ] Handler atualizado para: listar imagens no bucket de entrada por prefixo, baixar para /tmp, gerar ZIP, fazer upload do ZIP para bucket de saída
- [ ] Paginação S3 implementada usando `ListObjectsV2Paginator` para suportar >1000 objetos
- [ ] Logs estruturados informando: bucket de origem, prefixo, quantidade de imagens encontradas, quantidade baixada, tamanho do ZIP, bucket de destino, chave do ZIP
- [ ] Testes unitários cobrindo cenários: múltiplos objetos, paginação, bucket vazio, erro de permissão; cobertura ≥ 80%
- [ ] README.md documentando: permissões IAM necessárias, formato do evento de entrada com S3, exemplo de payload, estrutura de prefixos
- [ ] Deploy e validação no Lambda AWS com invocação usando bucket e prefixo reais, verificando que ZIP aparece no bucket de saída
- [ ] Limpeza de arquivos temporários em /tmp após upload bem-sucedido

## Rastreamento (dev tracking)
- **Início:** —
- **Fim:** —
- **Tempo total de desenvolvimento:** —
