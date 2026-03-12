# Subtask 01: Criar estrutura do projeto .NET 10 e handler básico

## Descrição
Criar a solution .NET 10 com o projeto da Lambda Finalizer, implementar o handler básico que retorna "Hello World from Finalizer" e configurar o projeto para deploy no AWS Lambda (runtime linux-x64).

## Passos de Implementação
1. Criar solution e projeto:
   - `dotnet new sln -n VideoProcessing.Finalizer`
   - `dotnet new classlib -n VideoProcessing.Finalizer -o src/VideoProcessing.Finalizer -f net10.0`
   - Adicionar projeto à solution
2. Criar classe `Function.cs` com método handler:
   - Implementar `FunctionHandler(string input, ILambdaContext context)`
   - Usar `ILambdaContext.Logger.LogInformation` para logar entrada e saída
   - Retornar objeto com `Message = "Hello World from Finalizer"`, `Input = input`, `RequestId = context.RequestId`
3. Configurar `.csproj`:
   - Adicionar PackageReference: `Amazon.Lambda.Core`, `Amazon.Lambda.Serialization.SystemTextJson`
   - Configurar assembly attribute `[assembly: LambdaSerializer(...)]` no topo do `Function.cs`
4. Criar projeto de testes:
   - `dotnet new xunit -n VideoProcessing.Finalizer.Tests -o test/VideoProcessing.Finalizer.Tests -f net10.0`
   - Adicionar PackageReference: `Amazon.Lambda.TestUtilities`
   - Adicionar referência ao projeto principal
5. Criar `README.md` básico documentando estrutura e como executar localmente

## Formas de Teste
1. **Teste local (build):**
   - Executar `dotnet build` e verificar compilação sem erros
   - Executar `dotnet test` (após criar testes na Subtask 04)
2. **Teste de estrutura:**
   - Verificar que `Function.cs` contém handler com assinatura correta
   - Confirmar que `LambdaSerializer` está configurado
3. **Inspeção de logs:**
   - Simular invocação local com `TestLambdaContext` (na Subtask 04)
   - Verificar que `ILambdaContext.Logger.LogInformation` é chamado

## Critérios de Aceite da Subtask
- [ ] Solution criada com projeto `VideoProcessing.Finalizer` em .NET 10
- [ ] Handler `FunctionHandler` implementado e compilando sem erros
- [ ] Pacotes `Amazon.Lambda.Core` e `Amazon.Lambda.Serialization.SystemTextJson` instalados
- [ ] `LambdaSerializer` configurado via assembly attribute
- [ ] Projeto de testes criado com referência ao projeto principal
- [ ] Build da solution executado com sucesso (`dotnet build`)
- [ ] README.md criado com seções: Estrutura, Requisitos, Como Executar, Testes
