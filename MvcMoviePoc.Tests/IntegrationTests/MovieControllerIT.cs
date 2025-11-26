using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcMoviePoc.Data;
using MvcMoviePoc.Models;
using System.Net;

namespace MvcMoviePoc.Tests.IntegrationTests;

// DB-backed integration test for MoviesController using real SQL Server (local or CI)
// Assumes connection string provided via environment variable or appsettings.Test.json.
public class MovieControllerIT : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MovieControllerIT(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((ctx, config) =>
            {
                var settings = new Dictionary<string, string?>();
                string? cs = Environment.GetEnvironmentVariable("MOVIE_CS");
                if (!string.IsNullOrWhiteSpace(cs))
                {
                    settings["ConnectionStrings:MvcMoviePocContext"] = cs;
                }
                config.AddInMemoryCollection(settings!);
            });
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MvcMoviePocContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                SeedData.Initialize(scope.ServiceProvider);
            });
        });
    }

    [Fact]
    public async Task Get_Index_ReturnsSeededMovies()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/Movies");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Ghostbusters", html);
        Assert.Contains("Rio Bravo", html);
    }

    // TODO: Enable when Create view is available
    //[Fact]
    //public async Task Post_Create_Then_Get_Details_ShowsNewMovie()
    //{
    //    var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = true });

    //    // GET form to obtain antiforgery token & cookie
    //    var getCreate = await client.GetAsync("/Movies/Create");
    //    Assert.Equal(HttpStatusCode.OK, getCreate.StatusCode);
    //    string createHtml = await getCreate.Content.ReadAsStringAsync();
    //    var tokenMatch = Regex.Match(createHtml, "__RequestVerificationToken[^>]*value=\"(?<val>[^\"]+)\"");
    //    Assert.True(tokenMatch.Success, "Antiforgery token not found in form HTML. Snippet: " + createHtml.Substring(0, Math.Min(createHtml.Length, 400)));
    //    string antiToken = tokenMatch.Groups["val"].Value;

    //    var form = new Dictionary<string, string?>
    //    {
    //        ["__RequestVerificationToken"] = antiToken,
    //        ["Title"] = "IT Test Movie",
    //        ["ReleaseDate"] = DateTime.Today.ToString("yyyy-MM-dd"),
    //        ["Genre"] = "Action",
    //        ["Rating"] = "PG",
    //        ["Price"] = 12.34M.ToString(System.Globalization.CultureInfo.InvariantCulture)
    //    };

    //    var post = await client.PostAsync("/Movies/Create", new FormUrlEncodedContent(form!));
    //    Assert.Equal(HttpStatusCode.Redirect, post.StatusCode);
    //    string? location = post.Headers.Location?.ToString();
    //    Assert.NotNull(location);

    //    var details = await client.GetAsync(location!);
    //    Assert.Equal(HttpStatusCode.OK, details.StatusCode);
    //    string html = await details.Content.ReadAsStringAsync();
    //    Assert.Contains("IT Test Movie", html);
    //    Assert.Contains("Action", html);
    //}
}
