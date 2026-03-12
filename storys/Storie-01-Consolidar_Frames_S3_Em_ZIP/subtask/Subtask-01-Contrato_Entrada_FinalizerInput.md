# Subtask 01: Definir contrato de entrada FinalizerInput

## Descrição
Criar o modelo `FinalizerInput` que representa o payload recebido pela Lambda, contendo `FramesBucket`, `FramesBasePrefix` e `OutputBucket`. O contrato deve ser serializável via `System.Text.Json` e compatível tanto com invocação direta do Step Functions quanto com mensagem SQS (leitura do campo `Body`).

## Passos de Implementação
1. Criar `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Models/FinalizerInput.cs` com as propriedades `FramesBucket` (string), `FramesBasePrefix` (string) e `OutputBucket` (string), usando `JsonPropertyName` para mapear os nomes `framesBucket`, `framesBasePrefix` e `outputBucket` do payload JSON.
2. No `Function.cs`, configurar o deserializador para aceitar o payload tanto como objeto direto (`FinalizerInput`) quanto como `SQSEvent` (extraindo `Body` da primeira mensagem), decidindo pelo conteúdo recebido.
3. Adicionar validação básica no handler: se `FramesBucket`, `FramesBasePrefix` ou `OutputBucket` for nulo/vazio, lançar `ArgumentException` com mensagem descritiva antes de continuar o pipeline.

## Formas de Teste
1. **Unitário:** testar deserialização do JSON `{ "framesBucket": "bucket-x", "framesBasePrefix": "processed/abc/def", "outputBucket": "bucket-output" }` e verificar que as três propriedades são preenchidas corretamente.
2. **Unitário:** testar que `ArgumentException` é lançada para cada campo obrigatório nulo ou vazio (`FramesBucket`, `FramesBasePrefix`, `OutputBucket`).
3. **Manual:** invocar a Lambda localmente via `dotnet lambda invoke-function` passando o payload JSON e verificar nos logs que o modelo foi carregado com os valores corretos.

## Critérios de Aceite
- [x] `FinalizerInput` tem propriedades `FramesBucket`, `FramesBasePrefix` e `OutputBucket` com atributos `JsonPropertyName` corretos
- [x] Deserialização a partir do payload JSON do Step Functions funciona sem erros
- [x] Validação lança `ArgumentException` descritiva para cada campo obrigatório vazio (`FramesBucket`, `FramesBasePrefix`, `OutputBucket`)
