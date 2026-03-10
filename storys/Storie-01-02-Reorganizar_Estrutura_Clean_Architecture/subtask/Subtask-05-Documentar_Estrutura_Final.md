# Subtask 05: Documentar estrutura final e convenções

## Descrição
Documentar a estrutura de diretórios adotada após a reorganização, a convenção de pastas virtuais na solution e os limites da mudança (o que está in scope e out of scope), para que futuras alterações e onboarding sigam o mesmo padrão alinhado às rules do projeto.

## Passos de Implementação
1. Incluir no README.md (raiz) uma seção "Estrutura do repositório" ou "Estrutura do projeto" descrevendo:
   - `src/Core/` — reservado para projetos Domain e Application (Clean Architecture)
   - `src/Infra/` — reservado para projetos de infraestrutura
   - `src/InterfacesExternas/` — pontos de entrada (API, Lambda, handlers); citar o projeto `VideoProcessing.Finalizer.Lambda`
   - `tests/` — projetos de teste
2. Documentar que o diretório físico deve refletir o diretório virtual da solution e referenciar as rules do projeto (`.cursor/rules/core-clean-architecture.mdc`, `.cursor/documents/quick-reference.md`) para convenções de camadas.
3. Opcionalmente, criar ou atualizar um doc em `docs/` (ex.: `docs/estrutura-repositorio.md`) com um diagrama ou lista da árvore de pastas esperada e um resumo dos limites da Storie-01-02 (apenas reorganização; criação de projetos Domain/Application/Infra fica para stories futuras).
4. Garantir que a story técnica (Storie-01-02) e suas subtasks estejam referenciadas ou linkadas na documentação onde fizer sentido (ex.: histórico de mudanças ou seção "Estrutura").

## Formas de Teste
1. **Leitura:** um desenvolvedor consegue entender, apenas lendo o README e/ou docs, onde colocar um novo projeto (Core, Infra, InterfacesExternas, tests).
2. **Consistência:** verificar que os nomes das pastas e convenções descritas batem com o que está no .slnx e nas rules.
3. **Limites:** documentação deixa claro que esta story não criou projetos Domain/Application/Infra, apenas a estrutura de pastas e a movimentação da Lambda e dos testes.

## Critérios de Aceite da Subtask
- [ ] README.md contém seção descrevendo a estrutura de diretórios (src/Core, src/Infra, src/InterfacesExternas, tests) e o propósito de cada uma
- [ ] Está documentado que a solution segue a mesma organização (diretório virtual alinhado ao físico) e há referência às rules do projeto para Clean Architecture
- [ ] Limites da mudança da Storie-01-02 estão explícitos (reorganização; sem criação de novos projetos de camada)
- [ ] Documentação permite que um novo membro ou contribuidor entenda onde adicionar novos projetos no futuro
