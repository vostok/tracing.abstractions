using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public class SilentSpanSender : ISpanSender
    {
        public SpanSendResult Send(ISpan span) => SpanSendResult.Sent;
    }
}