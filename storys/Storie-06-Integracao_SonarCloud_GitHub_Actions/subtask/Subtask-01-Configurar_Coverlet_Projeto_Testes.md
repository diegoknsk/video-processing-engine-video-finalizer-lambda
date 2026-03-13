# Subtask 01: Configurar Coverlet no projeto de testes

## Descrição
Adicionar e ajustar os pacotes Coverlet no projeto de testes para gerar relatório OpenCover compatível com o SonarScanner for .NET, seguindo a skill sonarcloud-dotnet.

## Passos de implementação
1. Abrir `src/tests/VideoProcessing.Finalizer.Tests/VideoProcessing.Finalizer.Tests.csproj`.
2. Garantir referência a `coverlet.collector` (versão 6.0.2 ou superior, ex.: 6.0.4) com:
   - `IncludeAssets`: `runtime; build; native; contentfiles; analyzers; buildtransitive`
   - `PrivateAssets`: `all`
3. Adicionar referência a `coverlet.msbuild` (mesma versão do collector) com os mesmos `IncludeAssets` e `PrivateAssets`.
4. Executar `dotnet restore` e `dotnet test` com os parâmetros de cobertura (CollectCoverage, CoverletOutputFormat=opencover, CoverletOutput=./TestResults/coverage.opencover.xml) para validar que o XML é gerado.

## Formas de teste
- **Unitário/CI:** Executar `dotnet test` com `/p:CollectCoverage=true`, `/p:CoverageReporter=opencover`, `/p:CoverletOutputFormat=opencover`, `/p:CoverletOutput=./TestResults/coverage.opencover.xml` e verificar que existe `**/TestResults/**/coverage.opencover.xml`.
- **Manual:** Abrir o `.csproj` e confirmar que não há pacote duplicado e que os atributos estão corretos.
- **Integração:** Rodar o job `sonar-analysis` (após Subtask 02) e conferir no SonarCloud que a cobertura é exibida.

## Critérios de aceite da subtask
- [x] `coverlet.collector` e `coverlet.msbuild` presentes no `.csproj` de testes com `IncludeAssets` e `PrivateAssets` conforme skill.
- [x] `dotnet test` com os parâmetros de cobertura gera `coverage.opencover.xml` em `TestResults`.
- [x] Nenhum warning de pacote duplicado ou conflito no restore/build.
