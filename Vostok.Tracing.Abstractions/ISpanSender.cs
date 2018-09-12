using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <see cref="ISpanSender"/>'s purpose is to send spans built by <see cref="ISpanBuilder"/> to external storage for further processing.
    /// </summary>
    [PublicAPI]
    public interface ISpanSender
    {
        SpanSendResult Send([NotNull] ISpan span);
    }
}