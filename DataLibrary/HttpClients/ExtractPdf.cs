using DataLibrary.Helpers;
using DataLibrary.Models;
using System.Net.Http.Json;

namespace DataLibrary.HttpClients;
public class ExtractPdf
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    public const string ClientName = "extractPdf";
    public ExtractPdf(IHttpClientFactory _factory, AccessTokenInformation accessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
    }
    public async Task<HttpResponseMessage> Extract(FileModel file)
    {
        HttpResponseMessage response = await ExtractInternal(file);
        return response;
    }

    private async Task<HttpResponseMessage> ExtractInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        ExtractPdfModel extractPdfBody = new()
        {
            AssetId = file.AssetId,
            RenditionsToExtract = new string[] { "tables" },
            ElementsToExtract = new string[] { "text", "tables" },
            TableOutputFormat = "csv",
        };
        var response = await _httpClient.PostAsJsonAsync("https://pdf-services.adobe.io/operation/extractpdf", extractPdfBody);
        return response;
    }
}
