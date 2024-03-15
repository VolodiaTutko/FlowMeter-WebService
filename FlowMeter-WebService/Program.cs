using Microsoft.EntityFrameworkCore;
using Serilog;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Application.DataAccess;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName();
});

// Add services to the container.
builder.Services.AddControllersWithViews();
AppDbContextExtensions.AddApplicationDbContext(
    builder.Services,
    "Host=localhost; Port=5432; Database=flowmeterWeb; Username=postgres; Password=12345"
//Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
);
builder.Services.AddTransient<IHouseRepository, HouseRepository>();
builder.Services.AddTransient<HouseService>();

var app = builder.Build();

app.UseSerilogRequestLogging(options => {
    options.EnrichDiagnosticContext = WebApplication1.Logger.Enricher.HttpRequestEnricher;
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
