using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Helpers;
using DataLibrary.HttpClients;
using DataLibrary.Settings;
using System.Net.Http.Headers;
using Serilog;
using OpenQA.Selenium.Support.UI;
using DataLibrary.Services.SDATScrapers;

namespace GroundRentProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        var logConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(logConfig)
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            Log.Information("App start");
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(Log.Logger);
            builder.Host.UseSerilog();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<IDataContext>(s => new DataContext(configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<IAddressDataServiceFactory, AddressDataServiceFactory>();
            builder.Services.AddScoped<IRealPropertySearchScraper, BaltimoreCityScraper>();
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
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            builder.Services.AddHttpClient("getUploadUri", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AccessTokenDelegatingHandler>();
            builder.Services.AddHttpClient("uploadPdf", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
            });
            builder.Services.AddHttpClient("extractPdf", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AccessTokenDelegatingHandler>();
            builder.Services.AddHttpClient("getDownloadStatus", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("x-api-key", pdfSettings.ClientId);
            }).AddHttpMessageHandler<AccessTokenDelegatingHandler>();
            builder.Services.AddHttpClient("downloadPdf", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
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

            app.UseSerilogRequestLogging();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "App start failure");
        }
        finally
        {
            Log.Information("App shut down complete");
            Log.CloseAndFlush();
        }
    }
}