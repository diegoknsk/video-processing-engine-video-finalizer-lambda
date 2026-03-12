# Storie-03: Deploy CI/CD do Lambda Finalizer via GitHub Actions

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Como engenheiro de plataforma, quero um pipeline de CI/CD automatizado para o Lambda Finalizer, para garantir que todo push na main faça build, teste e atualize o código e o handler da função Lambda na AWS de forma confiável.

## Objetivo
Corrigir e finalizar o workflow `.github/workflows/deploy-lambda.yml` para que ele: separe os jobs de CI (build + test) e deploy; use `dotnet lambda package` (Amazon.Lambda.Tools) para gerar o pacote correto para Lambda; atualize o código da função e configure explicitamente o handler após cada deploy. A função já existe na AWS como uma casca; a entrega desta story é o primeiro deploy funcional.

## Escopo Técnico
- Tecnologias: .NET 10, GitHub Actions, Amazon.Lambda.Tools, AWS CLI
- Arquivos afetados:
  - `.github/workflows/deploy-lambda.yml`
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/aws-lambda-tools-defaults.json`
- Componentes/Recursos: GitHub Actions workflow, Lambda na AWS (`video-processing-engine-dev-finalizer`)
- Pacotes/Dependências:
  - Amazon.Lambda.Tools (CLI tool — dotnet tool install -g Amazon.Lambda.Tools)
  - actions/checkout@v4
  - actions/setup-dotnet@v4
  - aws-actions/configure-aws-credentials@v4
  - actions/upload-artifact@v4

## Dependências e Riscos (para estimativa)
- Dependências: Função Lambda já criada na AWS (`video-processing-engine-dev-finalizer`); secrets configurados no repositório GitHub (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_SESSION_TOKEN`, `AWS_REGION`).
- Riscos/Pré-condições:
  - Credenciais AWS com permissão `lambda:UpdateFunctionCode` e `lambda:UpdateFunctionConfiguration` devem estar nos secrets do GitHub.
  - A função Lambda deve existir na AWS antes do primeiro deploy.
  - O `AWS_SESSION_TOKEN` expira periodicamente (credenciais temporárias da FIAP/Academy).

## Subtasks
- [Subtask 01: Refatorar workflow para usar dotnet lambda package e separar jobs](./subtask/Subtask-01-Refatorar_Workflow.md)
- [Subtask 02: Configurar handler no step de deploy](./subtask/Subtask-02-Configurar_Handler.md)
- [Subtask 03: Validar aws-lambda-tools-defaults e configurações do projeto](./subtask/Subtask-03-Validar_Configuracoes_Projeto.md)
- [Subtask 04: Validar e documentar secrets necessários no GitHub](./subtask/Subtask-04-Secrets_GitHub.md)

## Critérios de Aceite da História
- [ ] O job `build-and-test` executa restore, build Release e `dotnet test` com sucesso; falhas de teste bloqueiam o deploy.
- [ ] O job `deploy` só executa em push na `main` e usa `dotnet lambda package` via Amazon.Lambda.Tools para gerar o zip.
- [ ] Após upload do zip com `aws lambda update-function-code`, o pipeline aguarda a atualização completar (`aws lambda wait function-updated`) e em seguida chama `aws lambda update-function-configuration` com o handler `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`.
- [ ] O pipeline aguarda a atualização de configuração concluir antes de finalizar.
- [ ] O pacote gerado é salvo como artefato do GitHub Actions para rastreabilidade.
- [ ] Secrets `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_SESSION_TOKEN` e `AWS_REGION` estão documentados como requisito no `aws-lambda-tools-defaults.json` ou README da Lambda.
- [ ] O nome da função Lambda é configurável via variável de repositório `LAMBDA_FUNCTION_NAME` com fallback para `video-processing-engine-dev-finalizer`.

## Rastreamento (dev tracking)
- **Início:** —
- **Fim:** —
- **Tempo total de desenvolvimento:** —
