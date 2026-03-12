# Subtask 03: Compactar frames em ZIP único

## Descrição
Implementar a etapa de compactação: após todos os frames estarem baixados em `/tmp/frames/`, criar um único arquivo ZIP em `/tmp/<jobId>_frames.zip` contendo todas as imagens (de todos os chunks), usando `ZipFile.CreateFromDirectory` com `CompressionLevel.Fastest` (imagens JPG/PNG já são comprimidas — ganho real é mínimo, velocidade é prioridade).

## Passos de Implementação
1. No `FramesZipService`, implementar `CreateZipAsync(string framesDir, string zipPath)` que valida se `framesDir` existe e tem arquivos, em seguida chama `ZipFile.CreateFromDirectory(framesDir, zipPath, CompressionLevel.Fastest, includeBaseDirectory: false)`.
2. Derivar o `jobId` a partir do último segmento do `framesBasePrefix` (ex.: para `processed/uuid1/uuid2`, o jobId seria `uuid2`) para nomear o arquivo ZIP de forma determinística e rastreável.
3. Certificar que todos os `FileStream` abertos durante o download (subtask anterior) estão fechados antes de chamar `CreateZipAsync`, garantindo que o ZIP não seja corrompido por handles abertos.

## Formas de Teste
1. **Unitário:** criar um diretório temporário com 3 arquivos `.jpg` fake, chamar `CreateZipAsync` e verificar que o ZIP gerado existe e contém exatamente 3 entradas.
2. **Unitário:** testar com `framesDir` vazio (sem arquivos) e verificar que exceção adequada é lançada antes de tentar criar o ZIP.
3. **Unitário:** verificar que o nome do ZIP é derivado corretamente do último segmento de `framesBasePrefix`.

## Critérios de Aceite
- [x] ZIP é criado em `/tmp/<jobId>_frames.zip` com `CompressionLevel.Fastest`
- [x] ZIP contém todos os arquivos dos subdiretórios de chunks sem pasta raiz (`includeBaseDirectory: false`)
- [x] Exceção é lançada e logada se o diretório de frames estiver vazio ao tentar compactar
- [x] Nome do arquivo ZIP é derivado deterministicamente do `framesBasePrefix`
