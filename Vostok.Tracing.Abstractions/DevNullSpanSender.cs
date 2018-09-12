using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// A trivial <see cref="ISpanSender"/> implementation that simply drops all incoming spans and returns <see cref="SpanSendResult.Sent"/> results.
    /// </summary>
    [PublicAPI]
    public class DevNullSpanSender : ISpanSender
    {
        public SpanSendResult Send(ISpan span) => SpanSendResult.Sent;
    }
}