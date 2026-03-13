# Subtask 03: Atualizar testes unitários para o novo campo ZipBucket

## Descrição
Ajustar todos os testes em `FunctionTests.cs` que instanciam ou verificam `FinalizerResult`, e adicionar asserção explícita de que `ZipBucket` corresponde ao `OutputBucket` do input em cenários de sucesso.

## Passos de Implementação
1. Abrir `FunctionTests.cs` e localizar todos os usos de `FinalizerResult` (construção, asserção, pattern matching).
2. Atualizar cada construção `new FinalizerResult(key, count)` para `new FinalizerResult(bucket, key, count)` com valores compatíveis com o teste.
3. Nos testes de caminho feliz, adicionar asserção `Assert.Equal(expectedBucket, result.ZipBucket)` para garantir que o bucket correto é propagado.

## Formas de Teste
1. **Execução:** `dotnet test` deve passar sem falhas após as alterações.
2. **Cobertura:** verificar que as linhas modificadas em `Function.cs` estão cobertas pelos testes (`dotnet test --collect:"XPlat Code Coverage"`).
3. **Revisão:** inspecionar manualmente que nenhum teste ficou com asserção faltando para `ZipBucket`.

## Critérios de Aceite
- [ ] `dotnet test` executa sem erros nem falhas
- [ ] Existe ao menos um teste que verifica `result.ZipBucket == inputModel.OutputBucket` após execução bem-sucedida
- [ ] Nenhum teste usa a assinatura antiga de `FinalizerResult` com dois parâmetros
- [ ] Cobertura das classes modificadas (`Function.cs`) ≥ 80%
