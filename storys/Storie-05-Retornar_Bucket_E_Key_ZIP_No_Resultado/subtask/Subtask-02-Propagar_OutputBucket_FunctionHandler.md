# Subtask 02: Propagar OutputBucket na construção do resultado em FunctionHandler

## Descrição
Atualizar a linha de retorno do `FunctionHandler` em `Function.cs` para passar o `outputBucket` (já disponível como variável local) ao construir o `FinalizerResult`, garantindo que o bucket de destino do ZIP conste no resultado.

## Passos de Implementação
1. Localizar em `FunctionHandler` a linha `return new FinalizerResult(uploadedKey, keys.Count);`.
2. Substituir por `return new FinalizerResult(outputBucket, uploadedKey, keys.Count);`, usando a variável `outputBucket` já declarada no método.
3. Confirmar que a variável `outputBucket` está em escopo na linha de retorno (atualmente declarada logo após `ValidateInput`).

## Formas de Teste
1. **Build:** `dotnet build` passa sem erros após a alteração.
2. **Teste unitário:** ao mockar o serviço e invocar `FunctionHandler`, o resultado deve conter `ZipBucket` igual ao `outputBucket` do `FinalizerInput` fornecido.
3. **Inspeção manual:** enviar payload de teste via Lambda Test Tool e verificar no JSON retornado que `"ZipBucket"` tem o valor esperado.

## Critérios de Aceite
- [ ] A instrução `return` em `FunctionHandler` passa `outputBucket` como primeiro argumento de `FinalizerResult`
- [ ] O valor de `ZipBucket` no resultado é idêntico ao campo `outputBucket` do input processado
- [ ] Nenhuma outra lógica do método é alterada além da linha de retorno
