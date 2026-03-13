# Storie-06: Integrar SonarCloud com cobertura de código via GitHub Actions

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Como desenvolvedor do projeto, quero integrar o SonarCloud com cobertura de código (OpenCover) no pipeline do GitHub Actions, para garantir análise estática, Quality Gate e métricas de cobertura em cada PR para `main` e em pushes para `main`.

## Objetivo
Configurar a análise SonarCloud no repositório: pacotes Coverlet no projeto de testes, job `sonar-analysis` no workflow (com execução em PR para `main` e em push para `main`), secrets/variáveis no GitHub, projeto no SonarCloud com Automatic Analysis desativada, `.gitignore` atualizado, Branch Protection com check obrigatório do SonarCloud e badges de Quality Gate e Coverage no README.

## Escopo Técnico
- Tecnologias: SonarCloud, SonarScanner for .NET, Coverlet (OpenCover), GitHub Actions
- Arquivos afetados:
  - `src/tests/VideoProcessing.Finalizer.Tests/VideoProcessing.Finalizer.Tests.csproj`
  - `.github/workflows/deploy-lambda.yml`
  - `.gitignore`
  - `README.md`
- Componentes: job `sonar-analysis`, trigger `pull_request` para `main`, variáveis e secrets do GitHub, projeto SonarCloud, Branch Protection Rule
- Pacotes/Dependências:
  - coverlet.collector (6.0.2 ou compatível, ex.: 6.0.4) com `IncludeAssets` e `PrivateAssets` conforme skill
  - coverlet.msbuild (6.0.2 ou compatível) com `IncludeAssets` e `PrivateAssets` conforme skill

## Dependências e Riscos (para estimativa)
- Dependências: conta no SonarCloud (organização ou usuário), repositório no GitHub.
- Riscos: Automatic Analysis ativa no SonarCloud causa falha do pipeline (exit code 1) — desativar em Administration → Analysis Method. Projeto duplicado se repositório for público e projeto manual coexistir — usar um único projeto e desativar Automatic Analysis. Consultar `.cursor/skills/sonarcloud-dotnet/SKILL.md` para armadilhas e checklist.

## Subtasks
- [x] [Subtask 01: Configurar Coverlet no projeto de testes](./subtask/Subtask-01-Configurar_Coverlet_Projeto_Testes.md)
- [x] [Subtask 02: Job sonar-analysis e trigger em PR para main](./subtask/Subtask-02-Job_Sonar_Analysis_Trigger_PR_Main.md)
- [x] [Subtask 03: Atualizar .gitignore com entradas Sonar](./subtask/Subtask-03-Gitignore_Entradas_Sonar.md)
- [x] [Subtask 04: Documentar Secrets, Variables, SonarCloud e Branch Protection](./subtask/Subtask-04-Documentar_Secrets_SonarCloud_Branch_Protection.md)
- [x] [Subtask 05: Badges Quality Gate e Coverage no README](./subtask/Subtask-05-Badges_Quality_Gate_Coverage_README.md)

## Critérios de Aceite da História
- [x] Pacotes `coverlet.collector` e `coverlet.msbuild` configurados no `.csproj` de testes com `IncludeAssets` e `PrivateAssets` conforme skill
- [x] Workflow dispara em `pull_request` para `main` e em `push` para `main`; job `sonar-analysis` executa com `fetch-depth: 0`, `sonar.projectBaseDir="${{ github.workspace }}"`, cobertura OpenCover e deploy depende de `sonar-analysis` (`needs: [sonar-analysis]`)
- [x] `.gitignore` contém entradas do Sonar (`.sonarqube/`, `.scannerwork/`, `coverage.opencover.xml`, etc.)
- [x] README ou documentação descreve configuração de Secret `SONAR_TOKEN`, Variables `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION`, criação do projeto no SonarCloud com Automatic Analysis desativada e Branch Protection Rule com check "SonarCloud Analysis"
- [x] README exibe badges de Quality Gate e Coverage do SonarCloud (links corretos para o projeto)
- [x] Nenhum arquivo `sonar-project.properties` na raiz do repositório

## Rastreamento (dev tracking)
- **Início:** 13/03/2026, às 13:11 (Brasília)
- **Fim:** —
- **Tempo total de desenvolvimento:** —
