# Subtask 04: Documentar Secrets, Variables, SonarCloud e Branch Protection

## Descrição
Documentar no README (ou em documento indicado no README) os passos para configurar o SonarCloud no repositório: Secrets e Variables no GitHub, criação do projeto no SonarCloud com Automatic Analysis desativada e Branch Protection Rule com check obrigatório do SonarCloud.

## Passos de implementação
1. No README, adicionar uma seção (ex.: "SonarCloud" ou dentro de "Deploy via GitHub Actions") descrevendo:
   - **Secrets:** `SONAR_TOKEN` — gerado em [sonarcloud.io/account/security](https://sonarcloud.io/account/security), configurado em Settings > Secrets and variables > Actions > Secrets.
   - **Variables:** `SONAR_PROJECT_KEY` (chave do projeto no SonarCloud) e `SONAR_ORGANIZATION` (slug da organização), em Settings > Variables > Actions.
2. Incluir instrução para criar o projeto no SonarCloud (se ainda não existir) e **desativar Automatic Analysis** em Administration → Analysis Method, para evitar falha do pipeline com "You are running CI analysis while Automatic Analysis is enabled".
3. Incluir instrução para configurar Branch Protection Rule para a branch `main`: habilitar "Require status checks to pass before merging" e adicionar o check "SonarCloud Analysis" (nome exato do job no workflow). Mencionar que no SonarCloud (Project Settings > GitHub) o webhook deve estar ativo para reportar o Quality Gate na PR.
4. Opcional: referência à skill `.cursor/skills/sonarcloud-dotnet/SKILL.md` para armadilhas e checklist.

## Formas de teste
- **Manual:** Seguir a documentação e configurar um repo de teste; validar que o job SonarCloud roda e que o merge na `main` exige o status do check.
- **Revisão:** Verificar se um novo desenvolvedor consegue configurar o SonarCloud apenas com o README.

## Critérios de aceite da subtask
- [x] README (ou doc referenciada) descreve configuração de Secret `SONAR_TOKEN` e Variables `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION`.
- [x] Documentação explica desativar Automatic Analysis no SonarCloud (Administration → Analysis Method).
- [x] Documentação explica Branch Protection Rule para `main` com check obrigatório "SonarCloud Analysis" e webhook no SonarCloud.
