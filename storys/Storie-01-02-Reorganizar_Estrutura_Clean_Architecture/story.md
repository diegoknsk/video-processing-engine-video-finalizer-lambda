# Storie-01-02: Reorganizar estrutura de diretórios (Clean Architecture)

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** —

## Descrição
Como desenvolvedor do sistema Video Processing Engine, quero reorganizar a estrutura física de diretórios do repositório para refletir a Clean Architecture e as convenções do projeto, para que a localização dos projetos (Core, Infra, InterfacesExternas, Tests) fique explícita e o diretório virtual da solution esteja alinhado ao diretório físico.

## Objetivo
Mover o projeto Lambda da raiz para `src/InterfacesExternas/`, criar a estrutura de pastas alinhada à Clean Architecture (~70%), garantir que o arquivo de solution use caminhos físicos corretos e pastas virtuais (src/Core, src/Infra, src/InterfacesExternas, tests), e assegurar que build, testes e deploy continuem funcionando após a reorganização.

## Escopo Técnico
- Tecnologias: .NET 10, estrutura de pastas, arquivo de solution (.slnx), GitHub Actions
- Estrutura alvo:
  - `src/Core/` — reservado para projetos Domain e Application (vazio nesta story, apenas pasta criada)
  - `src/Infra/` — reservado para projetos de infraestrutura (vazio nesta story, apenas pasta criada)
  - `src/InterfacesExternas/` — projeto Lambda: `VideoProcessing.Finalizer.Lambda`
  - `tests/` — projeto de testes: `VideoProcessing.Finalizer.Tests`
- Arquivos afetados:
  - Movimento físico: `VideoProcessing.Finalizer.Lambda/` → `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/`
  - Movimento físico: `test/` → `tests/` (projeto de testes)
  - `VideoProcessing.Finalizer.slnx` — caminhos dos projetos e pastas virtuais
  - `.github/workflows/deploy-lambda.yml` — caminho do projeto a publicar
  - `README.md`, `docs/` e referências a caminhos antigos
- Componentes: nenhum novo componente de código; apenas reorganização de diretórios e referências.
- Pacotes/Dependências: nenhum pacote novo; versões existentes mantidas.

## Dependências e Riscos (para estimativa)
- Dependências: Nenhuma outra story obrigatória; recomenda-se que não haja branches com alterações pesadas em caminhos de projeto em paralelo.
- Riscos:
  - Referências quebradas: qualquer referência absoluta ou relativa a `VideoProcessing.Finalizer.Lambda` ou `test/` em scripts, docs ou CI deve ser atualizada.
  - Solution: formato .slnx exige caminhos relativos corretos; pastas virtuais devem refletir a estrutura arquitetural.
- Pré-condições:
  - Repositório com Lambda e testes funcionando (build e deploy atuais conhecidos).
  - Decisão de renomear `test/` para `tests/` (convenção adotada nas rules).

## Limites da mudança
- **In scope:** mover Lambda para `src/InterfacesExternas/`, mover testes para `tests/`, criar pastas `src/Core/` e `src/Infra/` (vazias), atualizar solution e CI/docs.
- **Out of scope:** criar novos projetos (Domain, Application, Infra); refatorar código interno da Lambda; alterar comportamento funcional ou contratos de API.

## Subtasks
- [Subtask 01: Criar estrutura de pastas e mover projeto Lambda](./subtask/Subtask-01-Estrutura_Pastas_Mover_Lambda.md)
- [Subtask 02: Mover projeto de testes e atualizar solution](./subtask/Subtask-02-Mover_Testes_Atualizar_Solution.md)
- [Subtask 03: Atualizar GitHub Actions e referências de caminho](./subtask/Subtask-03-Atualizar_CI_Referencias.md)
- [Subtask 04: Validar build, testes e deploy](./subtask/Subtask-04-Validar_Build_Testes_Deploy.md)
- [Subtask 05: Documentar estrutura final e convenções](./subtask/Subtask-05-Documentar_Estrutura_Final.md)

## Critérios de Aceite da História
- [ ] Estrutura de pastas criada: `src/Core/`, `src/Infra/`, `src/InterfacesExternas/`, `tests/`
- [ ] Projeto Lambda localizado em `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/` (não mais na raiz)
- [ ] Projeto de testes localizado em `tests/VideoProcessing.Finalizer.Tests/`
- [ ] Arquivo de solution (.slnx) com caminhos físicos corretos e pastas virtuais refletindo Core, Infra, InterfacesExternas e tests
- [ ] `dotnet build` e `dotnet test` executados na raiz do repositório passam sem erros
- [ ] Pipeline de deploy (GitHub Actions) publica a partir do novo caminho e atualiza a Lambda na AWS com sucesso
- [ ] README ou documentação atualizada descrevendo a nova estrutura e convenção de pastas
- [ ] Nenhuma referência quebrada a caminhos antigos (Lambda na raiz, pasta `test/`) em scripts, workflows ou documentação

## Rastreamento (dev tracking)
- **Início:** 11/03/2026, às 08:40 (Brasília)
- **Fim:** —
- **Tempo total de desenvolvimento:** —
