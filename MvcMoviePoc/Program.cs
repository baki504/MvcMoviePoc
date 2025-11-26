using Microsoft.EntityFrameworkCore;
using MvcMoviePoc.Data;
using MvcMoviePoc.Factories;
using MvcMoviePoc.Repositories;
using MvcMoviePoc.Services;
using MvcMoviePoc.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcMoviePocContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMoviePocContext") ?? throw new InvalidOperationException("Connection string 'MvcMoviePocContext' not found.")));

builder.Services.AddControllersWithViews();

// DI registration (Service????)
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<MovieViewModelFactory>();
builder.Services.AddScoped<MovieService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
