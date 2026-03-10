# Subtask 03: Atualizar GitHub Actions e referências de caminho

## Descrição
Atualizar o workflow de deploy (GitHub Actions) e todas as referências em documentação (README, docs) para os novos caminhos dos projetos: Lambda em `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/` e testes em `tests/VideoProcessing.Finalizer.Tests/`.

## Passos de Implementação
1. Editar `.github/workflows/deploy-lambda.yml`:
   - No step "Publish application", alterar o caminho do projeto de `src/VideoProcessing.Finalizer/VideoProcessing.Finalizer.csproj` para `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj` (ou o path exato usado na solution).
   - Garantir que os passos de restore, build e test usem a raiz do repositório e que `dotnet test` encontre o projeto em `tests/`.
2. Revisar `README.md` na raiz e em `VideoProcessing.Finalizer.Lambda` (agora em `src/InterfacesExternas/...`): atualizar instruções de build, execução local e caminhos de projetos citados.
3. Revisar arquivos em `docs/` que mencionem caminhos como `src/VideoProcessing.Finalizer`, `test/` ou a Lambda na raiz; atualizar para `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda` e `tests/`.
4. Verificar `aws-lambda-tools-defaults.json` e `Properties/launchSettings.json` no projeto Lambda: ajustar caminhos relativos apenas se referirem a raiz do repo ou a `test/`.

## Formas de Teste
1. **Build/restore no CI:** executar localmente os mesmos comandos do workflow (`dotnet restore`, `dotnet build`, `dotnet test`) na raiz e validar que não há referências a caminhos antigos.
2. **Simulação de publish:** executar o comando `dotnet publish` do workflow com o novo caminho e confirmar que o artefato é gerado em `./publish` sem erros.
3. **Revisão de documentação:** busca por "VideoProcessing.Finalizer", "test/VideoProcessing", "src/VideoProcessing.Finalizer" em README e docs para garantir que não restam referências quebradas.

## Critérios de Aceite da Subtask
- [ ] Workflow `deploy-lambda.yml` publica o projeto usando o caminho `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj`
- [ ] README.md (raiz e, se aplicável, do projeto Lambda) reflete a nova estrutura de pastas e comandos de build/test
- [ ] Documentação em `docs/` atualizada para os novos caminhos; nenhuma referência ao caminho antigo da Lambda na raiz ou a `test/`
- [ ] Execução local do fluxo do workflow (restore, build, test, publish) conclui com sucesso na raiz do repositório
