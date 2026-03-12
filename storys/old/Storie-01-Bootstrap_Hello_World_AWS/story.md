# Storie-01: Bootstrap + Hello World AWS

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** —

## Descrição
Como desenvolvedor do sistema Video Processing Engine, quero criar a estrutura inicial da Lambda Finalizer com um handler simples funcional, para validar o pipeline de deploy e a integração básica com a AWS.

## Objetivo
Criar a estrutura base da Lambda Finalizer (.NET 10) com handler simples, configurar o projeto, implementar o deploy via GitHub Actions (ZIP) e validar a execução via Console/CLI AWS com logs no CloudWatch.

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda, CloudWatch Logs
- Arquivos criados:
  - `src/VideoProcessing.Finalizer/Function.cs` (handler)
  - `src/VideoProcessing.Finalizer/VideoProcessing.Finalizer.csproj`
  - `test/VideoProcessing.Finalizer.Tests/FunctionTests.cs`
  - `.github/workflows/deploy-lambda.yml`
  - `README.md`
- Componentes: Handler Lambda puro (sem AddAWSLambdaHosting)
- Pacotes/Dependências:
  - Amazon.Lambda.Core (2.3.0)
  - Amazon.Lambda.Serialization.SystemTextJson (2.4.3)
  - xUnit (2.9.0)
  - xUnit.runner.visualstudio (2.8.2)
  - Amazon.Lambda.TestUtilities (2.0.0)
  - Microsoft.NET.Test.Sdk (17.11.0)

## Dependências e Riscos (para estimativa)
- Dependências: Repositório de infra deve ter provisionado a função Lambda `video-processing-engine-dev-finalizer` previamente
- Riscos: 
  - Lambda pode não existir na AWS (validar manualmente antes)
  - Credenciais AWS no GitHub Secrets devem estar configuradas
- Pré-condições: 
  - Acesso AWS configurado
  - GitHub Actions habilitado no repositório

## Subtasks
- [Subtask 01: Criar estrutura do projeto .NET 10 e handler básico](./subtask/Subtask-01-Estrutura_Projeto_Handler.md)
- [Subtask 02: Configurar GitHub Actions para deploy via ZIP](./subtask/Subtask-02-GitHub_Actions_Deploy.md)
- [Subtask 03: Validar deploy e execução com logs no CloudWatch](./subtask/Subtask-03-Validacao_Deploy_CloudWatch.md)
- [Subtask 04: Criar testes unitários básicos do handler](./subtask/Subtask-04-Testes_Unitarios_Handler.md)

## Critérios de Aceite da História
- [ ] Projeto .NET 10 criado com handler Lambda funcional que retorna "Hello World from Finalizer"
- [ ] Deploy via GitHub Actions executado com sucesso, gerando ZIP e atualizando a Lambda
- [ ] Invocação manual via AWS Console/CLI retorna mensagem esperada
- [ ] Logs da execução aparecem no CloudWatch Logs com grupo `/aws/lambda/video-processing-engine-dev-finalizer`
- [ ] Testes unitários do handler passando; cobertura mínima 80%
- [ ] README.md documentando estrutura, deploy e como invocar a Lambda
- [ ] Workflow GitHub Actions valida build, executa testes e faz deploy somente em push para `main`

## Rastreamento (dev tracking)
- **Início:** 15/02/2026, às 18:04 (Brasília)
- **Fim:** —
- **Tempo total de desenvolvimento:** —
