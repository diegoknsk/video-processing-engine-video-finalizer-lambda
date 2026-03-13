# Subtask 02: Job sonar-analysis e trigger em PR para main

## Descrição
Adicionar o job `sonar-analysis` ao workflow do GitHub Actions, configurar o trigger para execução em pull requests que tenham como destino a branch `main` e garantir que o deploy dependa do resultado da análise SonarCloud.

## Passos de implementação
1. Em `.github/workflows/deploy-lambda.yml`, adicionar em `on:` o trigger `pull_request: branches: [main]` para que o workflow rode em PRs destinados a `main`.
2. Inserir o job `sonar-analysis` **antes** do job `deploy`, com:
   - `runs-on: ubuntu-latest`
   - Checkout com `fetch-depth: 0`
   - Setup .NET 10
   - Instalação do `dotnet-sonarscanner` (ferramenta global)
   - Restore, Begin analysis (usar `vars.SONAR_PROJECT_KEY`, `vars.SONAR_ORGANIZATION`, `secrets.SONAR_TOKEN`, `sonar.projectBaseDir="${{ github.workspace }}"`, `sonar.sources`, `sonar.tests`, `sonar.cs.opencover.reportsPaths` conforme skill)
   - Build em Release
   - Run tests com cobertura OpenCover (`CollectCoverage`, `CoverageReporter=opencover`, `CoverletOutput=./TestResults/coverage.opencover.xml`)
   - End SonarCloud analysis
3. Fazer o job `build-and-test` ser substituído ou integrado pela sequência: o job que faz build/test pode ser o próprio `sonar-analysis` ou um job separado; o job `deploy` deve declarar `needs: [sonar-analysis]` (e manter `needs: build-and-test` apenas se `build-and-test` continuar existindo; caso se unifique build+test no sonar-analysis, usar `needs: [sonar-analysis]` no deploy).
4. Ajustar o job `deploy` para depender de `sonar-analysis` (ex.: `needs: [sonar-analysis]`), mantendo `if: github.ref == 'refs/heads/main'` para rodar apenas em push para `main`.

## Formas de teste
- **CI:** Abrir um PR para `main` e verificar que o workflow dispara e que o job "SonarCloud Analysis" executa e conclui com sucesso.
- **CI:** Fazer push para `main` e verificar que `sonar-analysis` e em seguida `deploy` executam.
- **Manual:** Conferir no SonarCloud que a análise e a cobertura aparecem para a branch/PR.

## Critérios de aceite da subtask
- [x] Workflow dispara em `pull_request` para branch `main` e em `push` para `main`.
- [x] Job `sonar-analysis` existe, usa `fetch-depth: 0` e `sonar.projectBaseDir="${{ github.workspace }}"`; não existe `sonar-project.properties` na raiz.
- [x] Job `deploy` declara `needs: [sonar-analysis]` e só roda em push para `main`.
- [x] SonarCloud recebe análise e cobertura (após configurar token e variáveis no repositório).
