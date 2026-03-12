# Subtask-03: Validar aws-lambda-tools-defaults e configurações do projeto

## Descrição
Revisar e ajustar o arquivo `aws-lambda-tools-defaults.json` do projeto Lambda para que as configurações de empacotamento (framework, arquitetura, função alvo) estejam alinhadas com o workflow de deploy, garantindo consistência entre o deploy local (via `dotnet lambda deploy`) e o pipeline de CI/CD.

## Passos de Implementação

1. **Revisar `aws-lambda-tools-defaults.json` atual:**
   - Verificar os campos: `function-handler`, `framework`, `configuration`, `function-architecture`.
   - Confirmar que `function-handler` é `VideoProcessing.Finalizer.Lambda::VideoProcessing.Finalizer.Lambda.Function::FunctionHandler`.

2. **Ajustar campos se necessário:**
   - `"framework"`: `"net10.0"`
   - `"configuration"`: `"Release"`
   - `"function-architecture"`: `"x86_64"` (padrão) ou `"arm64"` conforme configuração na AWS.
   - `"function-name"`: `"video-processing-engine-dev-finalizer"` (referência; o CI/CD usa a variável de repositório).

3. **Garantir que o `.csproj` mantém as propriedades necessárias:**
   - `<PublishReadyToRun>true</PublishReadyToRun>` — já presente; manter.
   - `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>` — já presente; manter.

## Formas de Teste

1. Executar `dotnet lambda package -c Release -pl src/InterfacesExternas/VideoProcessing.Finalizer.Lambda` localmente e confirmar que o zip é gerado sem erros.
2. Verificar no arquivo `.zip` gerado que os assemblies `VideoProcessing.Finalizer.Lambda.dll`, `Amazon.Lambda.Core.dll` e `Amazon.Lambda.Serialization.SystemTextJson.dll` estão presentes.
3. Confirmar no AWS Console que a arquitetura configurada na função Lambda (x86_64 ou arm64) bate com o `function-architecture` do `aws-lambda-tools-defaults.json`.

## Critérios de Aceite

- [ ] `aws-lambda-tools-defaults.json` contém `function-handler` correto e `framework` compatível com .NET 10.
- [ ] `dotnet lambda package` executa sem erros localmente e gera zip com os assemblies esperados.
- [ ] As propriedades `PublishReadyToRun` e `CopyLocalLockFileAssemblies` estão presentes no `.csproj` para garantir build correto para Lambda.
