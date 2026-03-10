# Subtask 02: Mover projeto de testes e atualizar solution

## Descrição
Mover o projeto de testes de `test/VideoProcessing.Finalizer.Tests/` para `tests/VideoProcessing.Finalizer.Tests/` e atualizar o arquivo de solution (.slnx) para usar os caminhos físicos corretos dos projetos e organizar os itens em pastas virtuais que reflitam a Clean Architecture (src/Core, src/Infra, src/InterfacesExternas, tests).

## Passos de Implementação
1. Criar a pasta `tests/` na raiz (se não existir) e mover a pasta `test/VideoProcessing.Finalizer.Tests/` para `tests/VideoProcessing.Finalizer.Tests/`, preservando todo o conteúdo (arquivos .cs, .csproj, etc.).
2. Atualizar o arquivo `VideoProcessing.Finalizer.slnx`:
   - Ajustar o caminho do projeto Lambda para `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj`
   - Incluir o projeto de testes com caminho `tests/VideoProcessing.Finalizer.Tests/VideoProcessing.Finalizer.Tests.csproj`
   - Organizar em pastas virtuais: projeto Lambda dentro de uma pasta virtual equivalente a `src/InterfacesExternas` (ou `/src/InterfacesExternas/`); projeto de testes dentro de `tests/`; manter pastas `src/Core` e `src/Infra` vazias no solution se o formato permitir, ou documentar que projetos futuros seguirão essa convenção.
3. Atualizar a referência de projeto no .csproj de testes (ProjectReference) para apontar ao novo caminho relativo do projeto Lambda, se necessário.
4. Remover a pasta `test/` da raiz após confirmar que o projeto de testes está íntegro em `tests/`.

## Formas de Teste
1. **Build da solution:** executar `dotnet build` na raiz e verificar que ambos os projetos (Lambda e Tests) são encontrados e compilam.
2. **Execução de testes:** executar `dotnet test` na raiz e confirmar que todos os testes passam.
3. **Abertura no IDE:** abrir a solution no Visual Studio ou Rider e verificar que a árvore de pastas virtuais exibe a estrutura esperada (InterfacesExternas, tests).

## Critérios de Aceite da Subtask
- [ ] Projeto de testes está em `tests/VideoProcessing.Finalizer.Tests/`; pasta `test/` não existe mais na raiz
- [ ] Arquivo .slnx referencia os caminhos corretos para Lambda e para o projeto de testes
- [ ] `dotnet build` e `dotnet test` executados na raiz do repositório passam sem erros
- [ ] Estrutura de pastas virtuais na solution reflete a organização (InterfacesExternas, tests)
