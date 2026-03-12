# Subtask 02: Configurar GitHub Actions para deploy via ZIP

## Descrição
Criar workflow do GitHub Actions que realiza build, executa testes, gera o pacote ZIP e faz deploy na Lambda AWS usando credenciais configuradas nos Secrets.

## Passos de Implementação
1. Criar arquivo `.github/workflows/deploy-lambda.yml`
2. Configurar triggers:
   - Push para branch `main`
   - Pull Request para `main` (apenas build e testes, sem deploy)
   - `workflow_dispatch` para execução manual
3. Implementar job `build-and-deploy`:
   - Checkout do código
   - Setup .NET 10
   - Restore de dependências (`dotnet restore`)
   - Build em modo Release (`dotnet build --configuration Release`)
   - Executar testes (`dotnet test --no-build`)
   - Publish para linux-x64 (`dotnet publish --configuration Release --runtime linux-x64 --self-contained false`)
   - Criar ZIP a partir da pasta de publish
   - Configurar AWS credentials usando secrets `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_SESSION_TOKEN` (opcional)
   - Deploy via `aws lambda update-function-code --function-name video-processing-engine-dev-finalizer --zip-file fileb://deployment-package.zip`
   - Aguardar Lambda ficar em estado `Active` (wait loop)
   - Verificar deploy com `aws lambda get-function`
   - Upload do ZIP como artifact
4. Adicionar condição de deploy somente em push (não em PR):
   - `if: github.event_name == 'push'` no step de deploy
5. Documentar no README.md: como configurar Secrets, variáveis esperadas, execução manual

## Formas de Teste
1. **Teste de sintaxe:**
   - Validar YAML com yamllint ou GitHub Actions validator
2. **Teste de execução (dry-run):**
   - Executar workflow manualmente via `workflow_dispatch` em branch de feature
   - Verificar logs de cada step no GitHub Actions
3. **Teste de deploy real:**
   - Fazer commit em branch de teste, abrir PR
   - Verificar que build e testes executam mas deploy não ocorre
   - Fazer merge para `main` e validar deploy completo

## Critérios de Aceite da Subtask
- [ ] Workflow `.github/workflows/deploy-lambda.yml` criado e funcionando
- [ ] Triggers configurados: push para `main`, PR para `main`, workflow_dispatch
- [ ] Steps executam na ordem correta: build → test → publish → deploy
- [ ] Deploy condicional: só ocorre em push para `main` (não em PRs)
- [ ] AWS credentials configuradas via secrets (AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, AWS_SESSION_TOKEN)
- [ ] Lambda atualizada com sucesso e workflow aguarda estado `Active`
- [ ] Artifact `deployment-package.zip` disponível para download no workflow
- [ ] README.md documentando configuração de Secrets e execução do workflow
