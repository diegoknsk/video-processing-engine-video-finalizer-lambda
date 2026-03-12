# Subtask-01: Refatorar workflow para usar dotnet lambda package e separar jobs

## Descrição
Reescrever o arquivo `.github/workflows/deploy-lambda.yml` separando os estágios em dois jobs independentes (`build-and-test` e `deploy`), substituindo o `dotnet publish + zip` manual pelo `dotnet lambda package` da Amazon.Lambda.Tools, que gera o pacote otimizado para execução no ambiente Lambda.

## Passos de Implementação

1. **Definir estrutura de dois jobs no workflow:**
   - Job `build-and-test`: checkout → setup .NET 10 → restore → build Release → `dotnet test`.
   - Job `deploy`: depende de `build-and-test` via `needs:`; só executa em `github.ref == 'refs/heads/main'`.

2. **Substituir dotnet publish + zip por dotnet lambda package:**
   - Instalar a ferramenta: `dotnet tool install -g Amazon.Lambda.Tools`.
   - Adicionar `export PATH="$PATH:$HOME/.dotnet/tools"` antes do uso.
   - Executar: `dotnet lambda package -o artifacts/VideoProcessing.Finalizer.zip -c Release -pl src/InterfacesExternas/VideoProcessing.Finalizer.Lambda`.

3. **Adicionar step de upload de código via AWS CLI:**
   - Usar `aws lambda update-function-code --function-name ... --zip-file fileb://artifacts/VideoProcessing.Finalizer.zip`.
   - Adicionar `aws lambda wait function-updated` após o update.

4. **Tornar o nome da função configurável:**
   - Usar `${{ vars.LAMBDA_FUNCTION_NAME || 'video-processing-engine-dev-finalizer' }}` no env do job `deploy`.

5. **Salvar zip como artefato do Actions** via `actions/upload-artifact@v4`.

## Formas de Teste

1. Fazer push em uma branch de teste e verificar nos logs do Actions que o job `build-and-test` passa antes do `deploy` ser iniciado.
2. Verificar no AWS Console (Lambda → Código) que a data de "Última modificação" avança após cada deploy via Actions.
3. Confirmar nos logs do Actions que o step "Package Lambda" gera o arquivo `.zip` sem erros e que o tamanho do artefato salvo é compatível com a aplicação.

## Critérios de Aceite

- [ ] O job `build-and-test` falha o pipeline quando `dotnet test` retorna erro — deploy não ocorre.
- [ ] O job `deploy` só é executado em push para `main`; em PRs apenas `build-and-test` roda.
- [ ] O zip gerado por `dotnet lambda package` é enviado com sucesso para o Lambda na AWS e o step de wait conclui sem timeout.
