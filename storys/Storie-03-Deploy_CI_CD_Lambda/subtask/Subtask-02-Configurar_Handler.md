# Subtask-02: Configurar handler no step de deploy

## Descrição
Após o upload do código, adicionar o step `aws lambda update-function-configuration` para garantir que o handler da função Lambda aponte para a classe e método corretos do projeto .NET — evitando que a função continue com um handler inválido ou de casca anterior.

## Passos de Implementação

1. **Identificar o handler correto:**
   - Namespace: `VideoProcessing.Finalizer.Lambda`
   - Classe: `Function`
   - Método: `FunctionHandler`
   - Formato AWS: `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`

2. **Adicionar step após o wait de update-function-code:**
   ```yaml
   - name: Set Lambda handler
     run: |
       aws lambda update-function-configuration \
         --function-name "${{ env.LAMBDA_FUNCTION_NAME }}" \
         --handler "VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler"
   ```

3. **Adicionar wait após update-function-configuration:**
   ```yaml
   - name: Wait for configuration update
     run: |
       aws lambda wait function-updated \
         --function-name "${{ env.LAMBDA_FUNCTION_NAME }}"
       echo "Lambda function updated successfully"
   ```

## Formas de Teste

1. No AWS Console (Lambda → Configuração → Informações gerais), verificar que o campo "Handler" exibe `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler` após o pipeline concluir.
2. Executar um teste via "Test" no console Lambda com payload vazio e verificar que a execução não retorna erro de "Handler not found".
3. Nos logs do Actions, confirmar que o step "Set Lambda handler" e "Wait for configuration update" completam sem erros.

## Critérios de Aceite

- [ ] O handler configurado no AWS Lambda após o deploy é exatamente `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`.
- [ ] O step de wait de configuração é executado após o set handler, garantindo consistência antes do pipeline finalizar.
- [ ] O pipeline completa sem erros nos steps de configuração do handler.
