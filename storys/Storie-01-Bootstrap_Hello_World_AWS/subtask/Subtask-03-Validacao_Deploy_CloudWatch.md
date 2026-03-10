# Subtask 03: Validar deploy e execução com logs no CloudWatch

## Descrição
Após o deploy via GitHub Actions, validar que a Lambda está funcional invocando-a via AWS Console ou CLI, e verificar que os logs aparecem corretamente no CloudWatch Logs.

## Passos de Implementação
1. Preparar payload de teste (JSON simples):
   ```json
   {
     "message": "Test invocation from Console"
   }
   ```
2. Invocar Lambda via AWS Console:
   - Acessar AWS Console → Lambda → `video-processing-engine-dev-finalizer`
   - Criar evento de teste com payload acima
   - Executar teste e capturar resposta
3. Invocar Lambda via AWS CLI:
   - Executar comando: `aws lambda invoke --function-name video-processing-engine-dev-finalizer --payload '{"message":"Test from CLI"}' response.json`
   - Verificar conteúdo de `response.json`
4. Validar logs no CloudWatch:
   - Acessar CloudWatch Logs → Grupo `/aws/lambda/video-processing-engine-dev-finalizer`
   - Verificar presença de log streams recentes
   - Confirmar que logs contêm:
     - Log de entrada com payload recebido
     - Log de saída com mensagem "Hello World from Finalizer"
     - RequestId correspondente à invocação
5. Documentar no README.md:
   - Comandos para invocar Lambda
   - Como acessar logs no CloudWatch
   - Exemplo de resposta esperada

## Formas de Teste
1. **Teste de invocação via Console:**
   - Criar evento de teste no Console e executar
   - Verificar resposta contém campos esperados: `Message`, `Input`, `RequestId`
2. **Teste de invocação via CLI:**
   - Executar comando `aws lambda invoke` e validar arquivo de resposta
   - Confirmar que status code é 200
3. **Teste de logs:**
   - Acessar CloudWatch Logs e localizar log stream mais recente
   - Verificar que logs contêm timestamp, nível (Information), mensagem

## Critérios de Aceite da Subtask
- [ ] Lambda invocada com sucesso via AWS Console retornando objeto com `Message`, `Input`, `RequestId`
- [ ] Lambda invocada com sucesso via AWS CLI; arquivo `response.json` contém resposta esperada
- [ ] Logs aparecem no CloudWatch Logs no grupo `/aws/lambda/video-processing-engine-dev-finalizer`
- [ ] Logs contêm informações de entrada (payload recebido) e saída (mensagem de resposta)
- [ ] RequestId no log corresponde ao RequestId da invocação
- [ ] README.md atualizado com instruções de invocação e acesso aos logs
- [ ] Captura de tela ou exemplo de log incluído na documentação (opcional)
