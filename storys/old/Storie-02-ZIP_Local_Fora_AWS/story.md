# Storie-02: ZIP Local (fora da AWS)

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** —

## Descrição
Como desenvolvedor do sistema Video Processing Engine, quero implementar a funcionalidade de criação de arquivo ZIP a partir de um diretório local de imagens, para validar a lógica de compactação antes de integrar com S3.

## Objetivo
Implementar lógica de leitura de arquivos de imagem de um diretório local, geração de arquivo ZIP contendo todas as imagens, e criar um harness simples para execução e validação local (fora da AWS).

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, System.IO.Compression.ZipFile
- Arquivos criados/modificados:
  - `src/VideoProcessing.Finalizer/Services/ZipService.cs` (nova classe)
  - `src/VideoProcessing.Finalizer/Services/IZipService.cs` (nova interface)
  - `src/VideoProcessing.Finalizer/Models/ZipCreationRequest.cs` (input model)
  - `src/VideoProcessing.Finalizer/Models/ZipCreationResult.cs` (output model)
  - `test/VideoProcessing.Finalizer.Tests/Services/ZipServiceTests.cs`
  - `test/VideoProcessing.Finalizer.Tests/Harness/LocalZipHarness.cs` (harness de teste local)
  - `README.md` (atualizar com instruções de teste local)
- Componentes: ZipService, modelos de request/result, harness de teste
- Pacotes/Dependências:
  - System.IO.Compression (incluso no .NET 10)
  - System.IO.Compression.ZipFile (incluso no .NET 10)

## Dependências e Riscos (para estimativa)
- Dependências: Storie-01 concluída (estrutura do projeto)
- Riscos:
  - Performance pode ser limitada com muitos arquivos (mitigado com streaming na Storie-03)
  - Validação de formatos de imagem não será feita nesta story (escopo futuro)
- Pré-condições: Diretório local com imagens de teste disponível

## Subtasks
- [Subtask 01: Criar modelos de request/result e interface IZipService](./subtask/Subtask-01-Modelos_Interface_ZipService.md)
- [Subtask 02: Implementar ZipService com lógica de criação de ZIP](./subtask/Subtask-02-Implementar_ZipService.md)
- [Subtask 03: Criar harness local para teste manual](./subtask/Subtask-03-Harness_Local_Teste.md)
- [Subtask 04: Criar testes unitários do ZipService](./subtask/Subtask-04-Testes_Unitarios_ZipService.md)

## Critérios de Aceite da História
- [ ] Interface `IZipService` e implementação `ZipService` criadas seguindo princípios Clean Architecture
- [ ] ZipService lê arquivos de diretório local e gera arquivo ZIP funcional
- [ ] Modelos `ZipCreationRequest` e `ZipCreationResult` implementados como records imutáveis
- [ ] Harness local (`LocalZipHarness`) permite executar a criação de ZIP com diretório de teste
- [ ] ZIP gerado pode ser aberto e validado manualmente (inspeção visual)
- [ ] Testes unitários cobrindo cenários: diretório vazio, múltiplos arquivos, arquivos grandes; cobertura ≥ 80%
- [ ] README.md atualizado com instruções de como executar harness local e validar ZIP
- [ ] Logs estruturados informando: quantidade de arquivos processados, tamanho do ZIP gerado

## Rastreamento (dev tracking)
- **Início:** —
- **Fim:** —
- **Tempo total de desenvolvimento:** —
