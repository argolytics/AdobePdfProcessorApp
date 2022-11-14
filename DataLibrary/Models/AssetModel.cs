using System.Text.Json.Serialization;

namespace DataLibrary.Models;
public class AssetModel
{
    [JsonPropertyName("metadata")]
    public MetadataModel Metadata { get; set; }
    [JsonPropertyName("downloadUri")]
    public string DownloadUri { get; set; }
    [JsonPropertyName("assetID")]
    public string AssetID { get; set; }
}
