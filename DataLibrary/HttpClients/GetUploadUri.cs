using DataLibrary.Helpers;
using DataLibrary.Models;
using System.Net.Http.Json;

namespace DataLibrary.HttpClients;
public class GetUploadUri
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    public const string ClientName = "getUploadUri";
    public GetUploadUri(IHttpClientFactory _factory, AccessTokenInformation accessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
    }
    public async Task<HttpResponseMessage> Upload()
    {
        HttpResponseMessage response = await UploadInternal();
        return response;
    }

    private async Task<HttpResponseMessage> UploadInternal()
    {
        var _httpClient = _factory.CreateClient(ClientName);
        MediaTypeModel mediaType = new()
        {
            mediaType = "application/pdf"
        };
        var response = await _httpClient.PostAsJsonAsync("https://pdf-services.adobe.io/assets", mediaType);
        return response;
    }
}
