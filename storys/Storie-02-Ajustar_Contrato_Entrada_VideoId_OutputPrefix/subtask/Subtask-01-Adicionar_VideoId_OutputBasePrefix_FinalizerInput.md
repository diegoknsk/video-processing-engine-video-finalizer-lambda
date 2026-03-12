# Subtask-01: Adicionar videoId e outputBasePrefix ao FinalizerInput

## Descrição
Adicionar as propriedades `videoId` (string) e `outputBasePrefix` (string) ao modelo `FinalizerInput`, mantendo compatibilidade com os campos existentes (`framesBucket`, `framesBasePrefix`, `outputBucket`).

## Passos de Implementação
1. Abrir `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs`.
2. Adicionar a propriedade `VideoId` com `[JsonPropertyName("videoId")]`.
3. Adicionar a propriedade `OutputBasePrefix` com `[JsonPropertyName("outputBasePrefix")]`.
4. Manter as propriedades existentes (`FramesBucket`, `FramesBasePrefix`, `OutputBucket`) sem alteração.

## Formas de Teste
1. Desserializar um JSON com todos os 5 campos e verificar que `VideoId` e `OutputBasePrefix` são populados corretamente.
2. Desserializar um JSON sem `videoId` e `outputBasePrefix` e confirmar que as propriedades ficam `null` (não quebra retrocompatibilidade).
3. Serializar uma instância de `FinalizerInput` e confirmar que os novos campos aparecem com os nomes corretos no JSON.

## Critérios de Aceite
- [ ] `FinalizerInput` possui as propriedades `VideoId` (string?) e `OutputBasePrefix` (string?) com os atributos `[JsonPropertyName]` corretos.
- [ ] A desserialização do payload completo (`videoId`, `framesBucket`, `framesBasePrefix`, `outputBucket`, `outputBasePrefix`) funciona sem erros.
- [ ] Nenhuma propriedade existente foi removida ou renomeada.
