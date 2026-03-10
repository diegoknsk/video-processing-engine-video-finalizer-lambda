# Subtask 03: Criar harness local para teste manual

## Descrição
Criar um harness (programa de teste local) que permite executar a criação de ZIP a partir de um diretório de imagens de teste, facilitando a validação manual da funcionalidade antes da integração com AWS.

## Passos de Implementação
1. Criar classe `LocalZipHarness.cs` em `test/VideoProcessing.Finalizer.Tests/Harness/`
2. Implementar método `Main` ou método de teste marcado com `[Fact(Skip = "Manual test")]`:
   - Criar diretório de teste temporário com imagens de amostra (usar `Path.GetTempPath()`)
   - Copiar ou gerar arquivos de imagem de teste (ex.: arquivos vazios simulando imagens)
   - Instanciar `ZipService` com logger configurado (usar `ConsoleLogger` ou `NullLogger`)
   - Invocar `CreateZipFromDirectoryAsync` com diretório de teste
   - Logar resultado no console: sucesso, quantidade de arquivos, tamanho do ZIP
   - Verificar existência do arquivo ZIP gerado
   - Abrir ZIP e listar arquivos (opcional: usar `ZipArchive` para validar conteúdo)
3. Adicionar limpeza de recursos:
   - Deletar diretório de teste e ZIP gerado após execução
   - Usar `try/finally` para garantir limpeza
4. Documentar no README.md:
   - Como executar o harness local
   - Estrutura do diretório de teste esperado
   - Como validar manualmente o ZIP gerado

## Formas de Teste
1. **Execução manual:**
   - Executar harness via IDE ou `dotnet run` (se for console app)
   - Ou executar teste com atributo `[Fact(Skip = ...)]` removendo o Skip temporariamente
2. **Validação de ZIP:**
   - Abrir arquivo ZIP gerado em ferramenta de descompactação (7-Zip, WinRAR, etc.)
   - Verificar que todos os arquivos de teste estão presentes
3. **Validação de logs:**
   - Verificar que console mostra logs estruturados: início, quantidade de arquivos, sucesso, tamanho

## Critérios de Aceite da Subtask
- [ ] Classe `LocalZipHarness` criada em `test/VideoProcessing.Finalizer.Tests/Harness/`
- [ ] Harness cria diretório de teste temporário com arquivos de amostra
- [ ] Harness instancia `ZipService` e invoca `CreateZipFromDirectoryAsync`
- [ ] Resultado da operação é logado no console com informações detalhadas
- [ ] ZIP gerado pode ser aberto e validado manualmente (contém arquivos esperados)
- [ ] Limpeza de recursos implementada (diretório e ZIP deletados após execução)
- [ ] README.md atualizado com instruções de como executar o harness local
- [ ] Harness pode ser executado de forma independente sem interação com AWS
