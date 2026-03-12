# Subtask 05: Validar deploy e execução no Lambda com logs

## Descrição
Fazer deploy da Lambda atualizada via GitHub Actions, invocar com evento de teste contendo arquivos em base64, e validar que o ZIP é gerado em `/tmp` e a resposta contém informações corretas, além de verificar logs no CloudWatch.

## Passos de Implementação
1. Preparar payload de teste JSON:
   ```json
   {
     "videoId": "test-video-001",
     "files": [
       {
         "fileName": "frame-001.jpg",
         "contentBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
       },
       {
         "fileName": "frame-002.jpg",
         "contentBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
       }
     ]
   }
   ```
   (Base64 acima representa PNG 1x1 pixel transparente)
2. Fazer commit das mudanças e push para `main`:
   - Workflow GitHub Actions deve executar build, testes e deploy
   - Verificar que deploy foi bem-sucedido
3. Invocar Lambda via AWS Console:
   - Criar evento de teste com payload acima
   - Executar teste e capturar resposta
4. Validar resposta:
   - Verificar que `success` é `true`
   - Verificar que `processedFileCount` é 2
   - Verificar que `zipSizeBytes` é > 0
   - Verificar que `zipFilePath` contém caminho em `/tmp`
5. Validar logs no CloudWatch:
   - Acessar CloudWatch Logs → `/aws/lambda/video-processing-engine-dev-finalizer`
   - Verificar logs estruturados:
     - "Received finalizer event for video test-video-001, files: 2"
     - "Created working directory at /tmp/finalizer/test-video-001"
     - "Wrote file frame-001.jpg, size: X bytes"
     - "ZIP created successfully at /tmp/finalizer/test-video-001/output.zip, size: X bytes"
     - "Cleaned up directory /tmp/finalizer/test-video-001"
6. Documentar no README.md:
   - Exemplo de payload de teste
   - Como invocar Lambda via Console/CLI
   - Como interpretar logs e resposta

## Formas de Teste
1. **Teste via Console:**
   - Executar evento de teste e verificar resposta
2. **Teste via CLI:**
   - `aws lambda invoke --function-name video-processing-engine-dev-finalizer --payload file://test-event.json response.json`
   - Verificar `response.json`
3. **Validação de logs:**
   - Buscar log stream mais recente e inspecionar mensagens

## Critérios de Aceite da Subtask
- [ ] Deploy via GitHub Actions executado com sucesso após commit/push
- [ ] Lambda invocada via Console com payload de teste retorna resposta esperada
- [ ] Resposta contém `success: true`, `processedFileCount: 2`, `zipSizeBytes > 0`, `zipFilePath` válido
- [ ] Logs estruturados aparecem no CloudWatch com informações de cada etapa do processamento
- [ ] Logs confirmam criação de diretório, escrita de arquivos, geração de ZIP, limpeza
- [ ] README.md atualizado com exemplo de payload, comandos de invocação, interpretação de logs
- [ ] Validação manual confirmada: ZIP gerado contém os arquivos esperados (inferido pelos logs)
