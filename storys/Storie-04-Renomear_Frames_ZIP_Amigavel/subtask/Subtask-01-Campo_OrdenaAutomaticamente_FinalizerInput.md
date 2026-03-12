# Subtask 01: Adicionar campo OrdenaAutomaticamente ao FinalizerInput

## Descrição
Adicionar a propriedade `OrdenaAutomaticamente` (bool, default `true`) ao modelo `FinalizerInput`, mapeada para o campo JSON `ordenaAutomaticamente`. Quando omitida no payload, o valor padrão `true` deve ser aplicado automaticamente.

## Passos de Implementação
1. Abrir `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`.
2. Adicionar a propriedade:
   ```csharp
   [JsonPropertyName("ordenaAutomaticamente")]
   public bool OrdenaAutomaticamente { get; set; } = true;
   ```
3. Verificar que a validação em `Function.ValidateInput` não precisa de alteração (o campo é opcional com default).

## Formas de Teste
1. Deserializar JSON sem `ordenaAutomaticamente` e verificar que o valor é `true`.
2. Deserializar JSON com `"ordenaAutomaticamente": false` e verificar que o valor é `false`.
3. Executar `dotnet test` e confirmar que os testes existentes continuam passando.

## Critérios de Aceite
- [ ] Campo presente em `FinalizerInput` com `[JsonPropertyName("ordenaAutomaticamente")]` e default `true`
- [ ] Desserialização de payload sem o campo retorna `OrdenaAutomaticamente = true`
- [ ] Desserialização com `"ordenaAutomaticamente": false` retorna `false`
