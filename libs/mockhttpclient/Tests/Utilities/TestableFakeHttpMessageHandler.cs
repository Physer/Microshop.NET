using Microshop.MockHttpClient;

namespace Tests.Utilities;

internal class TestableFakeHttpMessageHandler(IEnumerable<FakeHttpMessage?> httpMessages) : FakeHttpMessageHandler(httpMessages)
{
    public Task<HttpResponseMessage> TestableSendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => SendAsync(request, cancellationToken);
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => base.SendAsync(request, cancellationToken);
}
