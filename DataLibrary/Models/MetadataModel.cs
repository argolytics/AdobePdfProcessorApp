using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLibrary.Models;
public class MetadataModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("size")]
    public int Size { get; set; }
}
