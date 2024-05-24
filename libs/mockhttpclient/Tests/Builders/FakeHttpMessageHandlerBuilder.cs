using Microshop.MockHttpClient;
using Tests.Utilities;

namespace Tests.Builders;

internal class FakeHttpMessageHandlerBuilder
{
    private IEnumerable<FakeHttpMessage> _messages = [];

    public FakeHttpMessageHandlerBuilder WithMessages(IEnumerable<FakeHttpMessage> messages)
    {
        _messages = messages;

        return this;
    }

    public TestableFakeHttpMessageHandler Build() => new(_messages);
}
