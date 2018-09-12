using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// Represents the result of sending a single <see cref="ISpan"/> to external storage with <see cref="ISpanSender"/>.
    /// </summary>
    [PublicAPI]
    public enum SpanSendResult
    {
        /// <summary>
        /// The span was successfully sent and <see cref="ISpanSender"/> implementation no longer holds any references to <see cref="ISpan"/> object.
        /// </summary>
        Sent,

        /// <summary>
        /// The span was put into an internal queue and <see cref="ISpanSender"/> holds a reference to provided <see cref="ISpan"/> object.
        /// </summary>
        Queued,

        /// <summary>
        /// The span could not be sent and <see cref="ISpanSender"/> implementation no longer holds any references to <see cref="ISpan"/> object.
        /// </summary>
        Failed
    }
}