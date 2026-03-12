# Subtask 04: Propagar flag na Function e ajustar IFramesZipService

## Descrição
Atualizar `Function.FunctionHandler` para ler o campo `OrdenaAutomaticamente` do `FinalizerInput` e passá-lo na chamada a `_framesZipService.CreateZip`. Garantir que `IFramesZipService` tenha a assinatura atualizada e que o log registre o valor do parâmetro.

## Passos de Implementação
1. Abrir `Function.cs` e localizar a chamada `_framesZipService.CreateZip(framesDir, zipPath)`.
2. Atualizar para `_framesZipService.CreateZip(framesDir, zipPath, inputModel.OrdenaAutomaticamente)`.
3. Adicionar log informando o valor do parâmetro:
   ```csharp
   context.Logger.LogInformation($"OrdenaAutomaticamente={inputModel.OrdenaAutomaticamente}");
   ```
4. Verificar que `IFramesZipService.CreateZip` já está com a assinatura atualizada (feito na Subtask 03).

## Formas de Teste
1. Teste de integração local: payload com `"ordenaAutomaticamente": true` → ZIP na raiz com frames renomeados.
2. Teste de integração local: payload com `"ordenaAutomaticamente": false` → ZIP com estrutura de diretórios original.
3. Payload sem `ordenaAutomaticamente` → deve usar default `true` (verificar via log e ZIP gerado).

## Critérios de Aceite
- [ ] `Function.FunctionHandler` lê `inputModel.OrdenaAutomaticamente` e passa para `CreateZip`
- [ ] Log registra o valor de `OrdenaAutomaticamente` antes de criar o ZIP
- [ ] Nenhuma outra alteração desnecessária em `Function.cs` além do passo descrito
- [ ] `dotnet build` sem erros ou warnings novos
