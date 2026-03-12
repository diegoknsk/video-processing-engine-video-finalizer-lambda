# Subtask 05: Testes unitários — FrameRenamer e CreateZip com renomeação

## Descrição
Criar/atualizar testes unitários no projeto `VideoProcessing.Finalizer.Tests` para cobrir a lógica de renomeação: `FrameRenamer.TryParseTimeSeconds`, `FrameRenamer.GenerateRenamedEntries` e `FramesZipService.CreateZip` com `ordenaAutomaticamente = true/false`. Garantir cobertura ≥ 80% dos novos caminhos de código.

## Passos de Implementação
1. Criar classe `FrameRenamerTests` em `src/tests/VideoProcessing.Finalizer.Tests/`.
2. Adicionar testes para `TryParseTimeSeconds`:
   - `frame_0001_0s.jpg` → `true`, `seconds = 0`
   - `frame_0010_120s.png` → `true`, `seconds = 120`
   - `thumbnail.png` → `false`
   - `frame_0001.jpg` (sem sufixo de tempo) → `false`
3. Adicionar testes para `GenerateRenamedEntries`:
   - Múltiplos chunks com frames em ordens distintas → valida ordem por tempo e renomeação sequencial.
   - Mix de arquivos com e sem tempo → arquivos sem tempo aparecem com nome original.
   - Total > 9999 arquivos → padding ≥ 5 dígitos.
   - Empate de instante → desempate estável por nome original.
4. Adicionar testes para `CreateZip` com `ordenaAutomaticamente = true`:
   - Criar diretório temporário real com arquivos de frame simulados.
   - Verificar entradas do ZIP: sem barras (`/`) nos nomes das entradas, renomeadas corretamente.
5. Adicionar testes para `CreateZip` com `ordenaAutomaticamente = false`:
   - Verificar que estrutura relativa de diretórios foi preservada.
6. Executar `dotnet test` e confirmar que todos os testes passam.

## Formas de Teste
1. Executar `dotnet test` — todos os testes devem passar (0 falhas).
2. Verificar cobertura com `dotnet test --collect:"XPlat Code Coverage"` — novas classes/métodos ≥ 80%.
3. Revisar os nomes das entradas do ZIP nos testes de integração local para confirmar o comportamento esperado.

## Critérios de Aceite
- [ ] `FrameRenamerTests` com no mínimo 6 casos de teste cobrindo parse, ordenação, padding e fallback
- [ ] Testes de `CreateZip` com `ordenaAutomaticamente = true` verificam que entradas estão na raiz e renomeadas
- [ ] Testes de `CreateZip` com `ordenaAutomaticamente = false` verificam que comportamento original foi preservado
- [ ] `dotnet test` executa com 0 falhas e 0 erros
- [ ] Cobertura ≥ 80% nas novas classes/métodos adicionados nesta story
