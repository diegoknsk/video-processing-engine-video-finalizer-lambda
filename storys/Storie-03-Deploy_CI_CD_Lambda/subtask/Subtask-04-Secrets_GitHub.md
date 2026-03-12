# Subtask-04: Validar e documentar secrets necessários no GitHub

## Descrição
Verificar que os secrets e variáveis de repositório necessários para o deploy estão configurados corretamente no GitHub, e documentar o processo para facilitar a rotação periódica das credenciais (que expiram no contexto da FIAP/Academy).

## Passos de Implementação

1. **Verificar secrets obrigatórios no repositório GitHub (Settings → Secrets and variables → Actions):**
   - `AWS_ACCESS_KEY_ID` — chave de acesso da IAM user/role.
   - `AWS_SECRET_ACCESS_KEY` — chave secreta correspondente.
   - `AWS_SESSION_TOKEN` — token de sessão (credenciais temporárias; expira periodicamente).
   - `AWS_REGION` — região da AWS (ex.: `us-east-1`).

2. **Verificar variável de repositório opcional:**
   - `LAMBDA_FUNCTION_NAME` — se não configurada, o workflow usa o fallback `video-processing-engine-dev-finalizer`.

3. **Documentar no `aws-lambda-tools-defaults.json` ou no README da Lambda** como obter e renovar as credenciais temporárias (importante porque `AWS_SESSION_TOKEN` expira).

## Formas de Teste

1. Verificar no GitHub (Settings → Secrets) que os 4 secrets estão listados (não é possível ver os valores, apenas confirmar existência).
2. Executar o workflow manualmente via "Run workflow" (workflow_dispatch) e verificar que o step "Configure AWS credentials" passa sem erro de autenticação.
3. Após o pipeline, executar `aws lambda get-function --function-name video-processing-engine-dev-finalizer` localmente com as mesmas credenciais para confirmar que o acesso funciona.

## Critérios de Aceite

- [ ] Os 4 secrets (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_SESSION_TOKEN`, `AWS_REGION`) estão configurados no repositório GitHub.
- [ ] O step "Configure AWS credentials" do workflow não retorna erro de autenticação quando os secrets estão válidos.
- [ ] A rotação das credenciais está documentada (onde obtê-las e como atualizar os secrets no GitHub).
