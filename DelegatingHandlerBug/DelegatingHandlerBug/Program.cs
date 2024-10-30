using DelegatingHandlerBug;
using DelegatingHandlerBug.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options => { options.DetailedErrors = true; })
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<DelegatingHandlerBug.JsInteropDelegatingHandler>();
builder.Services.AddHttpClient("jsInteropHandler")
    .AddHttpMessageHandler<DelegatingHandlerBug.JsInteropDelegatingHandler>();

builder.Services.AddCircuitServicesAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DelegatingHandlerBug.Client._Imports).Assembly);

app.Run();