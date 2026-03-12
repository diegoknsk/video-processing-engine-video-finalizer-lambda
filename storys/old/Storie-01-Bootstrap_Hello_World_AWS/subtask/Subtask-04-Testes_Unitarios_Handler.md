# Subtask 04: Criar testes unitários básicos do handler

## Descrição
Implementar testes unitários para o handler `FunctionHandler`, validando que ele processa corretamente diferentes payloads de entrada, utiliza o contexto Lambda para logging e retorna a estrutura de resposta esperada.

## Passos de Implementação
1. Criar classe de testes `FunctionTests.cs` no projeto `VideoProcessing.Finalizer.Tests`
2. Implementar testes:
   - **Teste 1:** `FunctionHandler_WithValidInput_ReturnsExpectedMessage`
     - Criar `TestLambdaContext` e payload de entrada
     - Invocar `FunctionHandler`
     - Verificar que resposta contém `Message = "Hello World from Finalizer"`
   - **Teste 2:** `FunctionHandler_WithNullInput_ReturnsMessageWithNullInput`
     - Invocar com `input = null`
     - Verificar que resposta contém `Input = null`
   - **Teste 3:** `FunctionHandler_WithContext_ReturnsRequestId`
     - Configurar `TestLambdaContext` com RequestId específico
     - Verificar que resposta contém `RequestId` correto
   - **Teste 4:** `FunctionHandler_LogsInputAndOutput`
     - Usar `TestLambdaLogger` do contexto
     - Verificar que logger registrou mensagens de entrada e saída
3. Configurar relatório de cobertura (opcional para esta story):
   - Adicionar pacote `coverlet.collector` ao projeto de testes
   - Executar `dotnet test --collect:"XPlat Code Coverage"`
4. Adicionar validação de cobertura no README.md (executar `dotnet test` e interpretar saída)

## Formas de Teste
1. **Execução local:**
   - Executar `dotnet test` e verificar que todos os testes passam
2. **Cobertura:**
   - Executar `dotnet test --collect:"XPlat Code Coverage"`
   - Verificar relatório de cobertura gerado (target mínimo 80%)
3. **Validação no CI:**
   - Confirmar que GitHub Actions executa testes e falha se algum teste não passar

## Critérios de Aceite da Subtask
- [ ] Classe `FunctionTests.cs` criada com mínimo 4 testes unitários
- [ ] Testes validam: mensagem de retorno, tratamento de input nulo, RequestId, logging
- [ ] Todos os testes passam localmente (`dotnet test`)
- [ ] Cobertura de código do handler ≥ 80%
- [ ] Pacote `Amazon.Lambda.TestUtilities` utilizado para `TestLambdaContext` e `TestLambdaLogger`
- [ ] Testes executam no workflow GitHub Actions e bloqueiam deploy se falharem
- [ ] README.md documentando como executar testes localmente e interpretar cobertura
