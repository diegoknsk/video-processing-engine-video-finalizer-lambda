# Subtask 02: Atualizar modelos de evento para incluir informações de S3

## Descrição
Atualizar o record `FinalizerEvent` para incluir informações sobre buckets e prefixos S3, removendo ou mantendo compatibilidade com a propriedade `Files` (base64) para suportar testes.

## Passos de Implementação
1. Atualizar `FinalizerEvent.cs`:
   - Adicionar propriedades:
     - `string? S3SourceBucket` (bucket de origem das imagens)
     - `string? S3SourcePrefix` (prefixo/diretório das imagens)
     - `string? S3OutputBucket` (bucket de destino do ZIP)
     - `string? S3OutputKey` (chave/caminho do ZIP no bucket de saída, opcional; se não informado, gerar chave padrão)
   - Manter propriedade `List<FileItem>? Files` para compatibilidade com testes locais (fazer nullable)
   - Adicionar lógica de validação: se `S3SourceBucket` for informado, `S3SourcePrefix` e `S3OutputBucket` também devem estar preenchidos
2. Atualizar `FinalizerResponse.cs`:
   - Adicionar propriedades:
     - `string? S3OutputBucket`
     - `string? S3OutputKey`
   - Manter `ZipFilePath` para compatibilidade
3. Documentar no XML comments:
   - Explicar que modelo suporta dois modos: base64 (para testes) e S3 (produção)
   - Descrever formato de prefixo esperado: `videos/{videoId}/frames/`
4. Adicionar exemplo de payload JSON atualizado no README.md:
   ```json
   {
     "videoId": "video-123",
     "s3SourceBucket": "video-processing-frames",
     "s3SourcePrefix": "videos/video-123/frames/",
     "s3OutputBucket": "video-processing-output",
     "s3OutputKey": "videos/video-123/output.zip"
   }
   ```

## Formas de Teste
1. **Compilação:**
   - Executar `dotnet build` e verificar ausência de erros
2. **Serialização/Deserialização:**
   - Criar instância com propriedades S3 e serializar/deserializar
   - Verificar que ambos os modos (base64 e S3) funcionam
3. **Validação de lógica:**
   - Testar que payload com S3 incompleto gera erro apropriado (na Subtask 03)

## Critérios de Aceite da Subtask
- [ ] Record `FinalizerEvent` atualizado com propriedades `S3SourceBucket`, `S3SourcePrefix`, `S3OutputBucket`, `S3OutputKey`
- [ ] Propriedade `Files` mantida como nullable para compatibilidade
- [ ] Record `FinalizerResponse` atualizado com `S3OutputBucket` e `S3OutputKey`
- [ ] XML comments documentando dois modos de operação: base64 e S3
- [ ] Build da solution executado com sucesso
- [ ] README.md atualizado com exemplo de payload S3
- [ ] Serialização/Deserialização de payload S3 testada manualmente
