# Subtask 01: Adicionar campo ZipBucket ao FinalizerResult

## Descrição
Incluir o campo `ZipBucket` no record `FinalizerResult`, que é definido no final de `Function.cs`, tornando a localização completa do ZIP (bucket + chave) parte do contrato de saída da Lambda.

## Passos de Implementação
1. Localizar o record `FinalizerResult` em `Function.cs` (atualmente: `public record FinalizerResult(string ZipS3Key, int FramesCount);`).
2. Adicionar o parâmetro `string ZipBucket` ao record, resultando em `public record FinalizerResult(string ZipBucket, string ZipS3Key, int FramesCount);`.
3. Verificar que o record não é instanciado em outros locais além do `FunctionHandler` (busca global por `new FinalizerResult`).

## Formas de Teste
1. **Build:** `dotnet build` deve compilar sem erros após a alteração.
2. **Teste unitário:** a subtask 03 cobrirá a validação do campo; garantir que o código compila é o critério desta subtask.
3. **Inspeção manual:** verificar no JSON de resposta simulado (Lambda Test Tool) que o campo `ZipBucket` aparece na saída.

## Critérios de Aceite
- [ ] Record `FinalizerResult` possui exatamente três parâmetros: `ZipBucket`, `ZipS3Key` e `FramesCount`
- [ ] `dotnet build` passa sem erros ou warnings novos
- [ ] Nenhum outro local no projeto instancia `FinalizerResult` com assinatura antiga sem ser atualizado
