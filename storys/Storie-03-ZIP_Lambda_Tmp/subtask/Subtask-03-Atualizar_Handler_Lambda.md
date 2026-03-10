# Subtask 03: Atualizar handler para processar evento e gerar ZIP em /tmp

## Descrição
Atualizar o handler `FunctionHandler` para processar o novo evento `FinalizerEvent`, salvar os arquivos recebidos em `/tmp`, gerar o ZIP usando o `ZipService`, e retornar `FinalizerResponse` com informações do processamento.

## Passos de Implementação
1. Atualizar `Function.cs`:
   - Modificar assinatura do handler: `FunctionHandler(FinalizerEvent input, ILambdaContext context)`
   - Injetar dependências via construtor (ou resolver manualmente nesta fase):
     - `ITempStorageService`
     - `IZipService`
     - `ILogger<Function>`
   - Se injeção de dependência não estiver configurada, instanciar serviços manualmente (usar `NullLogger` ou `ConsoleLogger`)
2. Implementar fluxo do handler:
   - **Passo 1:** Logar recebimento do evento: "Received finalizer event for video {VideoId}, files: {FileCount}"
   - **Passo 2:** Criar diretório de trabalho em `/tmp` usando `TempStorageService.CreateWorkingDirectoryAsync`
   - **Passo 3:** Iterar sobre `input.Files` e processar cada arquivo:
     - Decodificar `ContentBase64` para `byte[]` usando `Convert.FromBase64String`
     - Escrever arquivo em `/tmp` usando `TempStorageService.WriteFileAsync`
   - **Passo 4:** Gerar ZIP usando `ZipService.CreateZipFromDirectoryAsync`:
     - Caminho de origem: diretório criado em `/tmp`
     - Caminho de saída: `/tmp/finalizer/{videoId}/output.zip`
   - **Passo 5:** Obter tamanho do ZIP usando `TempStorageService.GetFileSize`
   - **Passo 6:** Limpar arquivos temporários (opcional nesta fase, obrigatório na Storie-04)
   - **Passo 7:** Retornar `FinalizerResponse.Success` com informações
3. Implementar tratamento de exceções:
   - Envolver todo o fluxo em `try/catch`
   - Logar exceções e retornar `FinalizerResponse.Failure`
4. Implementar limpeza de recursos:
   - Usar `try/finally` para garantir que diretório temporário seja limpo mesmo em caso de erro

## Formas de Teste
1. **Teste local com harness:**
   - Criar harness simulando evento `FinalizerEvent` com 1-3 arquivos pequenos
   - Executar handler localmente e verificar que ZIP é gerado em `/tmp` (ou diretório temporário local)
2. **Teste unitário:**
   - Mockar `ITempStorageService` e `IZipService`
   - Verificar que fluxo completo é executado na ordem correta
3. **Teste no Lambda (Subtask 05):**
   - Invocar Lambda com payload de teste e verificar resposta

## Critérios de Aceite da Subtask
- [ ] Handler `FunctionHandler` atualizado para receber `FinalizerEvent` e retornar `FinalizerResponse`
- [ ] Dependências `ITempStorageService` e `IZipService` injetadas ou instanciadas manualmente
- [ ] Fluxo completo implementado: criar diretório, escrever arquivos, gerar ZIP, obter tamanho, limpar
- [ ] Arquivos do evento decodificados de base64 e salvos em `/tmp`
- [ ] ZIP gerado em `/tmp/finalizer/{videoId}/output.zip`
- [ ] Logging estruturado implementado em cada etapa do fluxo
- [ ] Tratamento de exceções implementado retornando `FinalizerResponse.Failure` em caso de erro
- [ ] Limpeza de recursos implementada em bloco `finally`
- [ ] Build da solution executado com sucesso
