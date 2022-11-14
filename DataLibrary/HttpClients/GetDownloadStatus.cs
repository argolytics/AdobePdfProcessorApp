using DataLibrary.Helpers;
using DataLibrary.Models;

namespace DataLibrary.HttpClients;

public class GetDownloadStatus
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    public const string ClientName = "getDownloadStatus";
    public GetDownloadStatus(IHttpClientFactory _factory, AccessTokenInformation accessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
    }
    public async Task<HttpResponseMessage> GetStatus(FileModel file)
    {
        HttpResponseMessage response = await GetStatusInternal(file);
        return response;
    }

    private async Task<HttpResponseMessage> GetStatusInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);

        var response = await _httpClient.GetAsync(file.DownloadCheckLocation);
        return response;
    }
}