# Subtask 06: Documentar permissões IAM e configuração de buckets

## Descrição
Documentar no README.md as permissões IAM necessárias para a Lambda Finalizer, a estrutura esperada dos buckets S3, convenções de prefixos/chaves, e troubleshooting de erros comuns relacionados a S3.

## Passos de Implementação
1. Adicionar seção "Permissões IAM" no README.md:
   - Documentar policy IAM mínima necessária:
     ```json
     {
       "Version": "2012-10-17",
       "Statement": [
         {
           "Effect": "Allow",
           "Action": [
             "s3:GetObject",
             "s3:ListBucket"
           ],
           "Resource": [
             "arn:aws:s3:::video-processing-frames-dev",
             "arn:aws:s3:::video-processing-frames-dev/*"
           ]
         },
         {
           "Effect": "Allow",
           "Action": [
             "s3:PutObject"
           ],
           "Resource": [
             "arn:aws:s3:::video-processing-output-dev/*"
           ]
         }
       ]
     }
     ```
   - Explicar cada permissão (GetObject, ListBucket, PutObject)
   - Instruções de como anexar policy à role da Lambda
2. Adicionar seção "Estrutura de Buckets" no README.md:
   - Documentar estrutura esperada:
     - Bucket de entrada: `video-processing-frames-{env}`
     - Prefixo de entrada: `videos/{videoId}/frames/`
     - Bucket de saída: `video-processing-output-{env}`
     - Chave de saída padrão: `videos/{videoId}/output.zip`
   - Incluir diagrama simples (ASCII art ou Mermaid)
3. Adicionar seção "Convenções de Prefixos" no README.md:
   - Explicar que prefixos devem terminar com `/`
   - Descrever como a Lambda lista objetos dentro do prefixo
   - Explicar paginação (suporta >1000 objetos)
4. Adicionar seção "Troubleshooting S3" no README.md:
   - **Erro "Access Denied"**: Verificar permissões IAM, verificar nome do bucket
   - **Erro "Bucket does not exist"**: Verificar que bucket foi criado, verificar região
   - **ZIP vazio**: Verificar que prefixo está correto, verificar que objetos existem no bucket
   - **Timeout**: Considerar aumentar timeout da Lambda, considerar otimizar quantidade de arquivos
5. Adicionar exemplos de comandos úteis (AWS CLI):
   - Listar objetos em prefixo: `aws s3 ls s3://bucket/prefix/`
   - Fazer upload de imagens de teste: `aws s3 sync ./local-folder s3://bucket/prefix/`
   - Baixar ZIP de saída: `aws s3 cp s3://bucket/key ./output.zip`

## Formas de Teste
1. **Revisão de documentação:**
   - Ler README.md e verificar clareza e completude
2. **Validação de comandos:**
   - Executar comandos AWS CLI de exemplo e verificar que funcionam
3. **Revisão por pares:**
   - Solicitar que outro desenvolvedor revise a documentação

## Critérios de Aceite da Subtask
- [ ] Seção "Permissões IAM" adicionada ao README.md com policy JSON completa e instruções
- [ ] Seção "Estrutura de Buckets" adicionada com convenções de prefixos e chaves
- [ ] Seção "Convenções de Prefixos" explicando formato esperado e paginação
- [ ] Seção "Troubleshooting S3" com erros comuns e soluções
- [ ] Exemplos de comandos AWS CLI adicionados para operações comuns
- [ ] README.md revisado e validado por teste manual dos comandos
- [ ] Documentação clara, completa e acessível para desenvolvedores e operadores
