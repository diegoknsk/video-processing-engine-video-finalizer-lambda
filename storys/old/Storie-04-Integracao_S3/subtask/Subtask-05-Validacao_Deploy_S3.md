# Subtask 05: Validar deploy e execução end-to-end com S3

## Descrição
Fazer deploy da Lambda atualizada via GitHub Actions, preparar buckets de teste no S3 com imagens de amostra, invocar Lambda com evento S3, e validar que o ZIP é gerado e armazenado corretamente no bucket de saída, além de verificar logs no CloudWatch.

## Passos de Implementação
1. Preparar buckets S3 de teste:
   - Criar bucket de entrada (se não existir): `video-processing-frames-dev`
   - Criar bucket de saída (se não existir): `video-processing-output-dev`
   - Fazer upload de 3-5 imagens de teste para bucket de entrada com prefixo:
     - Exemplo: `videos/test-video-001/frames/frame-001.jpg`, `frame-002.jpg`, etc.
     - Usar AWS Console ou CLI: `aws s3 cp frame-001.jpg s3://video-processing-frames-dev/videos/test-video-001/frames/`
2. Preparar payload de teste JSON:
   ```json
   {
     "videoId": "test-video-001",
     "s3SourceBucket": "video-processing-frames-dev",
     "s3SourcePrefix": "videos/test-video-001/frames/",
     "s3OutputBucket": "video-processing-output-dev",
     "s3OutputKey": "videos/test-video-001/output.zip"
   }
   ```
3. Fazer commit das mudanças e push para `main`:
   - Workflow GitHub Actions deve executar build, testes e deploy
   - Verificar que deploy foi bem-sucedido
4. Invocar Lambda via AWS Console:
   - Criar evento de teste com payload acima
   - Executar teste e capturar resposta
5. Validar resposta:
   - Verificar que `success` é `true`
   - Verificar que `processedFileCount` corresponde à quantidade de imagens no bucket
   - Verificar que `s3OutputBucket` e `s3OutputKey` estão preenchidos
6. Validar objeto no S3:
   - Acessar bucket de saída e verificar que ZIP foi criado na chave esperada
   - Baixar ZIP e abrir para verificar conteúdo (contém as imagens esperadas)
7. Validar logs no CloudWatch:
   - Acessar CloudWatch Logs → `/aws/lambda/video-processing-engine-dev-finalizer`
   - Verificar logs estruturados:
     - "Processing S3 mode for video test-video-001"
     - "Found X objects in S3"
     - "Downloading X images from S3"
     - "ZIP created successfully"
     - "Uploading ZIP to S3 bucket video-processing-output-dev, key videos/test-video-001/output.zip"
     - "Finalization completed for video test-video-001"
8. Documentar no README.md:
   - Passo a passo para configurar buckets de teste
   - Exemplo de payload S3
   - Como validar resultado no S3 e logs

## Formas de Teste
1. **Teste via Console:**
   - Executar evento de teste e verificar resposta
2. **Teste via CLI:**
   - `aws lambda invoke --function-name video-processing-engine-dev-finalizer --payload file://test-event-s3.json response.json`
   - Verificar `response.json`
3. **Validação de S3:**
   - Listar objetos no bucket de saída: `aws s3 ls s3://video-processing-output-dev/videos/test-video-001/`
   - Baixar ZIP: `aws s3 cp s3://video-processing-output-dev/videos/test-video-001/output.zip ./output.zip`
   - Abrir ZIP e verificar conteúdo

## Critérios de Aceite da Subtask
- [ ] Buckets de teste criados e populados com imagens de amostra
- [ ] Deploy via GitHub Actions executado com sucesso
- [ ] Lambda invocada com payload S3 retorna resposta esperada com `success: true`
- [ ] ZIP criado no bucket de saída na chave especificada
- [ ] ZIP baixado e validado manualmente contém todas as imagens do bucket de entrada
- [ ] Logs estruturados aparecem no CloudWatch com informações de cada etapa (listar, baixar, gerar ZIP, fazer upload)
- [ ] README.md atualizado com instruções de configuração de buckets, payload de exemplo, validação de resultado
- [ ] Validação end-to-end confirmada: imagens S3 → Lambda → ZIP no S3 de saída
