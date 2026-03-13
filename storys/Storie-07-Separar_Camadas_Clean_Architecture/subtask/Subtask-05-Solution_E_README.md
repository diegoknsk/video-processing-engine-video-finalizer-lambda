# Subtask-05: Criar solution .slnx e atualizar README

## Descrição
Criar o arquivo `VideoProcessing.Finalizer.slnx` na raiz do repositório com os 5 projetos organizados em pastas virtuais (Core, Infra, InterfacesExternas, tests) e atualizar o README com a nova estrutura de camadas, arquitetura limpa e guia completo de variáveis/configurações.

## Passos de implementação
1. Criar `VideoProcessing.Finalizer.slnx` na raiz com `<Solution>` e `<Folder>` para Core (Domain, Application), Infra, InterfacesExternas (Lambda) e tests (Tests).
2. Atualizar a seção "Estrutura do Projeto" do README com a nova árvore de diretórios refletindo as camadas.
3. Atualizar a seção "Estrutura do repositório" descrevendo a responsabilidade de cada camada (Domain, Application, Infra, Lambda).
4. Atualizar ou criar seção "Variáveis de Ambiente e Configuração" documentando as variáveis necessárias para execução local e no Lambda.
5. Corrigir trechos desatualizados (ex.: handler ainda descrito como ToUpper, exemplos de payload antigos).

## Formas de teste
1. Abrir `VideoProcessing.Finalizer.slnx` no Visual Studio / Rider — todos os 5 projetos devem aparecer organizados nas pastas virtuais corretas.
2. `dotnet build VideoProcessing.Finalizer.slnx` compila toda a solution sem erros.
3. Revisar o README: nenhuma seção deve referenciar a estrutura antiga de um único projeto.

## Critérios de aceite
- [ ] `VideoProcessing.Finalizer.slnx` existe na raiz e contém todos os 5 projetos.
- [ ] `dotnet build VideoProcessing.Finalizer.slnx` passa sem erros.
- [ ] README atualizado com estrutura, arquitetura por camadas, payload de invocação real e guia de variáveis/secrets.
