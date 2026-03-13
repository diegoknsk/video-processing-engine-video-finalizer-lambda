# Subtask 05: Badges Quality Gate e Coverage no README

## Descrição
Adicionar ao README os badges oficiais do SonarCloud para Quality Gate e Coverage, apontando para o projeto configurado (usando `SONAR_ORGANIZATION` e `SONAR_PROJECT_KEY` ou o nome do repositório no SonarCloud).

## Passos de implementação
1. Obter a URL do projeto no SonarCloud (ex.: organização + project key ou o link exibido no dashboard do projeto).
2. Inserir no topo do README (após o título ou na primeira seção) os badges no formato SonarCloud:
   - **Quality Gate:**  
     `[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=<PROJECT_KEY>&metric=alert_status)](https://sonarcloud.io/summary/new_code?project=<PROJECT_KEY>)`
   - **Coverage:**  
     `[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=<PROJECT_KEY>&metric=coverage)](https://sonarcloud.io/summary/new_code?project=<PROJECT_KEY>)`
3. Substituir `<PROJECT_KEY>` pelo valor real do projeto (geralmente `SONAR_ORGANIZATION_REPOSITORY` ou o project key exibido no SonarCloud). Se o projeto for criado após a story, usar placeholder documentado (ex.: `SONAR_PROJECT_KEY`) ou o slug esperado (ex.: `org_repo`).
4. Garantir que os links "summary" apontam para o projeto correto no SonarCloud.

## Formas de teste
- **Manual:** Abrir o README no GitHub e verificar que os badges são renderizados e que os links levam ao projeto no SonarCloud.
- **Manual:** Após a primeira análise no SonarCloud, confirmar que Quality Gate e Coverage exibem valores (ou "unknown" até a primeira execução).

## Critérios de aceite da subtask
- [x] README contém badge de Quality Gate Status (SonarCloud) com link para o summary do projeto.
- [x] README contém badge de Coverage (SonarCloud) com link para o summary do projeto.
- [x] Project key nos badges corresponde ao projeto configurado (variável ou valor documentado).
