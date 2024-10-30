using Microsoft.JSInterop;

namespace DelegatingHandlerBug;

public class JsInteropDelegatingHandler : DelegatingHandler
{
    // Private Fields
    private CircuitServicesAccessor _servicesAccessor;

    // Public Methods
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jsRunTime = _servicesAccessor.Services.GetRequiredService<IJSRuntime>();
        await jsRunTime.InvokeVoidAsync("alert", cancellationToken, $"Hello from {nameof(JsInteropDelegatingHandler)}");
        return await base.SendAsync(request, cancellationToken);
    }

    // Constructor
    public JsInteropDelegatingHandler(CircuitServicesAccessor servicesAccessor)
    {
        _servicesAccessor = servicesAccessor;
    }
}