using System.Text.Json.Serialization;
using DataLibrary.JsonConverters;

namespace DataLibrary.Models;
public class ExtractPdfModel
{
    [JsonPropertyName("assetID")]
    public string? AssetId { get; set; }

    [JsonPropertyName("renditionsToExtract")]
    public string[]? RenditionsToExtract { get; set; }

    [JsonPropertyName("elementsToExtract")]
    public string[]? ElementsToExtract { get; set; }

    [JsonPropertyName("tableOutputFormat")]
    public string? TableOutputFormat { get; set; }
}
