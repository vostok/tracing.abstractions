using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public class DevNullSpanSender : ISpanSender
    {
        public SpanSendResult Send(ISpan span) => SpanSendResult.Sent;
    }
}