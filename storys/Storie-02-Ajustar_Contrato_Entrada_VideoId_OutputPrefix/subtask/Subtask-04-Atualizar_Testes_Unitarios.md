# Subtask-04: Atualizar Testes Unitários para o Novo Contrato

## Descrição
Atualizar todos os testes unitários existentes que referenciam `BuildZipS3Key`, `GetJobIdFromPrefix` ou o `FinalizerInput`, além de adicionar novos cenários para cobrir as validações dos campos `videoId` e `outputBasePrefix` no handler e no serviço.

## Passos de Implementação
1. Localizar os arquivos de teste que testam `FramesZipService` e `Function` (pasta `tests/`).
2. Atualizar os testes de `BuildZipS3Key` para usar a nova assinatura `(outputBasePrefix, videoId)` e validar o novo padrão de chave.
3. Adicionar testes de `FinalizerInput` verificando desserialização dos campos `videoId` e `outputBasePrefix`.
4. Adicionar testes no handler para os cenários de payload sem `videoId` (deve lançar `ArgumentException`) e sem `outputBasePrefix` (deve lançar `ArgumentException`).
5. Executar `dotnet test` e confirmar que todos os testes passam; verificar cobertura ≥ 80%.

## Formas de Teste
1. `dotnet test` na solução — todos os testes devem passar sem erros.
2. Verificar cobertura de linha com relatório Cobertura/Coverlet — mínimo 80% nos arquivos alterados.
3. Testar manualmente os cenários de erro (payload incompleto) via ferramenta de teste Lambda local.

## Critérios de Aceite
- [ ] Todos os testes existentes que chamavam `BuildZipS3Key` com a assinatura antiga foram atualizados e passam.
- [ ] Novos testes cobrem: desserialização completa do `FinalizerInput` com os 5 campos, erro por `videoId` ausente, erro por `outputBasePrefix` ausente, e chave S3 correta gerada por `BuildZipS3Key`.
- [ ] `dotnet test` retorna 0 falhas e cobertura ≥ 80% dos arquivos modificados.
