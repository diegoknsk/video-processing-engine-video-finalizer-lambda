# Storie-05: Retornar Bucket e Chave S3 do ZIP no Resultado da Lambda

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Como consumidor do resultado da Lambda (ex.: Step Functions ou SQS downstream), quero que o retorno inclua o bucket de saída junto à chave S3 do ZIP gerado, para que eu possa localizar e baixar o arquivo sem precisar conhecer a topologia de buckets de antemão.

## Objetivo
Adicionar o campo `ZipBucket` ao `FinalizerResult`, populá-lo com o `OutputBucket` do input e atualizar todos os testes que constroem ou verificam esse record.

## Escopo Técnico
- Tecnologias: .NET 10, C# 13, AWS Lambda
- Arquivos afetados:
  - `src/InterfacesExternas/VideoProcessing.Finalizer.Lambda/Function.cs`
  - `src/tests/VideoProcessing.Finalizer.Tests/FunctionTests.cs`
- Componentes/Recursos:
  - `FinalizerResult` — novo campo `ZipBucket` (string)
  - `Function.FunctionHandler` — passar `outputBucket` ao construir o resultado
- Pacotes/Dependências:
  - Nenhum pacote externo adicional

## Dependências e Riscos (para estimativa)
- Dependências: Stories 01, 02 e 04 devem estar concluídas (contrato de entrada e pipeline S3→ZIP).
- Riscos/Pré-condições:
  - `FinalizerResult` é um `record` público; qualquer consumidor externo (Step Functions, testes de integração) precisará atualizar o parsing do novo campo.
  - A mudança é aditiva (novo campo), portanto consumidores que ignoram campos extras não serão quebrados.

## Subtasks
- [ ] [Subtask 01: Adicionar campo ZipBucket ao FinalizerResult](./subtask/Subtask-01-Adicionar_ZipBucket_FinalizerResult.md)
- [ ] [Subtask 02: Propagar OutputBucket na construção do resultado em FunctionHandler](./subtask/Subtask-02-Propagar_OutputBucket_FunctionHandler.md)
- [ ] [Subtask 03: Atualizar testes unitários para o novo campo ZipBucket](./subtask/Subtask-03-Atualizar_Testes_Unitarios_ZipBucket.md)

## Critérios de Aceite da História
- [ ] O `FinalizerResult` possui o campo `ZipBucket` (string não nula) além dos campos já existentes (`ZipS3Key`, `FramesCount`)
- [ ] Ao executar a Lambda com sucesso, o JSON de resposta contém `"ZipBucket"` com o valor do `outputBucket` informado no input
- [ ] `ZipS3Key` e `FramesCount` continuam presentes e com valores corretos no resultado
- [ ] Todos os testes unitários existentes que referenciam `FinalizerResult` são atualizados e passam sem erro
- [ ] Novos testes unitários verificam que `ZipBucket` é igual ao `OutputBucket` do input após execução bem-sucedida
- [ ] `dotnet test` passa com cobertura ≥ 80% nas classes modificadas

## Rastreamento (dev tracking)
- **Início:** 13/03/2026, às 03:30 (Brasília)
- **Fim:** —
- **Tempo total de desenvolvimento:** —
