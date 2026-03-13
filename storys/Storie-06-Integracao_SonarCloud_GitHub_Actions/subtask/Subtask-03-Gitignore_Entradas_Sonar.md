# Subtask 03: Atualizar .gitignore com entradas Sonar

## Descrição
Incluir no `.gitignore` as entradas recomendadas pela skill sonarcloud-dotnet para evitar versionar artefatos gerados pelo SonarScanner e pelo Coverlet.

## Passos de implementação
1. Abrir o `.gitignore` na raiz do repositório.
2. Adicionar uma seção comentada (ex.: `# SonarCloud / SonarQube`) e as entradas:
   - `.sonarqube/`
   - `**/.sonarqube/`
   - `**/out/.sonar/`
   - `.scannerwork/`
   - `**/.scannerwork/`
   - `coverage.opencover.xml`
3. Verificar se já existem padrões que cubram esses arquivos (ex.: `coverage*.xml`) e, se necessário, manter ou ajustar para não duplicar conflitos; a skill recomenda listar explicitamente as entradas do Sonar.

## Formas de teste
- **Manual:** Rodar análise Sonar localmente ou no CI e confirmar que pastas `.sonarqube/` e `.scannerwork/` e arquivo `coverage.opencover.xml` não aparecem em `git status`.
- **Manual:** Conferir que o `.gitignore` está versionado e que as novas linhas seguem o padrão do arquivo.

## Critérios de aceite da subtask
- [x] `.gitignore` contém as entradas: `.sonarqube/`, `**/.sonarqube/`, `**/out/.sonar/`, `.scannerwork/`, `**/.scannerwork/`, `coverage.opencover.xml`.
- [x] Após rodar sonarscanner e testes com cobertura, esses artefatos não são listados como untracked pelo Git (no contexto do projeto).
