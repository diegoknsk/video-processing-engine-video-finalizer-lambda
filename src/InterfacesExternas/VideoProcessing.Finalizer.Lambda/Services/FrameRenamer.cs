using System.Text.RegularExpressions;

namespace VideoProcessing.Finalizer.Lambda.Services;

/// <summary>
/// Helper para extrair instante de tempo do sufixo do nome do frame (*_Ns.ext) e gerar entradas renomeadas sequencialmente.
/// </summary>
internal static class FrameRenamer
{
    /// <summary>
    /// Regex para sufixo de tempo no nome do arquivo: _Ns.ext (ex.: _0s.jpg, _120s.png).
    /// Aplica-se ao nome do arquivo sem caminho.
    /// </summary>
    private static readonly Regex TimeSuffixRegex = new(@"_(\d+)s\.[^.]+$", RegexOptions.Compiled);

    /// <summary>
    /// Tenta extrair o instante em segundos do sufixo do nome do arquivo (ex.: frame_0001_20s.jpg → 20).
    /// </summary>
    /// <param name="fileName">Nome do arquivo com ou sem caminho; apenas o nome é considerado.</param>
    /// <param name="seconds">Segundos extraídos; -1 se não parseável.</param>
    /// <returns>True se o padrão *_Ns.ext foi encontrado.</returns>
    public static bool TryParseTimeSeconds(string fileName, out int seconds)
    {
        seconds = -1;
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        var name = Path.GetFileName(fileName);
        var match = TimeSuffixRegex.Match(name);
        if (!match.Success)
            return false;

        return int.TryParse(match.Groups[1].Value, out seconds);
    }

    /// <summary>
    /// Gera a lista de pares (caminho local, nome da entrada no ZIP): ordena por instante de tempo quando parseável,
    /// renumerando sequencialmente e preservando o sufixo de tempo; arquivos sem tempo mantêm o nome original.
    /// </summary>
    /// <param name="localFilePaths">Caminhos completos dos arquivos de frame no disco.</param>
    /// <returns>Lista de (localPath, entryName) para inclusão na raiz do ZIP.</returns>
    public static IReadOnlyList<(string LocalPath, string EntryName)> GenerateRenamedEntries(IReadOnlyList<string> localFilePaths)
    {
        if (localFilePaths is null || localFilePaths.Count == 0)
            return [];

        var withTime = new List<(string Path, int Seconds)>();
        var withoutTime = new List<string>();

        foreach (var path in localFilePaths)
        {
            if (TryParseTimeSeconds(path, out var sec))
                withTime.Add((path, sec));
            else
                withoutTime.Add(path);
        }

        // Ordenar por segundos crescente; desempate estável por nome e depois por caminho completo
        withTime.Sort((a, b) =>
        {
            var cmp = a.Seconds.CompareTo(b.Seconds);
            if (cmp != 0) return cmp;
            var nameCmp = StringComparer.OrdinalIgnoreCase.Compare(Path.GetFileName(a.Path), Path.GetFileName(b.Path));
            if (nameCmp != 0) return nameCmp;
            return StringComparer.Ordinal.Compare(a.Path, b.Path);
        });

        var totalCount = localFilePaths.Count;
        var paddingWidth = Math.Max(4, totalCount.ToString().Length);
        var result = new List<(string LocalPath, string EntryName)>(totalCount);

        var seq = 1;
        foreach (var (path, seconds) in withTime)
        {
            var ext = Path.GetExtension(path);
            var entryName = $"frame_{seq.ToString().PadLeft(paddingWidth, '0')}_{seconds}s{ext}";
            result.Add((path, entryName));
            seq++;
        }

        foreach (var path in withoutTime)
        {
            var entryName = Path.GetFileName(path);
            result.Add((path, entryName));
        }

        return result;
    }
}
