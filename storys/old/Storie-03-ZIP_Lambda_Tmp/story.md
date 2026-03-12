# Storie-03: ZIP dentro do Lambda usando /tmp

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** —

## Descrição
Como desenvolvedor do sistema Video Processing Engine, quero adaptar a Lambda Finalizer para gerar ZIP no diretório `/tmp` do ambiente Lambda, para validar que a solução funciona dentro das restrições de armazenamento temporário da AWS.

## Objetivo
Atualizar o handler da Lambda para receber um evento com lista de arquivos (simulados ou base64), processar esses arquivos salvando-os em `/tmp`, gerar o ZIP em `/tmp`, e retornar informações sobre o arquivo criado (quantidade de arquivos, tamanho aproximado).

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda, System.IO.Compression
- Arquivos criados/modificados:
  - `src/VideoProcessing.Finalizer/Function.cs` (atualizar handler)
  - `src/VideoProcessing.Finalizer/Models/FinalizerEvent.cs` (novo modelo de entrada)
  - `src/VideoProcessing.Finalizer/Models/FinalizerResponse.cs` (novo modelo de saída)
  - `src/VideoProcessing.Finalizer/Services/ITempStorageService.cs` (nova interface)
  - `src/VideoProcessing.Finalizer/Services/TempStorageService.cs` (nova implementação)
  - `test/VideoProcessing.Finalizer.Tests/FunctionTests.cs` (atualizar testes)
  - `test/VideoProcessing.Finalizer.Tests/Services/TempStorageServiceTests.cs` (novos testes)
  - `README.md` (atualizar com informações sobre limites de /tmp)
- Componentes: Handler atualizado, TempStorageService, modelos de evento
- Pacotes/Dependências: (sem novos pacotes, usar bibliotecas do .NET 10)

## Dependências e Riscos (para estimativa)
- Dependências: Storie-01 e Storie-02 concluídas (estrutura e lógica de ZIP)
- Riscos:
  - `/tmp` no Lambda tem limite de 512 MB (ou até 10 GB se configurado); validar que não excederemos o limite
  - Necessidade de limpeza de arquivos temporários após processamento
  - Performance: operações de I/O podem ser lentas
- Pré-condições: Lambda provisionada com memória e /tmp suficiente

## Subtasks
- [Subtask 01: Criar modelos de evento e resposta para o handler](./subtask/Subtask-01-Modelos_Evento_Resposta.md)
- [Subtask 02: Criar TempStorageService para gerenciar arquivos em /tmp](./subtask/Subtask-02-TempStorageService.md)
- [Subtask 03: Atualizar handler para processar evento e gerar ZIP em /tmp](./subtask/Subtask-03-Atualizar_Handler_Lambda.md)
- [Subtask 04: Criar testes unitários e de integração para o novo fluxo](./subtask/Subtask-04-Testes_Unitarios_Integracao.md)
- [Subtask 05: Validar deploy e execução no Lambda com logs](./subtask/Subtask-05-Validacao_Deploy_Lambda.md)

## Critérios de Aceite da História
- [ ] Handler Lambda atualizado para receber `FinalizerEvent` com lista de arquivos (nomes e conteúdo base64 ou paths simulados)
- [ ] TempStorageService implementado gerenciando escrita e leitura de arquivos em `/tmp`
- [ ] ZIP gerado em `/tmp` com todos os arquivos recebidos no evento
- [ ] Limpeza de arquivos temporários implementada após processamento (try/finally)
- [ ] Logs estruturados informando: quantidade de arquivos recebidos, quantidade processada, tamanho do ZIP, caminho do ZIP em /tmp
- [ ] Testes unitários e de integração cobrindo cenários: evento válido, múltiplos arquivos, diretório /tmp simulado; cobertura ≥ 80%
- [ ] README.md atualizado documentando limites de /tmp, formato do evento de entrada, exemplo de payload
- [ ] Deploy e validação no Lambda AWS com invocação de teste retornando resposta esperada

## Rastreamento (dev tracking)
- **Início:** —
- **Fim:** —
- **Tempo total de desenvolvimento:** —
