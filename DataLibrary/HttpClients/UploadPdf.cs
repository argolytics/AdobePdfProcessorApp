using DataLibrary.Helpers;
using DataLibrary.Models;
using System.Reflection.Metadata.Ecma335;

namespace DataLibrary.HttpClients;
public class UploadPdf
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    public const string ClientName = "uploadPdf";
    public UploadPdf(IHttpClientFactory _factory, AccessTokenInformation accessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
    }
    public async Task<HttpResponseMessage> Upload(FileModel file)
    {
        HttpResponseMessage response = await UploadInternal(file);

        return response;
    }

    private async Task<HttpResponseMessage> UploadInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        byte[] fileData = File.ReadAllBytes(file.UploadPath);
        HttpContent content = new ByteArrayContent(fileData);
        content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/pdf");
        //try
        //{
        //    var response = await _httpClient.PutAsync(file.UploadUri, content);
        //    return response;
        //}
        //catch (IOException)
        //{
        //    Console.WriteLine("IOException");
        //    return null;
        //}
        var response = await _httpClient.PutAsync(file.UploadUri, content);
        return response;
    }
}
