# Subtask 04: Validar build, testes e deploy

## Descrição
Validar que, após a reorganização, o repositório compila, os testes passam e o pipeline de deploy (GitHub Actions) executa com sucesso, incluindo a atualização da função Lambda na AWS quando aplicável.

## Passos de Implementação
1. Na raiz do repositório, executar `dotnet restore`, `dotnet build --configuration Release` e `dotnet test --configuration Release` e registrar que não há erros nem warnings que impeçam o build ou os testes.
2. Executar o passo de publish usado no workflow: `dotnet publish src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/VideoProcessing.Finalizer.Lambda.csproj --configuration Release --runtime linux-x64 --self-contained false --output ./publish` e verificar que a pasta `./publish` é gerada com os artefatos esperados (DLL da Lambda, dependências, etc.).
3. Disparar o workflow de deploy (push para branch configurada ou workflow_dispatch) e verificar que o job "build-and-deploy" conclui com sucesso; em ambiente de staging/dev, confirmar que a função Lambda é atualizada e que uma invocação de teste retorna o comportamento esperado (conforme Storie-01).
4. Documentar no README ou em docs qualquer comando necessário para validação pós-reorganização (ex.: como rodar testes, como publicar localmente).

## Formas de Teste
1. **Build e testes locais:** `dotnet build` e `dotnet test` na raiz; saída sem falhas.
2. **Publish local:** comando de publish do workflow com novo caminho; inspeção da pasta `./publish` para presença do assembly principal e de dependências.
3. **CI/CD:** execução do workflow no GitHub Actions; logs verdes e, se push para main, deploy da Lambda concluído e função invocável.

## Critérios de Aceite da Subtask
- [ ] `dotnet build` e `dotnet test` na raiz do repositório passam sem erros
- [ ] Comando `dotnet publish` do workflow executado localmente com o novo caminho gera corretamente a pasta `./publish`
- [ ] Pipeline GitHub Actions (build-and-deploy) executa com sucesso; em caso de deploy (push para branch configurada), a função Lambda na AWS é atualizada e responde à invocação conforme esperado
- [ ] Nenhum passo do workflow falha por caminho de projeto incorreto ou projeto não encontrado
