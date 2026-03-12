# Subtask 05: Testes unitários do pipeline

## Descrição
Criar projeto de testes xUnit cobrindo os cenários principais do `FramesZipService` e do contrato de entrada `FinalizerInput`: listagem paginada, download de frames, criação do ZIP e upload ao S3. Usar mocks para `IAmazonS3` (NSubstitute ou Moq) e sistema de arquivos real para testes de ZIP.

## Passos de Implementação
1. Criar projeto `tests/VideoProcessing.Finalizer.Lambda.Tests` (xUnit) referenciando o projeto Lambda; adicionar pacotes `xunit`, `xunit.runner.visualstudio`, `NSubstitute` (ou `Moq`) e `coverlet.collector`.
2. Implementar classe `FramesZipServiceTests` com testes para: (a) listagem paginada retorna todas as chaves; (b) filtro de extensão exclui arquivos não-imagem; (c) exceção quando nenhum frame encontrado; (d) `CreateZipAsync` gera ZIP com arquivos corretos; (e) upload chama S3 com parâmetros corretos; (f) falha no upload relança exceção após limpeza.
3. Implementar classe `FinalizerInputTests` com testes para: (a) deserialização correta do JSON; (b) validação lança exceção para campos obrigatórios vazios.

## Formas de Teste
1. **`dotnet test`** — todos os testes passando com saída verde.
2. **Cobertura:** executar `dotnet test --collect:"XPlat Code Coverage"` e verificar relatório `coverage.cobertura.xml`; cobertura ≥ 80% em `FramesZipService` e `FinalizerInput`.
3. **Revisão:** garantir que cada caminho de erro (nenhum frame, upload falho, limpeza) tem ao menos um teste dedicado.

## Critérios de Aceite
- [x] Projeto de testes criado com xUnit e NSubstitute (ou Moq); `dotnet test` executa sem erros de build
- [x] Mínimo 6 testes unitários implementados cobrindo os cenários descritos
- [x] Cobertura ≥ 80% nas classes `FramesZipService` e `FinalizerInput`
- [x] Cenários de erro (nenhum frame, falha no upload) têm testes dedicados com verificação de exceção e side effects
