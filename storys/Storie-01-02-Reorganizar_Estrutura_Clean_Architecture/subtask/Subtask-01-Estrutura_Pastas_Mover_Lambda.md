# Subtask 01: Criar estrutura de pastas e mover projeto Lambda

## Descrição
Criar as pastas `src/Core/`, `src/Infra/` e `src/InterfacesExternas/` na raiz do repositório e mover o projeto `VideoProcessing.Finalizer.Lambda` da raiz para `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda`, preservando todo o conteúdo do projeto (arquivos .cs, .csproj, Properties, etc.).

## Passos de Implementação
1. Criar a estrutura de diretórios na raiz do repositório:
   - `src/Core/` (vazio — reservado para Domain/Application)
   - `src/Infra/` (vazio — reservado para projetos de infraestrutura)
   - `src/InterfacesExternas/` (destino do projeto Lambda)
2. Mover a pasta `VideoProcessing.Finalizer.Lambda/` inteira da raiz para `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/`, mantendo nome do projeto e todos os arquivos (Function.cs, .csproj, Properties/, Readme.md, aws-lambda-tools-defaults.json, etc.).
3. Verificar que não restam referências ao caminho antigo dentro do próprio projeto (paths relativos em .csproj, launchSettings, etc.); ajustar apenas se houver referências a caminhos fora do projeto.
4. Remover a pasta `VideoProcessing.Finalizer.Lambda` da raiz após confirmar que a cópia em `src/InterfacesExternas/` está completa e íntegra.

## Formas de Teste
1. **Inspeção de diretórios:** listar `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/` e confirmar presença de Function.cs, .csproj e demais arquivos.
2. **Build local:** executar `dotnet build` no caminho do projeto movido (`src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj`) e verificar que compila sem erros.
3. **Integração com solution (Subtask 02):** após atualizar o .slnx, validar que a solution carrega o projeto no novo caminho.

## Critérios de Aceite da Subtask
- [ ] Pastas `src/Core/`, `src/Infra/` e `src/InterfacesExternas/` existem na raiz do repositório
- [ ] Projeto Lambda está em `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/` com todos os arquivos originais preservados
- [ ] Não existe mais a pasta `VideoProcessing.Finalizer.Lambda` na raiz do repositório
- [ ] Build do projeto no novo caminho (`dotnet build src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj`) conclui com sucesso
