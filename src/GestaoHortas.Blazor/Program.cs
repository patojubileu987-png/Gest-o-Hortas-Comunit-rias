using GestaoHortas.Blazor.Components;
using GestaoHortas.Library.Application.Interfaces;
using GestaoHortas.Library.Infra.Repositories;
using GestaoHortas.Library.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5142");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ICanteiroRepository, InMemoryCanteiroRepository>();
builder.Services.AddSingleton<ICultivoRepository, InMemoryCultivoRepository>();
builder.Services.AddSingleton<CultivoCompatibilityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
