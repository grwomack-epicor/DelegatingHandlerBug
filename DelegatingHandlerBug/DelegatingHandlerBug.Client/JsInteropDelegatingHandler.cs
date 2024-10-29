using Microsoft.JSInterop;

namespace DelegatingHandlerBug.Client;

public class JsInteropDelegatingHandler : DelegatingHandler
{
    // Private Fields
    private IJSRuntime _jsRuntime;

    // Public Methods
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _jsRuntime.InvokeVoidAsync("alert", cancellationToken, $"Hello from {nameof(JsInteropDelegatingHandler)}");
        return await base.SendAsync(request, cancellationToken);
    }

    // Constructor
    public JsInteropDelegatingHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
}