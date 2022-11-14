using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLibrary.Helpers;
public class AccessTokenInformation
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}
