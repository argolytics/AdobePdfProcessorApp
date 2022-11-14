using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Helpers;
using DataLibrary.HttpClients;
using DataLibrary.Settings;
using System.Net.Http.Headers;

namespace GroundRentProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<IDataContext>(s => new DataContext(configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IAddressDataServiceFactory, AddressDataServiceFactory>();
        builder.Services.AddScoped<AccessTokenInformation>();
        var pdfSettings = new PDFServicesSettings();
        configuration.GetSection("PDFServices").Bind(pdfSettings);
        builder.Services.Configure<PDFServicesSettings>(opt =>
        {
            opt.ClientId = pdfSettings.ClientId;
            opt.ClientSecret = pdfSettings.ClientSecret;
            opt.Sub = pdfSettings.Sub;
            opt.Issue = pdfSettings.Issue;
        });

        // Http clients
        builder.Services.AddScoped<GetUploadUri>();
        builder.Services.AddScoped<UploadPdf>();
        builder.Services.AddScoped<ExtractPdf>();
        builder.Services.AddScoped<GetDownloadStatus>();
        builder.Services.AddScoped<DownloadPdf>();
        builder.Services.AddTransient<AccessTokenDelegatingHandler>();
        builder.Services.AddHttpClient("jwt", client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        builder.Services.AddHttpClient("getUploadUri", client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<AccessTokenDelegatingHandler>();
        builder.Services.AddHttpClient("uploadPdf", client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
        });
        builder.Services.AddHttpClient("extractPdf", client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<AccessTokenDelegatingHandler>();
        builder.Services.AddHttpClient("getDownloadStatus", client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
        }).AddHttpMessageHandler<AccessTokenDelegatingHandler>();
        builder.Services.AddHttpClient("downloadPdf", client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}