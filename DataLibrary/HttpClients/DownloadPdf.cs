using DataLibrary.Helpers;
using DataLibrary.Models;

namespace DataLibrary.HttpClients;

public class DownloadPdf
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    public const string ClientName = "downloadPdf";
    public DownloadPdf(IHttpClientFactory _factory, AccessTokenInformation accessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
    }
    public async Task<HttpResponseMessage> Download(FileModel file)
    {
        HttpResponseMessage response = await DownloadInternal(file);
        return response;
    }

    private async Task<HttpResponseMessage> DownloadInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);

        var response = await _httpClient.GetAsync(file.DownloadUri);
        return response;
    }
}
