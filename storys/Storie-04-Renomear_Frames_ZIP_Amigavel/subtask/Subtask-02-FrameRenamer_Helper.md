# Subtask 02: Implementar helper FrameRenamer (parse de tempo e geração de nomes sequenciais)

## Descrição
Criar a classe estática interna `FrameRenamer` (ou método(s) no `FramesZipService`) responsável por: (a) extrair o instante de tempo em segundos do sufixo do nome do arquivo (`*_<N>s.<ext>`); (b) ordenar uma lista de caminhos de arquivo por instante crescente com desempate estável; (c) gerar o nome final sequencial preservando o sufixo de tempo e a extensão original, com padding numérico dinâmico.

## Passos de Implementação
1. Criar `FrameRenamer` como classe `internal static` dentro do namespace `VideoProcessing.Finalizer.Lambda.Services`.
2. Implementar `TryParseTimeSeconds(string fileName, out int seconds)`:
   - Regex: `_(\d+)s\.[^.]+$` no nome do arquivo (sem o caminho).
   - Retorna `false` se não encontrar; `out seconds = -1`.
3. Implementar `GenerateRenamedEntries(IReadOnlyList<string> localFilePaths)`:
   - Particionar em: arquivos com tempo parseável vs. sem tempo.
   - Ordenar a partição parseável por `seconds` crescente, com desempate por nome original (`StringComparer.OrdinalIgnoreCase`).
   - Calcular `paddingWidth = Math.Max(4, totalCount.ToString().Length)`.
   - Para cada arquivo ordenado: nome final = `frame_{seq.ToString().PadLeft(paddingWidth, '0')}_{seconds}s{ext}`.
   - Para arquivos sem tempo: manter `Path.GetFileName(localPath)` como nome de destino.
   - Retornar `IReadOnlyList<(string localPath, string entryName)>`.

## Formas de Teste
1. Teste unitário: lista com arquivos de múltiplos chunks (`chunk1/frames/frame_0001_0s.jpg`, `chunk2/frames/frame_0001_20s.jpg`, `chunk1/frames/frame_0002_5s.jpg`) → verifica ordenação por tempo e nomes gerados.
2. Teste unitário: arquivo sem sufixo de tempo (`thumbnail.png`) → mantém nome original, sem lançar exceção.
3. Teste unitário: 10001 arquivos → verifica padding de 5 dígitos no nome gerado.
4. Teste unitário: dois arquivos com mesmo instante de tempo → verifica desempate estável por nome.

## Critérios de Aceite
- [ ] `TryParseTimeSeconds` retorna `true` e o valor correto para nomes no padrão `*_<N>s.<ext>`
- [ ] `TryParseTimeSeconds` retorna `false` sem exceção para nomes fora do padrão
- [ ] `GenerateRenamedEntries` ordena por instante crescente e numera sequencialmente com padding correto
- [ ] Arquivos sem tempo parseable são incluídos com nome original no resultado
- [ ] Padding é dinâmico: mínimo 4 dígitos, ampliado conforme total de frames
