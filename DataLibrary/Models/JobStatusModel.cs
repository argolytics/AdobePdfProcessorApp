using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLibrary.Models;
public class JobStatusModel
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("content")]
    public AssetModel Content { get; set; }
    [JsonPropertyName("resource")]
    public AssetModel Resource { get; set; }
}
