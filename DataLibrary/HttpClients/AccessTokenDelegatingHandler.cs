using DataLibrary.Helpers;
using DataLibrary.Settings;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;

namespace DataLibrary.HttpClients;
public class AccessTokenDelegatingHandler : DelegatingHandler
{
    private AccessTokenInformation _accessToken;
    private IHttpClientFactory _httpClientFactory;
    private PDFServicesSettings _options;

    public AccessTokenDelegatingHandler(AccessTokenInformation accessToken, IOptions<PDFServicesSettings> options, IHttpClientFactory httpClientFactory)
    {
        this._accessToken = accessToken;
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _accessToken.AccessToken;
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
        var response = await base.SendAsync(request, cancellationToken);
        if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            var tokenResponse = await GetAccessToken();
            var tokenInfo = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await tokenResponse.Content.ReadAsStringAsync());
            token = tokenInfo.AccessToken;
            _accessToken.AccessToken = token;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private async Task<HttpResponseMessage> GetAccessToken()
    {
        var client = _httpClientFactory.CreateClient("jwt");
        var p = new Dictionary<string, string>();
        p.Add("jwt_token", await GenerateJWT());
        p.Add("client_id", this._options.ClientId);
        p.Add("client_secret", this._options.ClientSecret);
        var res = await client.PostAsync("https://ims-na1.adobelogin.com/ims/exchange/jwt", new FormUrlEncodedContent(p));
        return res;
    }

    private async Task<string> GenerateJWT()
    {
        var payload = new Dictionary<string, object>();
        payload.Add("exp", DateTimeOffset.Now.ToUnixTimeSeconds()+600);
        payload.Add("iss", _options.Issue);
        payload.Add("sub", _options.Sub);
        payload.Add("https://ims-na1.adobelogin.com/s/ent_documentcloud_sdk", true);
        payload.Add("aud", "https://ims-na1.adobelogin.com/c/" + _options.ClientId);

        RsaPrivateCrtKeyParameters keyPair;
        var key = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "\\wwwroot\\private.key");
        using (var sr = new StringReader(key))
        {
            PemReader pr = new PemReader(sr);
            keyPair = (RsaPrivateCrtKeyParameters)pr.ReadObject();
        }
        RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(keyPair);
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(rsaParams);
            return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
        }
    }
}
