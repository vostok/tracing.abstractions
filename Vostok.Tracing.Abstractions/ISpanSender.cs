using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public interface ISpanSender
    {
        SpanSendResult Send([NotNull]ISpan span);
    }
}