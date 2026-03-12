using System.Text.Json.Serialization;

namespace VideoProcessing.Finalizer.Lambda.Models;

/// <summary>
/// Contrato de entrada da Lambda: bucket de origem dos frames, prefixo base e bucket de destino do ZIP.
/// Compatível com invocação direta (Step Functions) e com body de mensagem SQS.
/// </summary>
public sealed class FinalizerInput
{
    [JsonPropertyName("framesBucket")]
    public string? FramesBucket { get; set; }

    [JsonPropertyName("framesBasePrefix")]
    public string? FramesBasePrefix { get; set; }

    [JsonPropertyName("outputBucket")]
    public string? OutputBucket { get; set; }

    [JsonPropertyName("videoId")]
    public string? VideoId { get; set; }

    [JsonPropertyName("outputBasePrefix")]
    public string? OutputBasePrefix { get; set; }

    /// <summary>
    /// Quando true (padrão), os frames no ZIP são renomeados sequencialmente e ordenados por instante de tempo, na raiz do ZIP.
    /// </summary>
    [JsonPropertyName("ordenaAutomaticamente")]
    public bool OrdenaAutomaticamente { get; set; } = true;
}
